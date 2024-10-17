using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class DiceHandler : MonoBehaviour
{

    public enum DiceModes { Compact, Expand}

    
    [SerializeField] FaceHandler _presentationFaceHandler = null;
    [SerializeField] SlotHandler[] _slotHandlers = null;
    //[SerializeField] FaceHandler[] _reserveFaceHandlers = null;

    //settings
    [SerializeField] float _rollTime_Min = 2f;
    [SerializeField] float _rollTime_Max = 4f;
    [SerializeField] float _timeFactorCoefficient_Min = .25f;
    [SerializeField] float _timeFactorCoefficient_Max = .5f;
    [SerializeField] Vector3 _jumpHeight = new Vector3(0, .5f, 0);
    [SerializeField] float[] _xPos = new float[5];
    [SerializeField] float _expandTime = 1f;

    //state
    DiceModes _diceMode = DiceModes.Compact;
    [SerializeField] Dice _dice;
    Dice.DiceTypes _diceType;
    public Dice.DiceTypes DiceType => _diceType;

    [SerializeField] DiceFace _activeFace;
    public DiceFace ActiveFace => _activeFace;
    float _elapsedRollTime;
    int _rand;
    float _rollTime;
    float _timeFactor;
    float _timeOnCurrentFace;
    bool _isRolling = false;
    float _timeFactorCoefficient;
    Tween _finalJumpTween;
    Tween[] _reserveMoveTweens = new Tween[6];


    private void Start()
    {
        _presentationFaceHandler.SetDiceParent(this);    

    }

    public void LoadWithDice(Dice dice)
    {
        _elapsedRollTime = 0;
        _dice = dice;
        _diceType = dice.DiceType;

        DiceFace[] loadedFaces = _dice.GetFaces();

        for (int i = 0; i < loadedFaces.Length; i++)
        {
            FaceHandler newFace = DiceLibrary.Instance.CreateFaceTile(loadedFaces[i], _slotHandlers[i].transform);
            _slotHandlers[i].RegisterNewFaceInSlot(loadedFaces[i], newFace);
            newFace.SetInitialSlotHandler(_slotHandlers[i]);
        }

        //_activeFaceHandler.SetDiceParent(this);
        //foreach (var face in _loadedFaces)
        //{
        //    face.SetDiceParent(this);
        //}

        _activeFace = loadedFaces[0];
        RenderPresentationFace(_activeFace);
        SetDiceMode(DiceModes.Compact, true);
    }

    [ContextMenu("Roll Dice")]
    public void RollDice()
    {
        if (_diceMode == DiceModes.Expand)
        {
            Debug.Log("Cannot roll die while expanded");
            return;
        }
        _elapsedRollTime = 0;
        _timeOnCurrentFace = 0;
        _timeFactor = 1;
        _isRolling = true;
        _rollTime = UnityEngine.Random.Range(_rollTime_Min, _rollTime_Max);
        _timeFactorCoefficient = UnityEngine.Random.Range(_timeFactorCoefficient_Min, _timeFactorCoefficient_Max);

        int sign = (2 * UnityEngine.Random.Range(0, 2)) - 1;
        _finalJumpTween.Kill();
        _finalJumpTween = transform.DOPunchPosition(sign * _jumpHeight, _rollTime);
    }

    private void Update()
    {
        if (_isRolling)
        {
            _elapsedRollTime += Time.deltaTime;
            _timeOnCurrentFace += Time.deltaTime;
            _timeFactor = _elapsedRollTime / _rollTime;


            if (_timeOnCurrentFace > _timeFactor * _timeFactorCoefficient)
            {
                _timeOnCurrentFace = 0;
                _timeFactorCoefficient = UnityEngine.Random.Range(_timeFactorCoefficient_Min, _timeFactorCoefficient_Max);
                SetRandomDiceFaceAsActiveFace();
            }
        }
        if (_isRolling && _elapsedRollTime >= _rollTime)
        {
            //Show the final dice face
            SetRandomDiceFaceAsActiveFace();
            _isRolling = false;
        }
    }

    private void SetRandomDiceFaceAsActiveFace()
    {
        _rand = UnityEngine.Random.Range(0, _slotHandlers.Length);
        if (_slotHandlers[_rand].DiceFaceInSlot)
        {
            _activeFace = _slotHandlers[_rand].DiceFaceInSlot;
        }
        else
        {
            _activeFace = null;
        }
        RenderPresentationFace(_activeFace);
    }

    private void RenderPresentationFace(DiceFace faceToRender)
    {
        _presentationFaceHandler.SetFace(faceToRender);
    }



    #region Dice Mode
    public void SetDiceMode(DiceModes diceMode, bool isInstantChange)
    {
        if (_isRolling) return;

        float time = isInstantChange ? 0.001f : _expandTime;

        switch (diceMode)
        {
            case DiceModes.Compact:
                CompactReserveDiceFaces(time);
                break;
                 
            case DiceModes.Expand:
                ExpandReserveDiceFaces(time);
                break;
        }
    }

    private void ExpandReserveDiceFaces(float time)
    {
        _presentationFaceHandler.gameObject.SetActive(false);
        foreach (var slot in _slotHandlers)
        {
            slot.gameObject.SetActive(true);
        }

        //List<DiceFace> reserveFaces = new List<DiceFace>();

        //foreach (var face in _loadedFaces)
        //{
        //    if (face == _activeFace) continue;
        //    else
        //    {
        //        reserveFaces.Add(face);
        //    }
        //}

        //int breaker = 9;
        //while (reserveFaces.Count < _slotHandlers.Length)
        //{
        //    reserveFaces.Add(null);
        //    breaker--;
        //    if (breaker == 0) break;
        //}
        
        for (int i = 0; i < _slotHandlers.Length; i++)
        {
            //_reserveFaceHandlers[i].SetFace(reserveFaces[i]);
            _reserveMoveTweens[i].Kill();
            _reserveMoveTweens[i] = _slotHandlers[i].transform.DOLocalMoveX(_xPos[i], time).
                SetEase(Ease.OutBack).OnComplete(HandleExpandCompleted);
        }
    }

    private void HandleExpandCompleted()
    {

        _diceMode = DiceModes.Expand;
        foreach (var slot in _slotHandlers)
        {
            slot.FaceHandlerInSlot.SetNewDiceMode(DiceModes.Expand);
        }
    }

    private void CompactReserveDiceFaces(float time)
    {
        for (int i = 0; i < _slotHandlers.Length; i++)
        {
            _reserveMoveTweens[i].Kill();
            _reserveMoveTweens[i] = _slotHandlers[i].transform.DOLocalMoveX(0, time).
                SetEase(Ease.InBack).OnComplete(HandleCompactCompleted);
        }
    }

    private void HandleCompactCompleted()
    {
        _presentationFaceHandler.gameObject.SetActive(true);
        _activeFace = _slotHandlers[0].DiceFaceInSlot;
        RenderPresentationFace(_activeFace);
        foreach (var slot in _slotHandlers)
        {
            slot.gameObject.SetActive(false);
        }
        _diceMode = DiceModes.Compact;
        foreach (var slot in _slotHandlers)
        {
            slot.FaceHandlerInSlot.SetNewDiceMode(DiceModes.Compact);
        }
    }


    [ContextMenu("Expand")]
    public void Expand_Debug()
    {
        SetDiceMode(DiceModes.Expand, false);
    }

    [ContextMenu("Compact")]
    public void Compact_Debug()
    {
        SetDiceMode(DiceModes.Compact, false);
    }

    #endregion
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class DiceHandler : MonoBehaviour
{
    public enum DiceModes { Compact, Expand}

    [SerializeField] FaceHandler _activeFaceHandler = null;
    [SerializeField] FaceHandler[] _reserveFaceHandlers = null;

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
    Dice _dice;
    DiceFace[] _loadedFaces;

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
    Tween[] _reserveMoveTweens = new Tween[5];

    public void LoadWithDice(Dice dice)
    {
        _elapsedRollTime = 0;
        _dice = dice;
        _loadedFaces = _dice.GetFaces();
        SetRandomDiceFace();
        SetDiceMode(DiceModes.Compact, true);
    }

    [ContextMenu("Roll Dice")]
    public void RollDice()
    {
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
                SetRandomDiceFace();
            }
        }
        if (_isRolling && _elapsedRollTime >= _rollTime)
        {
            //Show the final dice face
            SetRandomDiceFace();
            _isRolling = false;
        }
    }

    private void SetRandomDiceFace()
    {
        _rand = UnityEngine.Random.Range(0, _loadedFaces.Length);
        _activeFace = _loadedFaces[_rand];
        RenderDice();
    }

    private void RenderDice()
    {
        _activeFaceHandler.SetFace(_activeFace);
    }

    public void SetDiceMode(DiceModes diceMode, bool isInstantChange)
    {
        float time = isInstantChange ? 0.001f : _expandTime;

        switch (diceMode)
        {
            case DiceModes.Compact:
                CompactReserveDiceFaces(time);
                //if (_diceMode != DiceModes.Compact)
                //{
                //    CompactReserveDiceFaces(time);
                //}
                break;
                 
            case DiceModes.Expand:
                //if (_diceMode != DiceModes.Expand)
                //{
                //    ExpandReserveDiceFaces(time);
                //}
                ExpandReserveDiceFaces(time);
                break;
        }
    }

    private void ExpandReserveDiceFaces(float time)
    {
        for (int i = 0; i < _reserveFaceHandlers.Length; i++)
        {
            _reserveMoveTweens[i].Kill();
            _reserveFaceHandlers[i].transform.DOLocalMoveX(_xPos[i], time).
                SetEase(Ease.InSine).OnComplete(HandleExpandCompleted);
        }
    }

    private void HandleExpandCompleted()
    {
        _diceMode = DiceModes.Expand;
    }

    private void CompactReserveDiceFaces(float time)
    {
        for (int i = 0; i < _reserveFaceHandlers.Length; i++)
        {
            _reserveMoveTweens[i].Kill();
            _reserveFaceHandlers[i].transform.DOLocalMoveX(0, time).
                SetEase(Ease.InSine).OnComplete(HandleCompactCompleted);
        }
    }

    private void HandleCompactCompleted()
    {
        _diceMode = DiceModes.Compact;
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
}

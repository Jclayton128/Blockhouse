using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class DiceHandler : MonoBehaviour
{    
    //[SerializeField] FaceHandler _presentationFaceHandler = null;
    [SerializeField] SlotHandler[] _slotHandlers = null;
    //[SerializeField] FaceHandler[] _reserveFaceHandlers = null;

    //settings
    [SerializeField] float _rollTime_Min = 2f;
    [SerializeField] float _rollTime_Max = 4f;
    [SerializeField] float _timeFactorCoefficient_Min = .25f;
    [SerializeField] float _timeFactorCoefficient_Max = .5f;
    [SerializeField] Vector3 _jumpHeight = new Vector3(0, .5f, 0);
    //[SerializeField] float[] _xPos = new float[5];
    [SerializeField] float _expandTime = 1f;
    [SerializeField] float _fadeTime = 0.5f;

    //state
    [SerializeField] Dice _dice;
    //Dice.DiceTypes _diceType;
    //public Dice.DiceTypes DiceType => _diceType;
    [SerializeField] bool _isExpanded = false;
    public bool IsExpanded => _isExpanded;
    [SerializeField] SlotHandler _activeSlot;
    public SlotHandler ActiveFace => _activeSlot;
    float _elapsedRollTime;
    int _rand;
    float _rollTime;
    float _timeFactor;
    float _timeOnCurrentFace;
    bool _isRolling = false;
    float _timeFactorCoefficient;
    Tween _finalJumpTween;
    Tween[] _reserveMoveTweens = new Tween[6];
    ActorHandler _owningActor;
    List<Vector3> _expandPositions = new List<Vector3>();
    [SerializeField] bool _isLocked = false;

    private void Start()
    {
        //_presentationFaceHandler.SetDiceParent(this);   
    }

    public void SetOwningActor(ActorHandler owner)
    {
        _owningActor = owner;
    }

    public void LoadWithDice(Dice dice)
    {
        _elapsedRollTime = 0;
        _dice = dice;

        foreach (var slot in _slotHandlers)
        {
            slot.SetAsSans(true);
            _expandPositions.Add(slot.transform.localPosition);
        }

        DiceFace[] loadedFaces = _dice.GetFaces();

        for (int i = 0; i < loadedFaces.Length; i++)
        {
            FaceHandler newFace = DiceLibrary.Instance.CreateFaceTile(loadedFaces[i], _slotHandlers[i].transform);
            _slotHandlers[i].RegisterNewFaceInSlot(loadedFaces[i], newFace);
            newFace.SetInitialSlotHandler(_slotHandlers[i]);
        }
        _activeSlot = _slotHandlers[0];
        _isExpanded = false;
        Vis_DeactivateDice();
        Vis_CompactFadeAwayDeactivateDice();
    }

    public void RollDice()
    {
        if (_isExpanded)
        {
            Debug.Log("Cannot roll die while expanded");
            return;
        }

        if (_isLocked)
        {
            Debug.Log("Can't roll while locked.");
            transform.DOPunchPosition(transform.up/2f, _rollTime/4, 8);
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
        if (_slotHandlers[_rand] != null)
        {
            //Debug.Log("valid face");
            _activeSlot = _slotHandlers[_rand];
        }
        else
        {
            //Debug.Log($"null @ {_rand}");
            _activeSlot = null;
        }
        //RenderPresentationFace(_activeSlot);
        RenderPresentationFace();
    }

    private void RenderPresentationFace()
    {
        foreach (var slot in _slotHandlers)
        {
            slot.gameObject.SetActive(false);
        }

        _activeSlot.gameObject.SetActive(true);
    }

    #region Visuals
    /// <summary>
    /// All dice visual interactions must go through one of these functions.
    /// </summary>
    /// 

    public bool Vis_ToggleExpandCompactDice()
    {
        _isExpanded = !_isExpanded;
        if (_isExpanded)
        {
            ExpandFaces();
        }
        else
        {
            CompactFaces();
        }
        return _isExpanded;
    }
   
    public void Vis_DeactivateDice()
    {
        DeactivateFaces();
    }

    public void Vis_ActivateFadeInActiveFace()
    {
        ActivateActiveFace();
        FadeInDice();
    }

    public void Vis_ActivateFadeInExpandDice()
    {
        ActivateFaces();
        FadeInDice();
        ExpandFaces();
    }

    public void Vis_CompactFadeAwayDeactivateDice()
    {
        CompactFaces();
        FadeAwayDice();
        Invoke(nameof(DeactivateFaces), _fadeTime);
    }

    public void Vis_CompactToSingleVisibleDice()
    {
        CompactFaces();
        ActivateActiveFace();
    }


    #endregion

    #region Visual Helpers
    private void FadeInDice()
    {
        var srs = GetComponentsInChildren<SpriteRenderer>(true);
        foreach (var sr in srs)
        {

            sr.DOFade(1, _fadeTime);
        }
        //_isHidden = false;
    }

    //private void FadeInActiveFace()
    //{
    //    var srs = _activeSlot.GetComponentsInChildren<SpriteRenderer>(true);
    //    foreach (var sr in srs)
    //    {
    //        sr.DOFade(1, _fadeTime);
    //    }
    //}

    private void FadeAwayDice()
    {
        //Debug.Log("Hiding Dice", this);
        var srs = GetComponentsInChildren<SpriteRenderer>(true);
        //float fade = isInstant ? 0.001f : _fadeTime;
        foreach (var sr in srs)
        {
            sr.DOFade(0, _fadeTime);
        }
    }
    private void ExpandFaces()
    {
        foreach (var slot in _slotHandlers)
        {
            slot.gameObject.layer = 7;
        }

        ModifyExpandPositionBasedOnParentPosition();

        for (int i = 0; i < _slotHandlers.Length; i++)
        {
            _reserveMoveTweens[i].Kill();
            _reserveMoveTweens[i] = _slotHandlers[i].transform.DOLocalMove(
                _expandPositions[i], _expandTime).
                SetEase(Ease.OutBack).OnComplete(HandleExpandCompleted);
        }

        if (GameController.Instance.GameMode == GameController.GameModes.EncounterInspection)
        {
            UIController.Instance.ShowInspectionPanels();
        }
    }

    private void ModifyExpandPositionBasedOnParentPosition()
    {
        if (transform.root.transform.position.x <= -10.5f)
        {
            _expandPositions[0] = new Vector3(0, 0, 0);
            _expandPositions[1] = new Vector3(3.5f, 0, 0);
            _expandPositions[2] = new Vector3(7.0f, 0, 0);
            _expandPositions[3] = new Vector3(10.5f, 0, 0);
            _expandPositions[4] = new Vector3(14f, 0, 0);
            _expandPositions[5] = new Vector3(17.5f, 0, 0);
        }
        else if (transform.root.transform.position.x <= -7.0f)
        {
            _expandPositions[5] = new Vector3(-3.5f, 0, 0);
            _expandPositions[0] = new Vector3(0, 0, 0);
            _expandPositions[1] = new Vector3(3.5f, 0, 0);
            _expandPositions[2] = new Vector3(7.0f, 0, 0);
            _expandPositions[3] = new Vector3(10.5f, 0, 0);
            _expandPositions[4] = new Vector3(14f, 0, 0);

        }
        else if (transform.root.transform.position.x <= -3.5f)
        {
            _expandPositions[5] = new Vector3(-3.5f, 0, 0);
            _expandPositions[0] = new Vector3(0, 0, 0);
            _expandPositions[1] = new Vector3(3.5f, 0, 0);
            _expandPositions[2] = new Vector3(7.0f, 0, 0);
            _expandPositions[3] = new Vector3(10.5f, 0, 0);
            _expandPositions[4] = new Vector3(14f, 0, 0);

            //_expandPositions[5] = new Vector3(-7.0f, 0, 0);
            //_expandPositions[4] = new Vector3(-3.5f, 0, 0);
            //_expandPositions[0] = new Vector3(0, 0, 0);
            //_expandPositions[1] = new Vector3(3.5f, 0, 0);
            //_expandPositions[2] = new Vector3(7.0f, 0, 0);
            //_expandPositions[3] = new Vector3(10.5f, 0, 0);

        }
        else if (Mathf.Abs(transform.root.transform.position.x) <= Mathf.Epsilon)
        {
            _expandPositions[5] = new Vector3(-7.0f, 0, 0);
            _expandPositions[4] = new Vector3(-3.5f, 0, 0);
            _expandPositions[0] = new Vector3(0, 0, 0);
            _expandPositions[1] = new Vector3(3.5f, 0, 0);
            _expandPositions[2] = new Vector3(7.0f, 0, 0);
            _expandPositions[3] = new Vector3(10.5f, 0, 0);

            //_expandPositions[5] = new Vector3(-10.5f, 0, 0);
            //_expandPositions[4] = new Vector3(-7.0f, 0, 0);
            //_expandPositions[3] = new Vector3(-3.5f, 0, 0);
            //_expandPositions[0] = new Vector3(0, 0, 0);
            //_expandPositions[1] = new Vector3(3.5f, 0, 0);
            //_expandPositions[2] = new Vector3(7.0f, 0, 0);
        }
        else if (transform.root.transform.position.x >= 10.4f)
        {
            _expandPositions[5] = new Vector3(-17.5f, 0, 0);
            _expandPositions[4] = new Vector3(-14.0f, 0, 0);
            _expandPositions[3] = new Vector3(-10.5f, 0, 0);
            _expandPositions[2] = new Vector3(-7.0f, 0, 0);
            _expandPositions[1] = new Vector3(-3.5f, 0, 0);
            _expandPositions[0] = new Vector3(0, 0, 0);
        }
        else if (transform.root.transform.position.x >= 7.0f)
        {
            _expandPositions[5] = new Vector3(-13.5f, 0, 0);
            _expandPositions[4] = new Vector3(-10.5f, 0, 0);
            _expandPositions[3] = new Vector3(-7.0f, 0, 0);
            _expandPositions[2] = new Vector3(-3.5f, 0, 0);
            _expandPositions[0] = new Vector3(0, 0, 0);
            _expandPositions[1] = new Vector3(3.5f, 0, 0);
        }
        else if (transform.root.transform.position.x >= 3.5f)
        {
            _expandPositions[5] = new Vector3(-10.5f, 0, 0);
            _expandPositions[4] = new Vector3(-7.0f, 0, 0);
            _expandPositions[3] = new Vector3(-3.5f, 0, 0);
            _expandPositions[0] = new Vector3(0, 0, 0);
            _expandPositions[1] = new Vector3(3.5f, 0, 0);
            _expandPositions[2] = new Vector3(7.0f, 0, 0);
        }
    }

    private void HandleExpandCompleted()
    {
        _isExpanded = true;
        foreach (var slot in _slotHandlers)
        {
            slot.FaceHandlerInSlot.SetGrabbableStatus(_isExpanded);
        }
    }

    private void CompactFaces()
    {
        foreach (var slot in _slotHandlers)
        {
            slot.gameObject.layer = 0;
        }

        for (int i = 0; i < _slotHandlers.Length; i++)
        {
            _reserveMoveTweens[i].Kill();
            _reserveMoveTweens[i] = _slotHandlers[i].transform.DOLocalMove(
                Vector3.zero, _expandTime).
                SetEase(Ease.InBack).OnComplete(HandleCompactCompleted);
        }
    }

    private void HandleCompactCompleted()
    {
        //foreach (var slot in _slotHandlers)
        //{
        //    slot.gameObject.SetActive(false);
        //}

        _isExpanded = false;
        foreach (var slot in _slotHandlers)
        {
            slot.FaceHandlerInSlot?.SetGrabbableStatus(_isExpanded);
        }
    }

    private void ActivateFaces()
    {
        foreach (var slot in _slotHandlers)
        {
            slot.gameObject.SetActive(true);
        }
    }

    private void ActivateActiveFace()
    {
        foreach (var slot in _slotHandlers)
        {
            slot.gameObject.SetActive(false);
        }
        _activeSlot.gameObject.SetActive(true);
    }

    private void DeactivateFaces()
    {
        foreach (var slot in _slotHandlers)
        {
            slot.gameObject.SetActive(false);
        }
    }

    #endregion

    #region Select Dice

    public bool ToggleDiceLockStatus()
    {
        if (_isRolling) return false;

        _isLocked = !_isLocked;
        Debug.Log("lock status toggled");
        if (_isLocked) ShowDiceAsSelected();
        else ShowDiceAsDeselected();

        return _isLocked;
    }

    public void ShowDiceAsSelected()
    {
        //Depict dice as selected
        transform.localScale = Vector3.one * 1.2f;
    }

    public void ShowDiceAsDeselected()
    {
        //Depict dice as no longer selected
        transform.localScale = Vector3.one ;
    }

    #endregion
}

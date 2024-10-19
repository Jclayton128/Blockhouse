using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FaceHandler : MonoBehaviour
{
    [SerializeField] DiceFace _startingDiceFace_Debug = null;
    [SerializeField] SpriteRenderer _edgeSR = null;
    [SerializeField] SpriteRenderer _bandSR = null;
    [SerializeField] SpriteRenderer _fillSR = null;
    [SerializeField] SpriteRenderer _iconSR = null;

    //state
    int _restingSortIndex;
    DiceHandler _parentDiceHandler;
    DiceFace _diceFaceRepresented;
    SlotHandler _mostRecentSlotHandler;
    private void Awake()
    {
        _homePos = transform.position;
    }

    private void Start()
    {
        if (_startingDiceFace_Debug)
        {
            SetBaseSortOrder(0);
            _diceFaceRepresented = _startingDiceFace_Debug;
            SetFace(_startingDiceFace_Debug);
        }
    }

    public void SetDiceParent(DiceHandler parent)
    {
        _parentDiceHandler = parent;
    }

    public void SetInitialSlotHandler(SlotHandler sh)
    {
        _mostRecentSlotHandler = sh;
    }

    #region Displaying
    public void SetNewDiceMode(DiceHandler.DiceModes newDiceMode)
    {
        if (newDiceMode == DiceHandler.DiceModes.Compact)
        {
            _isGrabbable = false;
        }
        if (newDiceMode == DiceHandler.DiceModes.Expand)
        {
            _isGrabbable = true;
        }
    }

    public void SetBaseSortOrder(int sortOrder)
    {
        _restingSortIndex = sortOrder;
        _fillSR.sortingOrder = (_restingSortIndex * 5) + 0;
        _bandSR.sortingOrder = (_restingSortIndex * 5) + 1;
        _iconSR.sortingOrder = (_restingSortIndex * 5) + 2;
        _edgeSR.sortingOrder = (_restingSortIndex * 5) + 3;
    }

    public void PrioritizeSortOrder()
    {
        int order = 100;
        _fillSR.sortingOrder = (order * 5) + 0;
        _bandSR.sortingOrder = (order * 5) + 1;
        _iconSR.sortingOrder = (order * 5) + 2;
        _edgeSR.sortingOrder = (order * 5) + 3;
    }

    public void DeprioritizeSortOrder()
    {
        _fillSR.sortingOrder = (_restingSortIndex * 5) + 0;
        _bandSR.sortingOrder = (_restingSortIndex * 5) + 1;
        _iconSR.sortingOrder = (_restingSortIndex * 5) + 2;
        _edgeSR.sortingOrder = (_restingSortIndex * 5) + 3;
    }

    public void SetFace(DiceFace diceFace)
    {
        _diceFaceRepresented = diceFace;

        if (_diceFaceRepresented == null)
        {
            _iconSR.sprite = null;
            _bandSR.color = Color.white;
            _edgeSR.color = Color.black;
            _fillSR.color = Color.gray;
            switch (_parentDiceHandler.DiceType)
            {
                case Dice.DiceTypes.Light:
                    _fillSR.sprite = DiceLibrary.Instance.FillLight;
                    _bandSR.sprite = DiceLibrary.Instance.BandLight;
                    _edgeSR.sprite = DiceLibrary.Instance.EdgeLight;
                    break;

                case Dice.DiceTypes.Medium:
                    _fillSR.sprite = DiceLibrary.Instance.FillMedium;
                    _bandSR.sprite = DiceLibrary.Instance.BandMedium;
                    _edgeSR.sprite = DiceLibrary.Instance.EdgeMedium;
                    break;

                case Dice.DiceTypes.Heavy:
                    _fillSR.sprite = DiceLibrary.Instance.FillHeavy;
                    _bandSR.sprite = DiceLibrary.Instance.BandHeavy;
                    _edgeSR.sprite = DiceLibrary.Instance.EdgeHeavy;
                    break;


            }
            return;
        }

        _iconSR.sprite = diceFace.FaceSprite;

        switch (_diceFaceRepresented.FaceType)
        {
            case DiceFace.FaceTypes.Beast:
                _fillSR.color = DiceLibrary.Instance.ColorBeast;
                break;

            case DiceFace.FaceTypes.Discord:
                _fillSR.color = DiceLibrary.Instance.ColorDiscord;
                break;

            case DiceFace.FaceTypes.Hearth:
                _fillSR.color = DiceLibrary.Instance.ColorHearth;
                break;

            case DiceFace.FaceTypes.Order:
                _fillSR.color = DiceLibrary.Instance.ColorOrder;
                break;

            case DiceFace.FaceTypes.Arcane:
                _fillSR.color = DiceLibrary.Instance.ColorArcane;
                break;

            case DiceFace.FaceTypes.Nomad:
                _fillSR.color = DiceLibrary.Instance.ColorNomad;
                break;

        }

        switch (_diceFaceRepresented.DiceType)
        {
            case Dice.DiceTypes.Light:
                _edgeSR.sprite = DiceLibrary.Instance.EdgeLight;
                _bandSR.sprite = DiceLibrary.Instance.BandLight;
                _fillSR.sprite = DiceLibrary.Instance.FillLight;
                break;

            case Dice.DiceTypes.Medium:
                _edgeSR.sprite = DiceLibrary.Instance.EdgeMedium;
                _bandSR.sprite = DiceLibrary.Instance.BandMedium;
                _fillSR.sprite = DiceLibrary.Instance.FillMedium;
                break;

            case Dice.DiceTypes.Heavy:
                _edgeSR.sprite = DiceLibrary.Instance.EdgeHeavy;
                _bandSR.sprite = DiceLibrary.Instance.BandHeavy;
                _fillSR.sprite = DiceLibrary.Instance.FillHeavy;
                break;



        }

        switch (_diceFaceRepresented.DiceLevel)
        {
            case DiceFace.DiceLevels.Basic:
                _bandSR.color = DiceLibrary.Instance.Level0;
                break;

            case DiceFace.DiceLevels.Good:
                _bandSR.color = DiceLibrary.Instance.Level1;
                break;

            case DiceFace.DiceLevels.Better:
                _bandSR.color = DiceLibrary.Instance.Level2;
                break;

            case DiceFace.DiceLevels.Best:
                _bandSR.color = DiceLibrary.Instance.Level3;
                break;
        }
    }

    #endregion

    #region Grabbing/Moving 

    [SerializeField] float _homeMoveRate = 1f;
    int _layerMask_Slot = 1 << 7;
    Vector3 _posDelta = new Vector3(0, 0, -0.1f);
    [SerializeField] float _overshoot = 1.7f;

    //state
    float _dist;
    float _time;
    public Vector3 _homePos;
    public bool _isGrabbable = true;
    bool _isGrabbed;
    Vector3 _mouseTransformDelta = Vector3.zero;
    bool _isTweening = false;
    Tween _moveTween;

    //private void OnMouseOver()
    //{
    //    if (!_isGrabbable) return;
    //}

    private void OnMouseDown()
    {
        if (!_isGrabbable || _isTweening) return;
        PrioritizeSortOrder();
        _isGrabbed = true;
        _mouseTransformDelta = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _homePos = transform.position;
    }

    private void OnMouseUp()
    {
        _isGrabbed = false;
        DeprioritizeSortOrder();
        //Check if on an open Slot
        var newHome = Physics2D.OverlapCircle(transform.position, 1f, _layerMask_Slot);
        SlotHandler sh;

        if (newHome && newHome.TryGetComponent<SlotHandler>(out sh))
        {
            if (!sh.CheckDiceTypeAgainstSlotType(_diceFaceRepresented))
            {
                //AUDIO face install denied due to mismatch
                SendHome();
                return;
            }

            if (_mostRecentSlotHandler)
            {
                _mostRecentSlotHandler.ClearFaceHandlerFromSlot();
                sh.FaceHandlerInSlot?.PushToNewHome(_mostRecentSlotHandler);
            }
            else
            {
                sh.FaceHandlerInSlot?.PushToNewHome(_homePos);
            }

            SetNewHome(newHome, sh);
            //check if SH has a face already. If yes, send current FH to this FH's home
        }
        else
        {
            //If no, then return Home via tween
            SendHome();
        }


    }

    private void SetNewHome(Collider2D hits, SlotHandler sh)
    {
        transform.parent = hits.transform;
        transform.localPosition = _posDelta;

        
        sh.RegisterNewFaceInSlot(_diceFaceRepresented, this);
        _mostRecentSlotHandler = sh;

        _homePos = transform.position;
    }

    public void PushToNewHome(SlotHandler sh)
    {
        _homePos = sh.transform.position + _posDelta;
        transform.parent = sh.transform;
        sh.RegisterNewFaceInSlot(_diceFaceRepresented, this);
        _mostRecentSlotHandler = sh;
        SendHome();
    }

    public void PushToNewHome(Vector3 newHome)
    {
        _homePos = newHome;
        transform.parent = null;
        _mostRecentSlotHandler = null;
        SendHome();
    }

    private void Update()
    {
        if (_isGrabbable && _isGrabbed)
        {
            transform.position = (Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition) + _mouseTransformDelta);
        }
    }

    public void SendHome()
    {
        Debug.Log("sent home", this);
        _isTweening = true;
        
        
        _moveTween.Kill();
        _dist = (_homePos - transform.position).magnitude;
        _time = _dist / _homeMoveRate;

        _moveTween = transform.DOMove(_homePos, _time).SetEase(Ease.OutBack, _overshoot).
            OnComplete(HandleSentHome);
    }

    private void HandleSentHome()
    {
        _isTweening = false;

    }

    #endregion
}

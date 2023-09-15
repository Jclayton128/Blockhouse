using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHandler : MonoBehaviour
{
    public Action BuildingConquered;

    public enum BuildingType { Undefined, Blockhouse, Farm}

    [SerializeField] SpriteRenderer[] _banners = null;
    [SerializeField] SpriteRenderer _flag = null;
    [SerializeField] Vector3 _flagLow = new Vector3(0, 0,0);
    [SerializeField] Vector3 _flagHigh = new Vector3(0, 2,0);
    /// <summary>
    /// How fast the flag moves per second, in world units
    /// </summary>
    float _raiseRate = 1f;


    [SerializeField] int _startingOwner = 0;

    //state 
    [SerializeField] int _flagMoveDir = 0;
    [SerializeField] bool _isAutoRaisingOrLowering = false;
    int _conquerorAllegiance = 0;

    /// <summary>
    /// Evil is -1, Good is 1, Neutral/Unclaimed is 0
    /// </summary>
    public int Owner;// { get; private set; }

    private void Awake()
    {
        _flag.transform.localPosition = _flagLow;
    }

    private void Start()
    {
        AutoClaimAsNewOwner(_startingOwner);
        //SetBanners();
        //SetFlag();
    }

    //[ContextMenu("Claim for evil")]
    //public void ClaimForEvil()
    //{
    //    BeginClaimingAsNewOwner(-1);
    //}

    //[ContextMenu("Claim for good")]
    //public void ClaimForGood()
    //{
    //    BeginClaimingAsNewOwner(1);
    //}


    public void AutoClaimAsNewOwner(int newOwner)
    {
        if (newOwner == 0) return;
        else _isAutoRaisingOrLowering = true;

        if (Owner == 0)
        {
            Owner = newOwner;
            SetFlag(newOwner);
            _flagMoveDir = 1;
        }
        else
        {
            Owner = newOwner;
            _flagMoveDir = -1;
        }
        SetBanners();

    }

    private void SetBanners()
    {
        foreach (var banner in _banners)
        {
            if (Owner == -1)
            {
                banner.color = Color.white;
                banner.sprite = FlagLibrary.Instance.EvilBanner;
            }
            else if (Owner == 1)
            {
                banner.color = Color.white;
                banner.sprite = FlagLibrary.Instance.GoodBanner;
            }
            else
            {
                banner.sprite = null;
                //banner.color = Color.clear;
            }
        }
    }

    private void Update()
    {
        if (_isAutoRaisingOrLowering)
        {
            AutoRaiseLowerFlag();
        }
    }


    private void AutoRaiseLowerFlag()
    {
        if (_flagMoveDir == -1)
        {
            _flag.transform.localPosition = Vector3.MoveTowards(_flag.transform.localPosition,
                _flagLow, _raiseRate * Time.deltaTime);

            if ((_flag.transform.localPosition - _flagLow).magnitude < Mathf.Epsilon)
            {
                _flagMoveDir = 1;
                SetFlag(Owner);
            }
        }
        else if (_flagMoveDir == 1)
        {
            _flag.transform.localPosition = Vector3.MoveTowards(_flag.transform.localPosition,
                _flagHigh, _raiseRate * Time.deltaTime);

            if ((_flag.transform.localPosition - _flagHigh).magnitude < Mathf.Epsilon)
            {
                _flagMoveDir = 0;
                _isAutoRaisingOrLowering = false;
            }
        }
    }

    public void BeginConqueringBuilding(int conquerorAllegiance)
    {
        if (_conquerorAllegiance == conquerorAllegiance) return;
        _isAutoRaisingOrLowering = false;
        _flagMoveDir = -1;
        _conquerorAllegiance = conquerorAllegiance;
    }

    public void ContinueConqueringBuilding(float conquerRate)
    {
        if (_flagMoveDir == -1)
        {
            _flag.transform.localPosition = Vector3.MoveTowards(_flag.transform.localPosition,
                    _flagLow, conquerRate * Time.deltaTime);
            if ((_flag.transform.localPosition - _flagLow).magnitude < Mathf.Epsilon)
            {
                _flagMoveDir = 1;
                Debug.Log("flag lowered");
                SetFlag(_conquerorAllegiance);
            }
        }
        else
        {
            _flag.transform.localPosition = Vector3.MoveTowards(_flag.transform.localPosition,
                    _flagHigh, conquerRate * Time.deltaTime);
            if ((_flag.transform.localPosition - _flagHigh).magnitude < Mathf.Epsilon)
            {
                CompleteConqueringBuilding();
            }
        }
    }

    private void CompleteConqueringBuilding()
    {
        _flagMoveDir = 0;
        Owner = _conquerorAllegiance;
        _conquerorAllegiance = 0;
        BuildingConquered?.Invoke();
    }

    public void CancelConqueringBuilding()
    {
        _flagMoveDir = 1;
        _isAutoRaisingOrLowering = true;
        _conquerorAllegiance = 0;
    }

    private void SetFlag(int allegianceToDepict)
    {
        if (allegianceToDepict == -1)
        {
            _flag.color = Color.white;
            _flag.sprite = FlagLibrary.Instance.EvilFlag;
        }
        else if (allegianceToDepict == 1)
        {
            _flag.color = Color.white;
            _flag.sprite = FlagLibrary.Instance.GoodFlag;
        }
        else
        {
            _flag.sprite = null;
            //_flag.color = Color.clear;
        }
    }
}

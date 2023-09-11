using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingOwnershipHandler : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] _banners = null;
    [SerializeField] SpriteRenderer _flag = null;
    [SerializeField] Vector3 _flagLow = new Vector3(0, 0,0);
    [SerializeField] Vector3 _flagHigh = new Vector3(0, 2,0);
    /// <summary>
    /// How fast the flag moves per second, in world units
    /// </summary>
    [SerializeField] float _raiseRate = 1f;


    [SerializeField] int _startingOwner = 0;

    //state 
    public int _flagMoveDir = 0;

    /// <summary>
    /// Evil is -1, Good is 1, Neutral/Unclaimed is 0
    /// </summary>
    public int Owner { get; private set; }

    private void Awake()
    {
        _flag.transform.localPosition = _flagLow;
        BeginClaimingAsNewOwner(_startingOwner);
        SetBanners();
        SetFlag();
    }

    [ContextMenu("Claim for evil")]
    public void ClaimForEvil()
    {
        BeginClaimingAsNewOwner(-1);
    }

    [ContextMenu("Claim for good")]
    public void ClaimForGood()
    {
        BeginClaimingAsNewOwner(1);
    }


    public void BeginClaimingAsNewOwner(int newOwner)
    {
        if (newOwner == 0) return;

        if (Owner == 0)
        {
            Debug.Log("raising flag post-neutral");
            Owner = newOwner;
            SetFlag();
            _flagMoveDir = 1;
        }
        else
        {
            Owner = newOwner;
            Debug.Log("lowering flag");
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
        if (_flagMoveDir == -1)
        {
            _flag.transform.localPosition = Vector3.MoveTowards(_flag.transform.localPosition,
                _flagLow, _raiseRate * Time.deltaTime);

            if ((_flag.transform.localPosition - _flagLow).magnitude < Mathf.Epsilon)
            {
                Debug.Log("raising flag post-conquer");
                _flagMoveDir = 1;
                SetFlag();
            }
        }
        else if (_flagMoveDir == 1)
        {
            _flag.transform.localPosition = Vector3.MoveTowards(_flag.transform.localPosition,
                _flagHigh, _raiseRate * Time.deltaTime);

            if ((_flag.transform.localPosition - _flagHigh).magnitude < Mathf.Epsilon)
            {
                _flagMoveDir = 0;
            }
        }


    }

    private void SetFlag()
    {
        if (Owner == -1)
        {
            _flag.color = Color.white;
            _flag.sprite = FlagLibrary.Instance.EvilFlag;
        }
        else if (Owner == 1)
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

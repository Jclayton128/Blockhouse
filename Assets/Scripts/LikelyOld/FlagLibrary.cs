using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagLibrary : MonoBehaviour
{
    public static FlagLibrary Instance { get; private set; }
    [SerializeField] Sprite _evilBanner = null;
    public Sprite EvilBanner => _evilBanner;
    [SerializeField] Sprite _evilFlag = null;
    public Sprite EvilFlag => _evilFlag;
    [SerializeField] Sprite _goodBanner = null;
    public Sprite GoodBanner => _goodBanner;
    [SerializeField] Sprite _goodFlag = null;
    public Sprite GoodFlag => _goodFlag;


    private void Awake()
    {
        Instance = this;
    }
}

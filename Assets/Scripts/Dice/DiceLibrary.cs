using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceLibrary : MonoBehaviour
{
    public static DiceLibrary Instance { get; private set; }

    //{ Beast, Arcane, Hearth, Order, Nomad, Discord};
    [Header("Face Type")]
    [SerializeField] Color _beast = Color.red;
    [SerializeField] Color _arcane = Color.cyan;
    [SerializeField] Color _hearth = Color.yellow;
    [SerializeField] Color _order = Color.blue;
    [SerializeField] Color _nomad = Color.green;
    [SerializeField] Color _discord = Color.grey;
    public Color ColorBeast => _beast;
    public Color ColorArcane => _arcane;
    public Color ColorHearth => _hearth;
    public Color ColorOrder => _order;
    public Color ColorNomad => _nomad;
    public Color ColorDiscord => _discord;

    [Header("Level Bands")]
    [SerializeField] Color _level0 = Color.clear;
    [SerializeField] Color _level1 = Color.white;
    [SerializeField] Color _level2 = Color.white;
    [SerializeField] Color _level3 = Color.white;
    public Color Level0 => _level0;
    public Color Level1 => _level1;
    public Color Level2 => _level2;
    public Color Level3 => _level3;


    [Header("Dice Sprites")]
    [SerializeField] Sprite _edge_Light = null;
    [SerializeField] Sprite _edge_Medium = null;
    [SerializeField] Sprite _edge_Heavy = null;
    public Sprite EdgeLight => _edge_Light;
    public Sprite EdgeMedium => _edge_Medium;
    public Sprite EdgeHeavy => _edge_Heavy;

    [SerializeField] Sprite _band_Light = null;
    [SerializeField] Sprite _band_Medium = null;
    [SerializeField] Sprite _band_Heavy = null;
    public Sprite BandLight => _band_Light;
    public Sprite BandMedium => _band_Medium;
    public Sprite BandHeavy => _band_Heavy;

    [SerializeField] Sprite _fill_Light = null;
    [SerializeField] Sprite _fill_Medium = null;
    [SerializeField] Sprite _fill_Heavy = null;
    public Sprite FillLight => _fill_Light;
    public Sprite FillMedium => _fill_Medium;
    public Sprite FillHeavy => _fill_Heavy;

    [Header("Void Sprites")]
    [SerializeField] Sprite _void_Light = null;
    [SerializeField] Sprite _void_Light_Sans = null;
    [SerializeField] Sprite _void_Medium = null;
    [SerializeField] Sprite _void_Medium_Sans = null;
    [SerializeField] Sprite _void_Heavy = null;
    [SerializeField] Sprite _void_Heavy_Sans = null;

    public Sprite VoidLight => _void_Light;
    public Sprite VoidMedium => _void_Medium;
    public Sprite VoidHeavy => _void_Heavy;
    public Sprite VoidLightSans => _void_Light_Sans;
    public Sprite VoidMediumSans => _void_Medium_Sans;
    public Sprite VoidHeavySans => _void_Heavy_Sans;


    [Header("Face Tiles")]
    [SerializeField] FaceHandler _faceTilePrefab = null;
    public FaceHandler FaceTilePrefab => _faceTilePrefab;
    int _tileIndex = 1;
    private void Awake()
    {
        Instance = this;
    }

    public FaceHandler CreateFaceTile(DiceFace diceFace, Transform parent)
    {
        FaceHandler fh;

        fh = Instantiate(_faceTilePrefab, parent);
        fh.transform.localPosition = new Vector3(0, 0, -.1f);
        fh.SetFace(diceFace);
        fh.SetBaseSortOrder(_tileIndex);
        _tileIndex++;
        return fh;
    }
}
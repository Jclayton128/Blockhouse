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


    private void Awake()
    {
        Instance = this;
    }
}

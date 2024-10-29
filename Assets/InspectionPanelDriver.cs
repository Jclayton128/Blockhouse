using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class InspectionPanelDriver : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _nameTMP = null;
    [SerializeField] string _prefix_Fast = "Fast ";
    [SerializeField] string _prefix_Mid = "";
    [SerializeField] string _prefix_Heavy = "Heavy ";

    [SerializeField] Image _fillSR = null;
    [SerializeField] Image _edgeSR = null;
    [SerializeField] Image _bandSR = null;
    [SerializeField] Image _iconSR = null;


    public void DisplayFaceInformation(DiceFace diceFace)
    {
        DisplayFaceName(diceFace);
        DisplayLargeIcon(diceFace);
    }

    private void DisplayLargeIcon(DiceFace diceFace)
    {
        if (diceFace == null)
        {
            _iconSR.sprite = null;
            _bandSR.color = Color.white;
            _edgeSR.color = Color.black;
            _fillSR.color = Color.gray;

                         
            _fillSR.sprite = DiceLibrary.Instance.FillMedium;
            _bandSR.sprite = DiceLibrary.Instance.BandMedium;
            _edgeSR.sprite = DiceLibrary.Instance.EdgeMedium;
            return;
        }

        _iconSR.sprite = diceFace.FaceSprite;

        switch (diceFace.FaceAlignment)
        {
            case DiceFace.FaceAlignments.Beast:
                _fillSR.color = DiceLibrary.Instance.ColorBeast;
                break;

            case DiceFace.FaceAlignments.Discord:
                _fillSR.color = DiceLibrary.Instance.ColorDiscord;
                break;

            case DiceFace.FaceAlignments.Hearth:
                _fillSR.color = DiceLibrary.Instance.ColorHearth;
                break;

            case DiceFace.FaceAlignments.Order:
                _fillSR.color = DiceLibrary.Instance.ColorOrder;
                break;

            case DiceFace.FaceAlignments.Arcane:
                _fillSR.color = DiceLibrary.Instance.ColorArcane;
                break;

            case DiceFace.FaceAlignments.Nomad:
                _fillSR.color = DiceLibrary.Instance.ColorNomad;
                break;

        }

        switch (diceFace.DiceSpeed)
        {
            case Dice.DiceSpeeds.Light:
                _edgeSR.sprite = DiceLibrary.Instance.EdgeLight;
                _bandSR.sprite = DiceLibrary.Instance.BandLight;
                _fillSR.sprite = DiceLibrary.Instance.FillLight;
                break;

            case Dice.DiceSpeeds.Medium:
                _edgeSR.sprite = DiceLibrary.Instance.EdgeMedium;
                _bandSR.sprite = DiceLibrary.Instance.BandMedium;
                _fillSR.sprite = DiceLibrary.Instance.FillMedium;
                break;

            case Dice.DiceSpeeds.Heavy:
                _edgeSR.sprite = DiceLibrary.Instance.EdgeHeavy;
                _bandSR.sprite = DiceLibrary.Instance.BandHeavy;
                _fillSR.sprite = DiceLibrary.Instance.FillHeavy;
                break;



        }

        switch (diceFace.DiceLevel)
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


    private void DisplayFaceName(DiceFace diceFace)
    {
        string prefix;
        if (diceFace == null)
        {
            _nameTMP.text = "";
            return;
        }
        if (diceFace.DiceSpeed == Dice.DiceSpeeds.Light) prefix = _prefix_Fast;
        else if (diceFace.DiceSpeed == Dice.DiceSpeeds.Heavy) prefix = _prefix_Heavy;
        else prefix = _prefix_Mid;

        _nameTMP.text = prefix + diceFace.FaceName;
    }

    public void ClearFaceInformation()
    {
        DisplayFaceName(null);
        DisplayLargeIcon(null);
    }

}

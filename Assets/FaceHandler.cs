using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceHandler : MonoBehaviour
{
    [SerializeField] SpriteRenderer _edgeSR = null;
    [SerializeField] SpriteRenderer _bandSR = null;
    [SerializeField] SpriteRenderer _fillSR = null;
    [SerializeField] SpriteRenderer _iconSR = null;

    public void SetFace(DiceFace diceFace)
    {
        _iconSR.sprite = diceFace.FaceSprite;

        switch (diceFace.FaceType)
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

        switch (diceFace.DiceType)
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
}

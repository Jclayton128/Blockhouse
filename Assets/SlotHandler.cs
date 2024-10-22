using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotHandler : MonoBehaviour
{
    [SerializeField] DiceFace _diceFaceInSlot;
    public DiceFace DiceFaceInSlot => _diceFaceInSlot;

    [SerializeField] FaceHandler _faceHandlerInSlot;
    public FaceHandler FaceHandlerInSlot => _faceHandlerInSlot;

    [SerializeField] Dice.DiceTypes _slotType;

    public void SetAsSans(bool isSans)
    {
        switch (_slotType)
        {
            case Dice.DiceTypes.Light:
                if (isSans)
                {
                    GetComponent<SpriteRenderer>().sprite = DiceLibrary.Instance.VoidLightSans;
                }
                else
                {
                    GetComponent<SpriteRenderer>().sprite = DiceLibrary.Instance.VoidLight;
                }

                break;

            case Dice.DiceTypes.Medium:
                if (isSans)
                {
                    GetComponent<SpriteRenderer>().sprite = DiceLibrary.Instance.VoidMediumSans;
                }
                else
                {
                    GetComponent<SpriteRenderer>().sprite = DiceLibrary.Instance.VoidMedium;
                }

                break;

            case Dice.DiceTypes.Heavy:
                if (isSans)
                {
                    GetComponent<SpriteRenderer>().sprite = DiceLibrary.Instance.VoidHeavySans;
                }
                else
                {
                    GetComponent<SpriteRenderer>().sprite = DiceLibrary.Instance.VoidHeavy;
                }
                break;

        }

    }

    public void SetDiceType(Dice.DiceTypes diceType)
    {
        //_slotType = diceType;
    }

    public void RegisterNewFaceInSlot(DiceFace newDiceFace, FaceHandler newFaceHandler)
    {
        _diceFaceInSlot = newDiceFace;
        _faceHandlerInSlot = newFaceHandler;
    }

    public void ClearFaceHandlerFromSlot()
    {
        _faceHandlerInSlot = null;
    }

    internal bool CheckDiceTypeAgainstSlotType(DiceFace diceFaceRepresented)
    {
        if (diceFaceRepresented.DiceType == _slotType) return true;
        else return false;
    }
}

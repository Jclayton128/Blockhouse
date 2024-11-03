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

    [SerializeField] Dice.DiceSpeeds _slotSpeed = Dice.DiceSpeeds.Undefined;
    public Dice.DiceSpeeds SlotSpeed => _slotSpeed;

    public void SetAsSans(bool isSans)
    {
        switch (_slotSpeed)
        {
            case Dice.DiceSpeeds.Light:
                if (isSans)
                {
                    GetComponent<SpriteRenderer>().sprite = DiceLibrary.Instance.VoidLightSans;
                }
                else
                {
                    GetComponent<SpriteRenderer>().sprite = DiceLibrary.Instance.VoidLight;
                }

                break;

            case Dice.DiceSpeeds.Medium:
                if (isSans)
                {
                    GetComponent<SpriteRenderer>().sprite = DiceLibrary.Instance.VoidMediumSans;
                }
                else
                {
                    GetComponent<SpriteRenderer>().sprite = DiceLibrary.Instance.VoidMedium;
                }

                break;

            case Dice.DiceSpeeds.Heavy:
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

    public void RegisterNewFaceInSlot(DiceFace newDiceFace, FaceHandler newFaceHandler)
    {
        _slotSpeed = newDiceFace.DiceSpeed;
        _diceFaceInSlot = newDiceFace;
        _faceHandlerInSlot = newFaceHandler;
    }

    public void ClearFaceHandlerFromSlot()
    {
        _faceHandlerInSlot = null;
    }

    internal bool CheckDiceTypeAgainstSlotType(DiceFace diceFaceRepresented)
    {
        if (_slotSpeed == Dice.DiceSpeeds.Undefined ||
            diceFaceRepresented.DiceSpeed == _slotSpeed) return true;
        else return false;
    }
}

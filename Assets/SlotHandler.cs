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

    Dice.DiceTypes _diceType;
    public void SetDiceType(Dice.DiceTypes diceType)
    {
        _diceType = diceType;
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
        if (diceFaceRepresented.DiceType == _diceType) return true;
        else return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotHandler : MonoBehaviour
{
    [SerializeField] DiceFace _diceFaceInSlot;
    public DiceFace DiceFaceInSlot => _diceFaceInSlot;

    [SerializeField] FaceHandler _faceHandlerInSlot;
    public FaceHandler FaceHandlerInSlot => _faceHandlerInSlot;


    public void RegisterNewFaceInSlot(DiceFace newDiceFace, FaceHandler newFaceHandler)
    {
        _diceFaceInSlot = newDiceFace;
        _faceHandlerInSlot = newFaceHandler;
    }
}

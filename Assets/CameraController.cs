using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    CinemachineVirtualCamera _cvc;

    [SerializeField] Transform _cameraMouse = null;
    [SerializeField] float _mouseMoveTime = 2f;

    //state
    Vector3 _offset = Vector3.zero;
    Tween _offsetTween;

    private void Awake()
    {
        Instance = this;
        
    }

    private void Start()
    {
        _cvc = Camera.main.GetComponentInChildren<CinemachineVirtualCamera>();
        EncounterController.Instance.EncounterStarted += HandleEncounterStarted;
    }

    private void HandleEncounterStarted(Vector3 pos)
    {
        EngageCameraMouse(pos, _mouseMoveTime);
    }

    //private void Update()
    //{
    //    _cft.m_TrackedObjectOffset = _offset;
    //}

    public void EngageCameraMouse(Vector3 mousePos, float time)
    {
        _cvc.Follow = _cameraMouse;
        _cameraMouse.position = ActorController.Instance.PartyLead.transform.position;
        _cameraMouse.DOMove(mousePos, time);
    }

    public void SetCameraFocus(Transform targetTransform)
    {
        _cvc.Follow = targetTransform;
    }

    public void ClearCameraFocus()
    {
        _cvc.Follow = null;
    }
}

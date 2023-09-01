using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    CinemachineVirtualCamera _cvc;

    private void Awake()
    {
        Instance = this;
        _cvc = Camera.main.GetComponentInChildren<CinemachineVirtualCamera>();
    }

    private void Start()
    {

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

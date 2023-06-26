using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMover : MonoBehaviour
{
    [SerializeField] Transform _lookBug = null;

    //settings
    [SerializeField] float _moveSpeed = 3f;

    //state
    float _moveDir;
    [SerializeField] Vector3 _mousePos;
    [SerializeField] float _lookDir;
    [SerializeField] float _lookMagnitude;

    private void Update()
    {
        UpdateMovement();
        //UpdateLook();
    }


    private void UpdateCommandedMovement()
    {
        
    }


    private void UpdateMovement()
    {
        if (Mathf.Abs(_moveDir) > Mathf.Epsilon)
        {
            transform.position += Vector3.right * _moveDir * _moveSpeed * Time.deltaTime;
        }
    }
    
    private void UpdateLook()
    {
        //CVC follows the look bug.
        // look bug y local offset = 0
        // look bug x local x offset = (0 to 1 factor of screen space - 0.5) * 2 * sensitivy

        _mousePos = Input.mousePosition;
        _mousePos.z = Camera.main.nearClipPlane;
        _mousePos.y = transform.position.y;
        _lookDir = (Camera.main.ScreenToViewportPoint(_mousePos).x - 0.5f) * 2;
        _lookBug.localPosition = Vector3.right * _lookDir  * _lookMagnitude;
    }
}

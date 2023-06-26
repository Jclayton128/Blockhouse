using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    Camera _cam;

    public static InputController Instance { get; private set; }
    public Action<int> CommandedMoveDirectionChanged;
    public Action<int> CommandedLookDirectionChanged;
    public Action AttackCommanded;


    //state
    private int _commandedMoveDir = 0;
    private int _previousCommandedMoveDir = 0;
    private int _commandedLookDir = 0;
    private int _previousCommandedLookDir = 0;
    float _rawLookDir;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        UpdateCommandedMoveDir();
        UpdateCommandedLookDir();
        UpdateMouseClick();
    }

    private void UpdateMouseClick()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            AttackCommanded?.Invoke();
        }
    }

    private void UpdateCommandedLookDir()
    {
        _previousCommandedLookDir = _commandedLookDir;
        _rawLookDir = _cam.ScreenToViewportPoint(Input.mousePosition).x;
        if (_rawLookDir < 0.4f)
        {
            _commandedLookDir = -1;
        }
        else if (_rawLookDir > 0.6f)
        {
            _commandedLookDir = 1;
        }
        else
        {
            _commandedLookDir = 0;
        }

        if (_commandedLookDir != _previousCommandedLookDir)
        {
            CommandedLookDirectionChanged?.Invoke(_commandedLookDir);
        }
    }

    private void UpdateCommandedMoveDir()
    {
        _previousCommandedMoveDir = _commandedMoveDir;
        if (Input.GetKey(KeyCode.A))
        {
            _commandedMoveDir = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _commandedMoveDir = 1;
        }
        else
        {
            _commandedMoveDir = 0;
        }

        if (_commandedMoveDir != _previousCommandedMoveDir)
        {
            CommandedMoveDirectionChanged?.Invoke(_commandedMoveDir);
        }

    }
}

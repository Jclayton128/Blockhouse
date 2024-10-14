using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class DiceHandler : MonoBehaviour
{
    [SerializeField] FaceHandler _activeFaceHandler = null;
    [SerializeField] FaceHandler[] _reserveFaceHandlerss = null;

    //settings
    [SerializeField] float _rollTime_Min = 2f;
    [SerializeField] float _rollTime_Max = 4f;
    [SerializeField] float _timeFactorCoefficient_Min = .25f;
    [SerializeField] float _timeFactorCoefficient_Max = .5f;
    [SerializeField] Vector3 _jumpHeight = new Vector3(0, .5f, 0);


    //state
    Dice _dice;
    DiceFace[] _loadedFaces;

    [SerializeField] DiceFace _activeFace;
    public DiceFace ActiveFace => _activeFace;
    float _elapsedRollTime;
    int _rand;
    float _rollTime;
    float _timeFactor;
    float _timeOnCurrentFace;
    bool _isRolling = false;
    float _timeFactorCoefficient;
    Tween _finalJumpTween;


    public void LoadWithDice(Dice dice)
    {
        _elapsedRollTime = 0;
        _dice = dice;
        _loadedFaces = _dice.GetFaces();
        SetRandomDiceFace();
    }

    [ContextMenu("Roll Dice")]
    public void RollDice()
    {
        _elapsedRollTime = 0;
        _timeOnCurrentFace = 0;
        _timeFactor = 1;
        _isRolling = true;
        _rollTime = UnityEngine.Random.Range(_rollTime_Min, _rollTime_Max);
        _timeFactorCoefficient = UnityEngine.Random.Range(_timeFactorCoefficient_Min, _timeFactorCoefficient_Max);

        int sign = (2 * UnityEngine.Random.Range(0, 2)) - 1;
        _finalJumpTween.Kill();
        _finalJumpTween = transform.DOPunchPosition(sign * _jumpHeight, _rollTime);
    }

    private void Update()
    {
        if (_isRolling)
        {
            _elapsedRollTime += Time.deltaTime;
            _timeOnCurrentFace += Time.deltaTime;
            _timeFactor = _elapsedRollTime / _rollTime;


            if (_timeOnCurrentFace > _timeFactor * _timeFactorCoefficient)
            {
                _timeOnCurrentFace = 0;
                _timeFactorCoefficient = UnityEngine.Random.Range(_timeFactorCoefficient_Min, _timeFactorCoefficient_Max);
                SetRandomDiceFace();
            }
        }
        if (_isRolling && _elapsedRollTime >= _rollTime)
        {
            //Show the final dice face
            SetRandomDiceFace();
            _isRolling = false;
        }
    }

    private void SetRandomDiceFace()
    {
        _rand = UnityEngine.Random.Range(0, _loadedFaces.Length);
        _activeFace = _loadedFaces[_rand];
        RenderDice();
    }

    private void RenderDice()
    {
        _activeFaceHandler.SetFace(_activeFace);
    }
}

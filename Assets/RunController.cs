using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunController : MonoBehaviour
{
    /// <summary>
    /// Amount of party 'movement' since last frame
    /// </summary>
    public Action<float> RunDistanceIncreased;

    public static RunController Instance { get; private set; }
    public Action TargetDistanceReached;

    //settings
    [SerializeField] float _moveSpeed = 3;

    //state
    [SerializeField] float _distanceTraveled = 0;
    public float DistanceTraveled => _distanceTraveled;
    [SerializeField] float _targetDistance;


    private void Awake()
    {
        Instance = this;

    }

    private void Update()
    {
        if (GameController.Instance.GameMode == GameController.GameModes.WalkingToNextEncounter)
        {
            _distanceTraveled += _moveSpeed * Time.deltaTime;
            RunDistanceIncreased?.Invoke(_moveSpeed * Time.deltaTime);

            foreach (var thing in ActorController.Instance.EncounterThing)
            {
                thing.transform.position -= Vector3.right * _moveSpeed * Time.deltaTime;
            }

            if (_distanceTraveled >= _targetDistance)
            {
                Debug.Log("target distance reached");
                TargetDistanceReached?.Invoke();
                _targetDistance = Mathf.Infinity;
            }
        }


    }

    public void SetTargetCountdown(float targetDistance)
    {
        _targetDistance = _distanceTraveled + targetDistance;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovementHandler : MonoBehaviour
{
    //public Action ReachedDestination;

    ////settings
    //[SerializeField] float _moveRate = 3f;

    ////state
    //[SerializeField] float _xDest;
    //IFFHandler _iff;
    //ActorHandler _ah;
    //bool _isWalking = false;

    //private void Awake()
    //{
    //    _iff = GetComponentInChildren<IFFHandler>();
    //    _ah = GetComponentInChildren<ActorHandler>();
    //    _xDest = transform.position.x;
    //}

    //public void SetDestination(float xDest)
    //{
    //    _xDest = xDest;
    //    _isWalking = true;
    //}

    //private void Update()
    //{
    //    if (!_isWalking) return;
    //    //if (GameController.Instance.GameMode != GameController.GameModes.WalkingToNextEncounter) return;

    //    if (_iff.Allegiance == IFFHandler.Allegiances.Player)
    //    {
    //        //expect players to always be moving to the right
    //        if (transform.position.x < _xDest)
    //        {
    //            transform.position += Vector3.right * _moveRate * Time.deltaTime;
    //        }
    //        else
    //        {
    //            _isWalking = false;
    //            ReachedDestination?.Invoke();
    //            _ah.SetActorMode(ActorHandler.ActorModes.Idling);
    //        }
    //    }
    //    else if (_iff.Allegiance == IFFHandler.Allegiances.Enemy ||
    //        _iff.Allegiance == IFFHandler.Allegiances.Neutral)
    //    {
    //        //expect enemy/neutrals to always be moving to the left
    //        if (transform.position.x > _xDest)
    //        {
    //            transform.position -= Vector3.right * _moveRate * Time.deltaTime;
    //        }
    //        else
    //        {
    //            _isWalking = false;
    //            _ah.SetActorMode(ActorHandler.ActorModes.Idling);
    //        }
    //    }
       

    //}
}

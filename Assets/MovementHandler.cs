using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    //settings
    [SerializeField] float _moveRate = 3f;

    //state
    [SerializeField] float _xDest;
    IFFHandler _iff;
    ActorHandler _ah;

    private void Awake()
    {
        _iff = GetComponentInChildren<IFFHandler>();
        _ah = GetComponentInChildren<ActorHandler>();
        _xDest = transform.position.x;
    }

    public void SetDestination(float xDest)
    {
        _xDest = xDest;
    }

    private void Update()
    {
        if (_iff.Allegiance == IFFHandler.Allegiances.Player)
        {
            //expect players to always be moving to the right
            if (transform.position.x < _xDest)
            {
                transform.position += Vector3.right * _moveRate * Time.deltaTime;
            }
            else
            {
                _ah.SetActorMode(ActorHandler.ActorModes.Idling);
            }
        }
        else if (_iff.Allegiance == IFFHandler.Allegiances.Enemy ||
            _iff.Allegiance == IFFHandler.Allegiances.Neutral)
        {
            //expect enemy/neutrals to always be moving to the left
            if (transform.position.x > _xDest)
            {
                transform.position -= Vector3.right * _moveRate * Time.deltaTime;
            }
            else
            {
                _ah.SetActorMode(ActorHandler.ActorModes.Idling);
            }
        }
       

    }
}

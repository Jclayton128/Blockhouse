using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainProfile_Pacing : MonoBehaviour, BrainProfile
{
    ActorBrain _actorBrain;

    //setting
    [SerializeField] float _pacingDistance = 6;
    

    //state
    float _startingPoint;
    

    public void ExecuteStartup(ActorBrain actorBrain)
    {
        _actorBrain = actorBrain;
        _startingPoint = transform.position.x;
        int rand = (UnityEngine.Random.Range(0, 2)*2) - 1;
        _actorBrain.CommandedMoveDir = rand;
    }


    public void ExecuteUpdate(ActorBrain actorBrain)
    {
        if (transform.position.x < _startingPoint - _pacingDistance)
        {
            //turn, walk right
            _actorBrain.CommandedMoveDir = 1;
        }
        if (transform.position.x > _startingPoint + _pacingDistance)
        {
            //turn, walk left
            _actorBrain.CommandedMoveDir = -1;
        }
    }
}

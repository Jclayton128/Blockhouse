using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBrainProfile : MonoBehaviour, BrainProfile
{
    ActorBrain _actorBrain;

    public void ExecuteStartup(ActorBrain actorBrain)
    {
        _actorBrain = actorBrain;
        Debug.Log("starting up this actor as a player");
        if (actorBrain.GetComponent<IFFHandler>().IsPlayer)
        {
            InputController.Instance.CommandedMoveDirectionChanged += HandleCommandedMoveDirectionChanged;
            InputController.Instance.CommandedFlyDirectionChanged += HandleCommandedFlyDirectionChanged;
            
            //InputController.Instance.LMB_Down += HandleAttackCommanded;
        }
    }

    private void HandleCommandedMoveDirectionChanged(int dir)
    {
        _actorBrain.CommandedMoveDir = dir;
    }

    private void HandleCommandedFlyDirectionChanged(int dir)
    {
        _actorBrain.CommandedFlyDir = dir;
    }

    private void HandleAttackCommanded()
    {
        
    }

    public void ExecuteUpdate(ActorBrain actorBrain)
    {
        Debug.Log("updating up this actor as a player");
    }
}

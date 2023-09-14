using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainProfile_Attacker : MonoBehaviour, BrainProfile
{
    ActorBrain _ab;
    AttackHandler _ah;


    //settings


    //state


    public void ExecuteStartup(ActorBrain actorBrain)
    {
        _ab = actorBrain;
        _ah = _ab.GetComponent<AttackHandler>();
        _ab.CommandedMoveDir = GetComponent<IFFHandler>().Allegiance;

    }

    public void ExecuteUpdate(ActorBrain actorBrain)
    {
        //UpdateMove();
        if (_ab.EnemyTarget)
        {
            UpdateAttack();
        }
    }

    private void UpdateAttack()
    {
        _ah.CommandAttack();
    }

    //private void UpdateMove()
    //{
    //    _ab.CommandedMoveDir = _moveDirection;
    //}
}

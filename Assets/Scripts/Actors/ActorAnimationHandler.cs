using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorAnimationHandler : MonoBehaviour
{
    //refs
    ActorHandler _ah;
    Animator _anim;

    //settings

    //state
    Vector2 _pos;

    private void Awake()
    {
        //_rb = GetComponent<Rigidbody2D>();  
        _anim = GetComponentInChildren<Animator>();
        _ah = GetComponentInParent<ActorHandler>();
        _ah.ActorModeChanged += HandleActorModeChanged;
        _ah.ActorHighlighted += HandleActorHighlighted;
        _ah.ActorDehighlighted += HandleActorDehighlighted;
        _ah.ActorSelected += HandleActorSelected;
        _pos = transform.position + LayerLibrary.GetNextVisualLayer();
    }


    private void HandleActorModeChanged(ActorHandler.ActorModes newMode)
    {
        switch (newMode)
        {
            case ActorHandler.ActorModes.Idling:
                TriggerIdle();
                break;

            case ActorHandler.ActorModes.Walking:
                TriggerWalking();
                break;
        }
    }

    private void HandleActorHighlighted()
    {
        TriggerCheer();
    }

    private void HandleActorDehighlighted()
    {
        TriggerIdle();
    }


    private void HandleActorSelected()
    {
        TriggerCheerLoop();
    }

    public void ExecuteEffectAnimation(DiceFace.Animations animation)
    {
        switch (animation)
        {
            case DiceFace.Animations.Attack:
                TriggerAttack();
                break;

            case DiceFace.Animations.Cast:
                TriggerCasting();
                break;

            case DiceFace.Animations.Cheer:
                TriggerCheer();
                break;


        }
    }

    #region Basic Animations

    public void TriggerCheer()
    {
        _anim.SetTrigger("TriggerCheer");
    }
    
    public void TriggerCheerLoop()
    {
        _anim.SetTrigger("TriggerCheerLoop");
    }

    public void TriggerIdle()
    {
        _anim.SetTrigger("TriggerIdle");
    }


    public void TriggerAttack()
    {
        _anim.SetTrigger("TriggerAttack");
    }

    /// <summary>
    /// This should be called via AnimationEvent once the attack anim is complete.
    /// </summary>
    public void HandleCompletedAttack()
    {

    }


    public void TriggerWalking()
    {
        _anim.SetTrigger("TriggerWalk");
    }

    public void TriggerCasting()
    {
        _anim.SetTrigger("TriggerCast");
    }

    public void TriggerFlinch()
    {
        _anim.SetTrigger("TriggerFlinch");
    }

    public void TriggerDeath()
    {
        _anim.SetTrigger("TriggerDeath");
    }

    #endregion
}
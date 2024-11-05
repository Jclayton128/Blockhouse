using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ActorVisualsHandler : MonoBehaviour
{
    //refs
    ActorHandler _ah;
    Animator _anim;
    SpriteRenderer _sr;

    //settings
    [SerializeField] float _partialFadeAmount = 0.5f;
    [SerializeField] float _fadeTime = 0.3f;

    //state
    Vector2 _pos;
    Tween _fadeTween;

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

        _sr = GetComponent<SpriteRenderer>();
    }


    #region Publics
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

    public void SetPartialFade(bool shouldBePartiallyFaded)
    {
        if (shouldBePartiallyFaded)
        {
            _fadeTween.Kill();
            _fadeTween = _sr.DOFade(_partialFadeAmount, _fadeTime);
        }
        else
        {
            _fadeTween.Kill();
            _fadeTween = _sr.DOFade(1, _fadeTime);
        }
    }

    #endregion

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

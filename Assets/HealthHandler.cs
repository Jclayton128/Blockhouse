using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HealthHandler : MonoBehaviour
{
    ActorBrain _ab;
    SpriteRenderer _sr;
    Animator _anim;
    [SerializeField] int _health_Starting = 1;
    float _deathFadeoutTime = 3f;

    //state
    [SerializeField] int _health_Current;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        _ab = GetComponent<ActorBrain>();
    }

    private void Start()
    {
        _health_Current = _health_Starting;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ProjectileHandler ph;
        if (collision.TryGetComponent<ProjectileHandler>(out ph))
        {
            ModifyHealth(ph.Damage);
            ph.Deactivate();
        }
    }

    public void ModifyHealth(int healthToDeduct)
    {
        _health_Current -= healthToDeduct;
        _health_Current = Mathf.Clamp(_health_Current, 0, _health_Starting);
        if (CheckForDeath())
        {
            InitiateDeath();
        }
        else
        {
            InitiateFlinch();
        }
    }

    private void InitiateDeath()
    {
        _anim.SetTrigger("TriggerDeath");
        _ab.SetDeathStatus(true);
        gameObject.layer = 0;
        _ab.enabled = false;
        _sr.DOFade(0, _deathFadeoutTime);
        Invoke(nameof(CompleteDelayedDeath), _deathFadeoutTime);
    }

    private void CompleteDelayedDeath()
    {
        Destroy(gameObject);
    }

    private void InitiateFlinch()
    {
        _anim.SetTrigger("TriggerFlinch");
        _ab.IsFlinching = true;
        _ab.IsAttacking = false;
    }

    public void CompleteFlinch()
    {
        _ab.IsFlinching = false;
    }

    private bool CheckForDeath()
    {
        if (_health_Current <= 0)
        {
            return true;
        }
        else return false;
    }
}

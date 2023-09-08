using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHandler : MonoBehaviour
{
    ActorBrain _ab;
    Animator _anim;
    [SerializeField] int _health_Starting = 1;

    //state
    [SerializeField] int _health_Current;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
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
        Debug.Log("death!");
        _anim.SetTrigger("TriggerDeath");
        _ab.SetDeathStatus(true);
        gameObject.layer = 0;
    }

    private void InitiateFlinch()
    {
        Debug.Log("flinch!");
        _anim.SetTrigger("TriggerFlinch");
        _ab.IsFlinching = true;
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

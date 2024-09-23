using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    Animator _anim;

    //settings

    //state
    //private int _commandedMoveDir;
    //private int _commandedLookDir;
    float _moveSpeed_Current;
    float _flySpeed_Current;
    //private bool _isMovementPaused = false;
    Vector2 _pos;

    private void Awake()
    {
        //_rb = GetComponent<Rigidbody2D>();  
        _anim = GetComponent<Animator>();
        _pos = transform.position + LayerLibrary.GetNextVisualLayer();
    }

    private void Start()
    {
        StartWalking();
    }    

    /// <summary>
    /// This should be called by a BrainProfile when an attack is needed.
    /// </summary>
    public void DisplayAttack()
    {
        _anim.SetTrigger("TriggerAttack");
        //_rb.velocity = Vector2.zero;
        //_moveSpeed_Current = 0;

    }

    /// <summary>
    /// This should be called via AnimationEvent once the attack anim is complete.
    /// </summary>
    public void HandleCompletedAttack()
    {

    }


    public void StartWalking()
    {
        _anim.SetInteger("WalkDir", 1);
    }

}

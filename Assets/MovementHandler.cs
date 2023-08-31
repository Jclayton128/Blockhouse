using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    //Rigidbody2D _rb;
    Animator _anim;

    //settings
    [SerializeField] float _moveSpeed_Max = 3f;


    //state
    private int _commandedMoveDir;
    private int _commandedLookDir;
    float _moveSpeed_Current;
    private bool _isMovementPaused = false;
    Vector2 _pos;

    private void Awake()
    {
        //_rb = GetComponent<Rigidbody2D>();  
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        if (GetComponent<IFFHandler>().IsPlayer)
        {
            InputController.Instance.CommandedMoveDirectionChanged += HandleCommandedMoveDirectionChanged;
            InputController.Instance.CommandedLookDirectionChanged += HandleCommandedLookDirectionChanged;
            InputController.Instance.AttackCommanded += HandleAttackCommanded;
        }
        _pos = transform.position;
    }

    private void HandleCommandedMoveDirectionChanged(int dir)
    {
        _commandedMoveDir = dir;
        _anim.SetInteger("WalkDir", dir);
        if (dir != 0)
        {
            transform.localScale = new Vector2(dir, 1);

        }
    }

    private void HandleCommandedLookDirectionChanged(int dir)
    {
        //if (dir != 0)
        //{
        //    _commandedLookDir = dir;
        //    transform.localScale = new Vector2(dir, 1);
        //}
    }

    public void HandleAttackCommanded()
    {
        _isMovementPaused = true;
        _anim.SetTrigger("TriggerAttack");
        //_rb.velocity = Vector2.zero;
        _moveSpeed_Current = 0;

    }

    public void HandleCompletedAttack()
    {
        _isMovementPaused = false;
    }


    private void Update()
    {
        if (!_isMovementPaused)
        {
            //_rb.velocity = _moveSpeed * _commandedMoveDir * Vector2.right;
            _moveSpeed_Current = _moveSpeed_Max * _commandedMoveDir;
            _pos.x += _moveSpeed_Current * Time.deltaTime;
            transform.position = _pos;
        }

    }
}

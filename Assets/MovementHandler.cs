using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    //Rigidbody2D _rb;
    Animator _anim;
    ActorBrain _brain;

    //settings
    [SerializeField] float _moveSpeed_Max = 3f;
    [SerializeField] float _flySpeed_Max = 0f;


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
        _brain = GetComponent<ActorBrain>();
        _pos = transform.position;
    }

    private void Start()
    {
        //if (GetComponent<IFFHandler>().IsPlayer)
        //{
        //    InputController.Instance.CommandedMoveDirectionChanged += HandleCommandedMoveDirectionChanged;
        //    //InputController.Instance.CommandedLookDirectionChanged += HandleCommandedLookDirectionChanged;
        //    InputController.Instance.LMB_Down += HandleAttackCommanded;
        //}
        //_pos = transform.position;
    }

    private void HandleCommandedMoveDirectionChanged(int dir)
    {
        //_commandedMoveDir = dir;
        //_anim.SetInteger("WalkDir", dir);
        //if (dir != 0)
        //{
        //    transform.localScale = new Vector2(dir, 1);

        //}
    }

    //private void HandleCommandedLookDirectionChanged(int dir)
    //{
    //    //if (dir != 0)
    //    //{
    //    //    _commandedLookDir = dir;
    //    //    transform.localScale = new Vector2(dir, 1);
    //    //}
    //}

    //public void HandleAttackCommanded()
    //{
    //    _brain.IsMovementPaused = true;
    //    _anim.SetTrigger("TriggerAttack");
    //    //_rb.velocity = Vector2.zero;
    //    _moveSpeed_Current = 0;

    //}

    //public void HandleCompletedAttack()
    //{
    //    _isMovementPaused = false;
    //}


    private void Update()
    {
        if (!_brain.IsMovementPaused)
        {
            //_rb.velocity = _moveSpeed * _commandedMoveDir * Vector2.right;
            _moveSpeed_Current = _moveSpeed_Max * _brain.CommandedMoveDir;
            _flySpeed_Current = _flySpeed_Max * _brain.CommandedFlyDir;
            _pos.x += _moveSpeed_Current * Time.deltaTime;
            _pos.y += _flySpeed_Current * Time.deltaTime;
            _pos.y = Mathf.Clamp(_pos.y,
                ArenaController.Instance.MinY, ArenaController.Instance.MaxY);
            _pos.x = Mathf.Clamp(_pos.x,
                ArenaController.Instance.MinX, ArenaController.Instance.MaxX);
            transform.position = _pos;

            _anim.SetInteger("WalkDir", _brain.CommandedMoveDir);
            if (_brain.CommandedMoveDir != 0)
            {
                transform.localScale = new Vector2(_brain.CommandedMoveDir, 1);

            }
        }

    }
}

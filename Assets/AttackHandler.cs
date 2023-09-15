using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    ActorBrain _ab;
    MovementHandler _mh;
    IFFHandler _ih;

    [SerializeField] ProjectileHandler.ProjectileType _projectileType = ProjectileHandler.ProjectileType.Undefined;
    [SerializeField] int _damage = 1;
    [SerializeField] float _speed = 1;
    [SerializeField] float _range = 5;
    [SerializeField] float _yHeight = 1f;
    public float Range => _range;

    //state
    bool _canInitiateAnotherAttack = true;

    private void Awake()
    {
        _ab = GetComponent<ActorBrain>();
        _mh = _ab.GetComponent<MovementHandler>();
        _ih = _ab.GetComponent<IFFHandler>();
    }

    public void CommandAttack()
    {
        float dist = Mathf.Abs(_ab.EnemyTarget.transform.position.x - transform.position.x);
        if (dist > _range)
        {
            _ab.CommandedMoveDir = _ih.Allegiance;
        }
        else if (_canInitiateAnotherAttack && dist < _range)
        {
            _mh.DisplayAttack();
            _canInitiateAnotherAttack = false;
        }
    }

    /// <summary>
    /// This is called via AnimationEvent once the proper frame has been reached.
    /// </summary>
    public void HandleCompletedAttack()
    {
        ProjectileHandler ph = ProjectileController.Instance.RequisitionActivatedProjectile(_projectileType);
        ph.Initialize(_speed, _range, _ab.CommandedMoveDir,
            new Vector2(transform.position.x, transform.position.y + _yHeight), _damage);
        _canInitiateAnotherAttack = true;
    }


}

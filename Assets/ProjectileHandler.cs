using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    public enum ProjectileType {Undefined, Arrow}
    SpriteRenderer _sr;


    //state
    int _moveDir;
    public ProjectileType PType = ProjectileType.Undefined;
    float _timeToDeactivate;
    Vector2 _pos;
    float _speed;
    int _damage;
    public int Damage => _damage;
    bool _hasStartedDeactivating = false;

    public void Initialize(float projectileSpeed, float projectileRange, int moveDir, Vector2 pos,
        int damage)
    {
        _timeToDeactivate = Time.time + (projectileRange / projectileSpeed);
        _moveDir = moveDir;

        if (_moveDir > 0)
        {
            gameObject.layer = LayerLibrary.GoodProjectile_Layer;
        }
        else if (_moveDir < 0)
        {
            gameObject.layer = LayerLibrary.BadProjectile_Layer;
        }

        _pos = pos;
        transform.position = pos;
        _speed = projectileSpeed;
        _damage = damage;
        _hasStartedDeactivating = false;
    }

    private void Update()
    {
        if (_hasStartedDeactivating) return;
        if (Time.time >= _timeToDeactivate)
        {
            Deactivate();
        }
        else
        {
            _pos.x += (_speed * _moveDir * Time.deltaTime);
        }
        transform.position = _pos;
    }

    public void Activate()
    {
        if (!_sr) _sr = GetComponent<SpriteRenderer>();
        _sr.color = Color.white;
        _hasStartedDeactivating = false;
        //Begin any particle effects
        //enable collider, and otherwise undo the effects of Deactivate
    }

    public void Deactivate()
    {
        if (_hasStartedDeactivating) return;
        _hasStartedDeactivating = true;
        //Debug.Log("Deactivating");
        _sr.color = Color.clear;
        gameObject.layer = 0;
        //begin fadeout
        //disable colliders
        //stop emitting particles
        Invoke(nameof(Deactive_Delayed), 1f);
    }

    private void Deactive_Delayed()
    {
        //Kill any particle effects
        ProjectileController.Instance.ReturnDeactivatedProjectile(this);
    }

}

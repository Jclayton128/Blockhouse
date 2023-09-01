using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorBrain : MonoBehaviour
{
    public Action EnemyDetected;
    public Action EnemyNotDetected;
    public Action AllyDetected;


    //settings
    float _timeBetweenScans = 0.1f;
    [SerializeField] float _scanRange = 3f;

    //state
    public int CommandedMoveDir = 0;
    public int CommandedFlyDir = 0;
    public bool IsMovementPaused = false;

    public IFFHandler EnemyTarget = null;
    public IFFHandler AllyTarget = null;

    BrainProfile _brainProfile = null;
    float _timeForNextScan;
    Vector3 _scanStart = new Vector3(0, 0.5f, 0);
    Vector3 _scanTerminus = new Vector3(0,0.5f, 0);
    int _enemyLayerMask;
    int _allyLayerMask;

    void Start()
    {
        _brainProfile = GetComponent<BrainProfile>();
        if (_brainProfile == null)
        {
            Debug.LogWarning("This actor is missing a brain profile");
            return;
        }
        _brainProfile?.ExecuteStartup(this);
        _timeForNextScan = Time.time + _timeBetweenScans;

        if (GetComponent<IFFHandler>().IsGood)
        {
            _enemyLayerMask = LayerLibrary.BadActor_LayerMask;
            _allyLayerMask = LayerLibrary.GoodActor_LayerMask;
        }
        else
        {
            _allyLayerMask = LayerLibrary.BadActor_LayerMask;
            _enemyLayerMask = LayerLibrary.GoodActor_LayerMask;
        }
    }

    void Update()
    {
        if (Time.time >= _timeForNextScan)
        {
            _scanStart.x = transform.position.x;
            _scanTerminus.x = transform.position.x + (_scanRange * CommandedMoveDir);
            ScanForEnemyTarget();
            ScanForAllyTarget();
            _timeForNextScan = Time.time + _timeBetweenScans;
        }
        _brainProfile?.ExecuteUpdate(this);
    }

    private void ScanForAllyTarget()
    {
        var hit = Physics2D.Linecast(_scanStart, _scanTerminus, _allyLayerMask);
        if (hit)
        {
            AllyTarget = hit.transform.GetComponent<IFFHandler>();
            AllyDetected?.Invoke();
        }
        else
        {
            AllyTarget = null;
        }
    }

    private void ScanForEnemyTarget()
    {        
        var hit = Physics2D.Linecast(_scanStart, _scanTerminus, _enemyLayerMask);
        //Debug.DrawLine(_scanStart, _scanTerminus, Color.blue, _timeBetweenScans);
        if (hit)
        {
            EnemyTarget = hit.transform.GetComponent<IFFHandler>();
            EnemyDetected?.Invoke();
        }
        else
        {
            EnemyTarget = null;
            EnemyNotDetected?.Invoke();
        }          
    }

    //private RaycastHit2D GetClosestEnemyToCurrentXPos(RaycastHit2D[] hits)
    //{
    //    float testPosX;
    //    float range;
    //    float rangeToBeat = Mathf.Infinity;
    //    RaycastHit2D hitToBeat = new RaycastHit2D();
    //    foreach (var hit in hits)
    //    {
    //        testPosX = hit.transform.position.x;
    //        range = Math.Abs(transform.position.x - testPosX);
    //        if (range < rangeToBeat)
    //        {
    //            hitToBeat = hit;
    //            rangeToBeat = range;
    //        }
    //    }

    //    return hitToBeat;

    //}
}

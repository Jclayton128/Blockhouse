using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public static BackgroundController Instance { get; private set; }

    [SerializeField] List<GameObject> _chunks = null;
    float _sweepPoint = -18f;
    Vector3 _redeployAmount = new Vector3(48f,0,0);

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RunController.Instance.RunDistanceIncreased += HandleRunDistanceIncreased;
    }

    private void HandleRunDistanceIncreased(float obj)
    {
        foreach (var chunk in _chunks)
        {
            chunk.transform.position -= Vector3.right * obj;
            if (chunk.transform.position.x < _sweepPoint)
            {
                chunk.transform.position += _redeployAmount;
            }
        }

        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionLibrary : MonoBehaviour
{
    public static PositionLibrary Instance { get; private set; }

    [SerializeField] List<Vector3> _rewardTilePositions = new List<Vector3>();

    //state

    private void Awake()
    {
        Instance = this;
    }
}

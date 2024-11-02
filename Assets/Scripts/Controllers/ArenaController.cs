using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    public static ArenaController Instance { get; private set; }

    //settings
    [SerializeField] float _minX = -50f;
    [SerializeField] float _maxX = 50f;
    [SerializeField] float _minY = 0f;
    [SerializeField] float _maxY = 7f;

    public float MinX => _minX;
    public float MaxX => _maxX;
    public float MinY => _minY;
    public float MaxY => _maxY;


    private void Awake()
    {
        Instance = this;
    }
}

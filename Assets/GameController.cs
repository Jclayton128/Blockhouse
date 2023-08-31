using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    //settings
    [SerializeField] Vector2 _playerStartingPosition = new Vector2(0, 3);

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PlayerController.Instance.SpawnPlayer(_playerStartingPosition);
    }
}

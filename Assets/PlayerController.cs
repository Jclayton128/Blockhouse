using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    [SerializeField] GameObject _playerPrefab = null;


    //state
    public Vector3 Pos = new Vector3(0, 0, 0);
    public GameObject Player { get; private set; }

    private void Awake()
    {
        Instance = this;
    }


    public void SpawnPlayer(Vector3 position)
    {
        if (Player)
        {
            Debug.LogWarning("A player already exists!");
        }
        else
        {
            Player = Instantiate(_playerPrefab, position, Quaternion.identity);
        }
    }
}

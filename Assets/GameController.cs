using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    public enum GameModes { Title, HeroSelect, WalkingToNextEncounter, InEncounter}
    public Action<GameModes> GameModeChanged;

    public static GameController Instance { get; private set; }

    //settings
    [SerializeField] Vector2[] _characterStartingPosition = null;
    [SerializeField] ActorLibrary.ActorTypes[] _startingCharacters = null;

    //state
    [SerializeField] GameModes _gameMode;
    public GameModes GameMode => _gameMode;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < _startingCharacters.Length; i++)
        {
            ActorHandler ah = ActorController.Instance.SpawnActor(_startingCharacters[i], _characterStartingPosition[i], IFFHandler.Allegiances.Undefined);
            ah.Initialize();
        }
        SetGameMode(GameModes.HeroSelect);
    }

    public void SetGameMode(GameModes newGameMode)
    {
        _gameMode = newGameMode;
        GameModeChanged?.Invoke(_gameMode);
    }

}

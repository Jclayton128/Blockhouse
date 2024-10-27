using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    public enum GameModes { Title, HeroSelect, WalkingToNextEncounter, EncounterIntro, EncounterInspection, EncounterActionSelection, EncounterActionResolution, EncounterOutcome}
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
        }
        Invoke(nameof(Delay_Start), 0.01f);
    }

    private void Delay_Start()
    {
        SetGameMode(GameModes.HeroSelect);
    }

    /// <summary>
    /// Other Controllers should listen for GameMode Changes and execute the changes they want to see. The GameController doesn't care about what impact the changing GameMode may have.
    /// </summary>
    /// <param name="newGameMode"></param>
    public void SetGameMode(GameModes newGameMode)
    {
        _gameMode = newGameMode;
        GameModeChanged?.Invoke(_gameMode);
    }

}

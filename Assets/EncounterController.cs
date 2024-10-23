using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EncounterController : MonoBehaviour
{
    /// <summary>
    /// Vector3 Position is the midpoint of the encounter
    /// </summary>
    public Action<Vector3> EncounterStarted;
    public Action<Vector3> EncounterFinished;
    public static EncounterController Instance { get; private set; }


    //settings

    [SerializeField] float _timeBetweenChecks = 10f;
    [SerializeField] Vector3 _encounterOffset = new Vector3(13, 0, 0);
    //[SerializeField] float _additionalWalkDistance = 10f;
    [SerializeField] float[] _xOffsets_Encounter = { 2.5f, 6f, 9.5f, 13f };
    [SerializeField] float[] _xOffsets_Player = { -6f, -9.5f, -13f };

    //state 
    [SerializeField] float _timeOfNextCheck = 0;
    [SerializeField] Encounter _currentEncounter;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameController.Instance.GameModeChanged += HandleGameModeChanged;
    }

    private void HandleGameModeChanged(GameController.GameModes obj)
    {
        if (obj == GameController.GameModes.WalkingToNextEncounter)
        {
            _timeOfNextCheck = Time.time + _timeBetweenChecks;
        }
    }
    public void AdvanceEncounter()
    {
        if (GameController.Instance.GameMode == GameController.GameModes.EncounterIntro)
        {
            GameController.Instance.SetGameMode(GameController.GameModes.EncounterInspection);
        }
        else if (GameController.Instance.GameMode == GameController.GameModes.EncounterInspection)
        {

            GameController.Instance.SetGameMode(GameController.GameModes.EncounterActionSelection);
        }
    }

    #region Flow

    private void Update()
    {
        UpdateCheckForNextEncounter();
    }

   
    
    private void UpdateCheckForNextEncounter()
    {
        if (GameController.Instance.GameMode != GameController.GameModes.WalkingToNextEncounter) return;

        if (!_currentEncounter && Time.time >= _timeOfNextCheck)
        {
            _timeOfNextCheck = Time.time + _timeBetweenChecks;

            Encounter enc = EncounterLibrary.Instance.FindValidRandomEncounter(RunController.Instance.DistanceTraveled);
            if (enc)
            {
                _currentEncounter = enc;
                StartEncounter();

                //AUDIO somekind of bell announcing a new encounter
                RunController.Instance.SetTargetCountdown(_encounterOffset.x);
                RunController.Instance.TargetDistanceReached += HandleTargetDistanceReached;
            }
        }
    }

    private void HandleTargetDistanceReached()
    {
        GameController.Instance.SetGameMode(GameController.GameModes.EncounterIntro);
    }

    private void StartEncounter()
    {
        SpawnEncounterActors();
        //EncounterStarted?.Invoke(_encounterOffset);
        EncounterStarted?.Invoke(new Vector3(2.5f,0,0));
    }

    private void SpawnEncounterActors()
    {
        int count = 0;
        if (_currentEncounter.Actors.Length > 4)
        {
            Debug.LogWarning("More actors requested for this encounter than can fit");
        }
        foreach (var actor in _currentEncounter.Actors)
        {

            ActorHandler ah = ActorController.Instance.SpawnActor(actor,
                _encounterOffset + new Vector3(_xOffsets_Encounter[count], 0, 0),
                IFFHandler.Allegiances.Enemy);

            ah.Initialize(IFFHandler.Allegiances.Enemy);

            count++;
        }
    }

    #endregion
}

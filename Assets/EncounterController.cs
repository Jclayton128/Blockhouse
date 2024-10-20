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
    [SerializeField] Vector3 _encounterMidpointOffset = new Vector3(40f, 0, 0);
    //[SerializeField] float _additionalWalkDistance = 10f;
    [SerializeField] float[] _xOffsets_Encounter = { 2.5f, 6f, 9.5f, 13f };
    [SerializeField] float[] _xOffsets_Player = { -6f, -9.5f, -13f };

    //state 
    [SerializeField] float _timeOfNextCheck = 0;
    [SerializeField] Encounter _currentEncounter;
    Vector3 _encounterMidpoint = Vector3.zero;

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

    private void Update()
    {
        if (GameController.Instance.GameMode != GameController.GameModes.WalkingToNextEncounter) return;
        if (Time.time >= _timeOfNextCheck)
        {
            _timeOfNextCheck = Time.time + _timeBetweenChecks;

            Encounter enc = EncounterLibrary.Instance.FindValidRandomEncounter(ActorController.Instance.PartyLead.transform.position.x);
            if (enc)
            {
                _currentEncounter = enc;
                _encounterMidpoint = ActorController.Instance.PartyLead.transform.position + _encounterMidpointOffset;
                _encounterMidpoint.x = Mathf.RoundToInt(_encounterMidpoint.x);

                Debug.Log("encounter midpoint: " + _encounterMidpoint.x);

                StartEncounter();
                //AUDIO somekind of bell announcing a new encounter

            }
        }
        
    }

    private void StartEncounter()
    {        
        ActorController.Instance.
            StopParty(_encounterMidpoint.x + _xOffsets_Player[0],
            _encounterMidpoint.x + _xOffsets_Player[1],
            _encounterMidpoint.x + _xOffsets_Player[2]);

        Debug.Log($"stopping party at {_encounterMidpoint.x + _xOffsets_Player[0]}, " +
            $"{_encounterMidpoint.x + _xOffsets_Player[1]}, and" +
            $" {_encounterMidpoint.x + _xOffsets_Player[2]} ");


        SpawnEncounterActors();
        GameController.Instance.SetGameMode(GameController.GameModes.InEncounter);
        EncounterStarted?.Invoke(_encounterMidpoint);
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
                _encounterMidpoint + new Vector3(_xOffsets_Encounter[count], 0, 0),
                IFFHandler.Allegiances.Enemy);

            ah.Initialize(IFFHandler.Allegiances.Enemy);

            count++;
        }
    }
}

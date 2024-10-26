using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ActorController : MonoBehaviour
{
    public static ActorController Instance { get; private set; }   
    

    //state
    //Dictionary<ActorLibrary.ActorTypes, List<GameObject>> _currentActors = new Dictionary<ActorLibrary.ActorTypes, List<GameObject>>();
    //[SerializeField] ActorLibrary.ActorTypes _selectedActorType;

    [SerializeField] List<ActorHandler> _party = new List<ActorHandler>();
    public ActorHandler PartyLead => _party[0];
    [SerializeField] List<GameObject> _encounter = new List<GameObject>();
    public List<GameObject> EncounterThing => _encounter;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameController.Instance.GameModeChanged += HandleGameModeChanged;
    }

    private void HandleGameModeChanged(GameController.GameModes gameMode)
    {
        switch (gameMode)
        {
            case GameController.GameModes.WalkingToNextEncounter:
                WalkParty();
                CompactPartyDice();
                break;

            case GameController.GameModes.EncounterIntro:
                StopParty();
                break;

            case GameController.GameModes.EncounterInspection:
                //StopParty();
                ShowPartyDice();
                ShowEncounterDice();                
                break;

            case GameController.GameModes.EncounterActionSelection:
                Invoke(nameof(Delay_ModeChangedToEncounterActionSelectionMode), 1f);
                break;
        }
    }

    private void Delay_ModeChangedToEncounterActionSelectionMode()
    {
        RollPartyDice();
        RollEncounterDice();
    }


    public ActorHandler SpawnActor(ActorLibrary.ActorTypes actorType, Vector3 spawnPos,
        IFFHandler.Allegiances allegiance)
    {
        GameObject prefabGO = ActorLibrary.Instance.GetActorPrefabFromActorType(actorType);

        if (!prefabGO)
        {
            Debug.Log("No prefab for this!");
            return null;
        }

        GameObject newGO = Instantiate(prefabGO, spawnPos, Quaternion.identity);
        ActorHandler ah = newGO.GetComponent<ActorHandler>();
        ah.Initialize(allegiance);
        _encounter.Add(ah.gameObject);

        if (allegiance == IFFHandler.Allegiances.Player)
        {
            AddActorToParty(ah);
        }

        return ah;
    }

    public void SelectCharacter(ActorHandler character)
    {
        AddActorToParty(character);
        GameController.Instance.SetGameMode(GameController.GameModes.WalkingToNextEncounter);

        character.transform.DOMove(Vector3.zero, 3f);
        CameraController.Instance.EngageCameraMouse(Vector3.zero, 3f);
    }

    public void AddActorToParty(ActorHandler actor)
    {
        _party.Add(actor);
        _encounter.Remove(actor.gameObject);
    }

    

    #region Party Actions

    
    public void WalkParty()
    {
        foreach (var actor in _party)
        {
            actor.SetActorMode(ActorHandler.ActorModes.Walking);
            //actor.GetComponent<MovementHandler>().SetDestination(999f);      
        }
    }

    public void StopParty()
    {
        foreach (var actor in _party)
        {
            actor.SetActorMode(ActorHandler.ActorModes.Idling);
            //actor.GetComponent<MovementHandler>().SetDestination(999f);      
        }
    }

    public void ShowPartyDice()
    {
        //AUDIO Play a many dice being rolled sound. This is the point where all the dice are rolled together.
        foreach (var actor in _party)
        {
            actor.ShowDice();
            //actor.GetComponent<MovementHandler>().SetDestination(999f);      
        }
    }

    public void RollPartyDice()
    {
        foreach (var actor in _party)
        {
            actor.RollDice();
        }
    }

    public void CompactPartyDice()
    {
        foreach (var actor in _party)
        {
            actor.CompactDice();
        }
    }

    public void CompactHideAllPartyDiceExceptThis(ActorHandler actorToIgnore)
    {
        foreach (var actor in _party)
        {
            if (actor == actorToIgnore) continue;

            actor.HideDice(false);
            actor.CompactDice();
        }
    }

    #endregion

    #region Encounters

    public void ShowEncounterDice()
    {
        ActorHandler ah;
        foreach (var thing in _encounter)
        {
            if (thing.TryGetComponent<ActorHandler>(out ah))
            {
                ah.ShowDice();
            }
        }
    }

    public void HideEncounterDice()
    {
        ActorHandler ah;
        foreach (var thing in _encounter)
        {
            if (thing.TryGetComponent<ActorHandler>(out ah))
            {
                ah.HideDice(false);
            }
        }
    }

    public void CompactEncounterDice()
    {
        ActorHandler ah;
        foreach (var thing in _encounter)
        {
            if (thing.TryGetComponent<ActorHandler>(out ah))
            {
                ah.CompactDice();
            }
        }
    }


    public void RollEncounterDice()
    {
        ActorHandler ah;
        foreach (var thing in _encounter)
        {
            if (thing.TryGetComponent<ActorHandler>(out ah))
            {
                ah.RollDice();
            }
        }
    }

    public void CompactHideAllEncounterDiceExceptThis(ActorHandler actorToIgnore)
    {
        ActorHandler ah;
        foreach (var thing in _encounter)
        {
            if (thing.TryGetComponent<ActorHandler>(out ah) && ah == actorToIgnore)
            {
                continue;
            }
            else
            {
                ah.HideDice(false);
                ah.CompactDice();
            }
        }
    }

    #endregion


}

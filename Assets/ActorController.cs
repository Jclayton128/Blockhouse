using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public static ActorController Instance { get; private set; }

    //settings

    

    //state
    //Dictionary<ActorLibrary.ActorTypes, List<GameObject>> _currentActors = new Dictionary<ActorLibrary.ActorTypes, List<GameObject>>();
    //[SerializeField] ActorLibrary.ActorTypes _selectedActorType;

    [SerializeField] List<ActorHandler> _party = new List<ActorHandler>();
    public ActorHandler PartyLead => _party[0];
    [SerializeField] List<ActorHandler> _encounter = new List<ActorHandler>();

    private void Awake()
    {
        Instance = this;
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

        if (allegiance == IFFHandler.Allegiances.Player)
        {
            AddActorToParty(ah);
        }
        else if (allegiance == IFFHandler.Allegiances.Enemy || allegiance == IFFHandler.Allegiances.Neutral)
        {
            _encounter.Add(ah);
        }
        else
        {
            //if not player, enemy, or neutral, then I don't care about this actor and it should be swept up.
        }

        return ah;
    }

    public void AddActorToParty(ActorHandler actor)
    {
        _party.Add(actor);
        if (_party.Count >= 1)
        {
            //focus on the first dude
            CameraController.Instance.SetCameraFocus(_party[0].transform);
        }
    }

    public void RemoveActorFromParty(ActorHandler actor)
    {
        _party.Remove(actor);
        if (_party.Count >= 1)
        {
            //focus on the first dude
            CameraController.Instance.SetCameraFocus(_party[0].transform);
        }
    }

    #region Party Movement

    public void WalkParty()
    {
        foreach (var actor in _party)
        {
            actor.SetActorMode(ActorHandler.ActorModes.Walking);
            actor.GetComponent<MovementHandler>().SetDestination(999f);      
        }

    }

    public void StopParty(float dist_0, float dist_1, float dist_2)
    {
        _party[0].GetComponent<MovementHandler>().SetDestination(dist_0);

        if (_party.Count < 2) return;
        _party[1].GetComponent<MovementHandler>().SetDestination(dist_1);

        if (_party.Count < 3) return;
        _party[2].GetComponent<MovementHandler>().SetDestination(dist_2);
    }

    #endregion


}

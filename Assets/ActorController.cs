using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public static ActorController Instance { get; private set; }

    //settings


    //state


    Dictionary<ActorLibrary.ActorType, List<GameObject>> _currentActors = new Dictionary<ActorLibrary.ActorType, List<GameObject>>();
    [SerializeField] ActorLibrary.ActorType _selectedActorType;

    private void Awake()
    {
        Instance = this;
    }

    //private void SpawnSelectedActorInAppropriateCorner()
    //{
    //    Vector2 pos = Vector2.zero;
    //    if (ActorLibrary.Instance.GetActorPrefabFromActorType(_selectedActorType).GetComponent<IFFHandler>().Allegiance == 1)
    //    {
    //        pos = new Vector2(PlayerController.Instance.Pos.x, 0);
    //    }
    //    SpawnActor(_selectedActorType, pos);
    //}

    public void SpawnStartingKnight_Debug()
    {
        SpawnActor(ActorLibrary.ActorType.Knight3, Vector3.zero);
    }

    private void SpawnActor(ActorLibrary.ActorType actorType, Vector3 spawnPos)
    {
        GameObject prefabGO = ActorLibrary.Instance.GetActorPrefabFromActorType(actorType);
        if (!prefabGO) return;
        GameObject newGO = Instantiate(prefabGO, spawnPos, Quaternion.identity);
        if (_currentActors.ContainsKey(actorType))
        {
            _currentActors[actorType].Add(newGO);
        }
        else
        {
            List<GameObject> list = new List<GameObject>();
            list.Add(newGO);
            _currentActors.Add(actorType, list);
        }
        //return newGO;
    }

    public void SelectActorType(ActorLibrary.ActorType actorType)
    {
        _selectedActorType = actorType;
    }


}

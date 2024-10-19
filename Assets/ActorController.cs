using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public static ActorController Instance { get; private set; }

    //settings


    //state


    Dictionary<ActorLibrary.ActorTypes, List<GameObject>> _currentActors = new Dictionary<ActorLibrary.ActorTypes, List<GameObject>>();
    [SerializeField] ActorLibrary.ActorTypes _selectedActorType;

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



    public ActorHandler SpawnActor(ActorLibrary.ActorTypes actorType, Vector3 spawnPos)
    {
        GameObject prefabGO = ActorLibrary.Instance.GetActorPrefabFromActorType(actorType);

        if (!prefabGO)
        {
            Debug.Log("No prefab for this!");
            return null;
        }

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
        return newGO.GetComponent<ActorHandler>();
    }

    public void SelectActorType(ActorLibrary.ActorTypes actorType)
    {
        _selectedActorType = actorType;
    }


}

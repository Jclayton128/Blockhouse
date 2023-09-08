using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public static ActorController Instance { get; private set; }

    //settings
    [SerializeField] Vector2 _goodOffset = new Vector2(-7, 0);
    [SerializeField] Vector2 _badOffset = new Vector2(7, 0);

    //state
    Dictionary<ActorLibrary.ActorType, List<GameObject>> _currentActors = new Dictionary<ActorLibrary.ActorType, List<GameObject>>();
    [SerializeField] ActorLibrary.ActorType _selectedActorType;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InputController.Instance.Space_Down += SpawnSelectedActorInAppropriateCorner;
    }

    private void SpawnSelectedActorInAppropriateCorner()
    {
        Vector2 pos;
        if (ActorLibrary.Instance.GetActorPrefabFromActorType(_selectedActorType).GetComponent<IFFHandler>().IsGood)
        {
            pos = new Vector2(PlayerController.Instance.Pos.x, 0) + _goodOffset;
        }
        else
        {
            pos = new Vector2(PlayerController.Instance.Pos.x, 0) + _badOffset;
        }
        SpawnActor(_selectedActorType, pos);
    }

    private void SpawnSelectedActorUnderPlayer()
    {

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

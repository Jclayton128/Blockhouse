using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public static ActorController Instance { get; private set; }

    //state
    Dictionary<ActorLibrary.ActorType, List<GameObject>> _currentActors = new Dictionary<ActorLibrary.ActorType, List<GameObject>>();
    [SerializeField] ActorLibrary.ActorType _selectedActorType;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject SpawnActor(ActorLibrary.ActorType actorType, Vector3 spawnPos)
    {
        GameObject prefabGO = ActorLibrary.Instance.GetActorPrefabFromActorType(actorType);
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
        return newGO;
    }

    public void SelectActorType(ActorLibrary.ActorType actorType)
    {
        _selectedActorType = actorType;
    }


}

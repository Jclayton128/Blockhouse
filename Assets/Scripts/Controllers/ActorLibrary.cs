using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class ActorLibrary : SerializedMonoBehaviour
{
    public static ActorLibrary Instance { get; private set; }
    public enum ActorTypes
    {
        Cleric0, Crossbowman1, Farmer2, Knight3, Monk4, Peasant5, Ranger6, Wizard7,
        Goblin8, Ogre9, Orc10, Skeleton11, Slime12, Summoner13, Wraith14, Player15, Count
    }

    [SerializeField] Dictionary<ActorTypes, ActorHandler> _actorMenu = new Dictionary<ActorTypes, ActorHandler>();

    private void Awake()
    {
        Instance = this;
    }

    public GameObject GetActorPrefabFromActorType(ActorTypes actorType)
    {
        if (_actorMenu.ContainsKey(actorType))
        {
            return _actorMenu[actorType].gameObject;
        }
        else
        {
            Debug.LogWarning($"Actor Library does not contain a prefab of {actorType}");
            return null;
        }
    }

    public UIDataHandler GetUIDataFromActorType(ActorTypes actorType)
    {
        if (_actorMenu.ContainsKey(actorType))
        {
            return (_actorMenu[actorType].GetComponent<UIDataHandler>());
        }
        else
        {
            Debug.LogWarning("Menu doesn't contain " + actorType);
            return null;
        }
    }

    public List<ActorTypes> GetListOfLoadedActorTypes()
    {
        List<ActorTypes> list = new List<ActorTypes>();
        foreach (var key in _actorMenu.Keys)
        {
            list.Add(key);
        }
        return list;
    }
}

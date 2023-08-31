using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class ActorLibrary : SerializedMonoBehaviour
{
    public static ActorLibrary Instance { get; private set; }
    public enum ActorType
    {
        Cleric0, Crossbowman1, Farmer2, Knight3, Monk4, Peasant5, Ranger6, Wizard7,
        Goblin8, Ogre9, Orc10, Skeleton11, Slime12, Summoner13, Wraith14, Player15, Count
    }

    [SerializeField] Dictionary<ActorType, GameObject> _actorMenu;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject GetActorPrefabFromActorType(ActorType actorType)
    {
        if (_actorMenu.ContainsKey(actorType))
        {
            return _actorMenu[actorType];
        }
        else
        {
            Debug.LogWarning($"Actor Library does not contain a prefab of {actorType}");
            return null;
        }
    }

    public UIDataHandler GetUIDataFromActorType(ActorType actorType)
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

    public List<ActorType> GetListOfLoadedActorTypes()
    {
        List<ActorType> list = new List<ActorType>();
        foreach (var key in _actorMenu.Keys)
        {
            list.Add(key);
        }
        return list;
    }
}

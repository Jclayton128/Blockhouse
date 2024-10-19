using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActorSelectButtonDriver : MonoBehaviour
{
    [SerializeField] Image _actorIcon = null;
    [SerializeField] TextMeshProUGUI _actorName = null;

    //state
    public bool IsLoaded { get; private set; } = false;
    public ActorLibrary.ActorTypes ActorType { get; private set; }
        = ActorLibrary.ActorTypes.Count;


    public void LoadActorButton(Sprite icon, string name, ActorLibrary.ActorTypes actorType)
    {
        _actorIcon.sprite = icon;
        _actorIcon.color = Color.white;
        _actorName.text = name;
        IsLoaded = true;
        ActorType = actorType;
    }

    public void ClearActorButton()
    {
        _actorIcon.sprite = null;
        _actorIcon.color = Color.clear;
        _actorName.text = " ";
        IsLoaded = false;
        ActorType = ActorLibrary.ActorTypes.Count;
    }

    public void HandleButtonPress()
    {
        ActorController.Instance.SelectActorType(ActorType);
    }
}

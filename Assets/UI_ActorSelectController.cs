using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UI_ActorSelectController : MonoBehaviour
{
    public static UI_ActorSelectController Instance { get; private set; }
    [SerializeField] ActorSelectButtonDriver[] _actorSelectorButtons = null;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetupActorSelectButtons();
    }

    private void SetupActorSelectButtons()
    {
        var actors = ActorLibrary.Instance.GetListOfLoadedActorTypes();

        foreach (var button in _actorSelectorButtons)
        {
            button.ClearActorButton();
        }

        for (int i = 0; i < _actorSelectorButtons.Length; i++)
        {
            if (i < actors.Count)
            {
                UIDataHandler uidh = ActorLibrary.Instance.GetUIDataFromActorType(actors[i]);
                _actorSelectorButtons[i].LoadActorButton(uidh.Icon, uidh.Name, actors[i]);
            }
        }
    }
}

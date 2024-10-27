using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PartySelectPanelDriver : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _helperText = null;
    [SerializeField] Button _panelAsButton = null;

    private void Start()
    {
        ActorController.Instance.PartyModified += HandlePartyModified;
        HandlePartyModified();
    }

    private void HandlePartyModified()
    {
        int numberLeft = 3 - ActorController.Instance.Party.Count;
        if (numberLeft > 1) _helperText.text = $"Click {numberLeft} More Characters";
        else if (numberLeft == 1) _helperText.text = $"Click 1 More Character";
        else
        {
            _helperText.text = "Click Here to Start";
        }

        if (numberLeft > 0)
        {
            _panelAsButton.interactable = false;
        }
        else
        {
            _panelAsButton.interactable = true;
        }
    }
}

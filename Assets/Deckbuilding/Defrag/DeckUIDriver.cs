using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DeckUIDriver : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _cardsRemainingTMP = null;

    private void Start()
    {
        DeckHandler.InstanceDebug.DrawPileModified += HandleDrawPileModified;
        HandleDrawPileModified();
    }

    private void HandleDrawPileModified()
    {
        _cardsRemainingTMP.text = DeckHandler.InstanceDebug.CardsInDrawDeck.ToString();
    }
}

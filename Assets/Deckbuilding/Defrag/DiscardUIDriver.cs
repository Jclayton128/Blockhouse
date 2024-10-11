using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiscardUIDriver : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _cardsInPileTMP = null;

    private void Start()
    {
        DeckHandler.InstanceDebug.DiscardPileModified += HandleDiscardPileModified;
        HandleDiscardPileModified();
    }

    private void HandleDiscardPileModified()
    {
        _cardsInPileTMP.text = DeckHandler.InstanceDebug.CardsInDiscard.ToString();
    }
}

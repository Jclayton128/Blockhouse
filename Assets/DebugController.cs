using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    [SerializeField] DeckHandler _deckHandler = null;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.X))
        {
            Debug.Log("Discarding Entire Hand");
            _deckHandler.DiscardEntireHand();
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            Debug.Log("Drawing New Hand");
            _deckHandler.DrawCardsToHand(3);
        }

        if (Input.GetKeyUp(KeyCode.N))
        {
            Debug.Log("Adding Random New Card to Deck");
            Card card = CardLibrary.Instance.GetRandomCard();
            _deckHandler.AddCardToDiscard(card);
        }
    }
}

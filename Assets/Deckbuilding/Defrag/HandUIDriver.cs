using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandUIDriver : MonoBehaviour
{
    [SerializeField] CardUIDriver[] _cardUIs = null;

    public void LoadNewHand(List<Card> newCards)
    {
        if (newCards.Count > _cardUIs.Length)
        {
            Debug.LogError("More Cards arriving than UI can hold!");
            return;
        }

        foreach (var driver in _cardUIs)
        {
            driver.BlankCard();
        }
        for (int i = 0; i < newCards.Count; i++)
        {
            _cardUIs[i].LoadCard(newCards[i]);
        }
    }
}

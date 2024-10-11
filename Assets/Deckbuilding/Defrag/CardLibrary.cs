using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLibrary : MonoBehaviour
{
    static public CardLibrary Instance { get; private set; }

    [SerializeField] List<Card> _masterCardList = new List<Card>();

    private void Awake()
    {
        Instance = this;
    }

    public Card GetRandomCard()
    {
        int rand = UnityEngine.Random.Range(0, _masterCardList.Count);
        Card newCard = Instantiate(_masterCardList[rand]);
        return newCard;
    }
}

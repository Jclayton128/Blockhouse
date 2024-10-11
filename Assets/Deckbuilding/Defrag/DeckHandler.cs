using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Radial.Deck;
using System;

public class DeckHandler : MonoBehaviour
{
    public static DeckHandler InstanceDebug { get; private set; }

    [SerializeField] HandUIDriver _handDriver = null;
    //[SerializeField] DrawUIDriver _drawDriver = null;
    //[SerializeField] DiscardDriver _discardDriver = null;

    //settings
    [SerializeField] int _startingActionPoints = 3;
    [SerializeField] int _startingHandLimit = 3;


    //state
    [SerializeField] int _actionPoints;
    public int ActionPoints => _actionPoints;
    [SerializeField] int _handLimit;
    public int HandLimit => _handLimit;

    [SerializeField] Deck<Card> _deck = new Deck<Card>();
    [SerializeField] List<Card> _currentHand = new List<Card>();

    public int CardsInDrawDeck => _deck.Library.Size;
    public int CardsInDiscard => _deck.Discarded.Size;

    public Action DrawPileModified;
    public Action DiscardPileModified;

    private void Awake()
    {
        InstanceDebug = this;
    }

    public void Start()
    {
        _deck.ShufflingLibrary += HandleShufflingLibrary;
        _actionPoints = _startingActionPoints;
        _handLimit = _startingHandLimit;
    }

    private void HandleShufflingLibrary()
    {
        Debug.Log("Returning discards and then shuffling Library");
        DrawPileModified?.Invoke();
        DiscardPileModified?.Invoke();
    }

    public void AddCardToDiscard(Card card)
    {
        _deck.Discard(card);
        DiscardPileModified?.Invoke();
    }

    public void DrawCardsToHand(int numberOfCards)
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            Card newCard = _deck.Draw();
            _currentHand.Add(newCard);
            newCard.RevealCard();

        }
        DrawPileModified?.Invoke();
        _handDriver.LoadNewHand(_currentHand);
    }

    public void UseCard(int indexOfCardInHand)
    {
        Card selectedCard = _currentHand[indexOfCardInHand];
        selectedCard.UseCard();
        _currentHand.Remove(selectedCard);
        _deck.Discard(selectedCard);
        DiscardPileModified?.Invoke();
    }

    public void DiscardEntireHand()
    {
        for (int i = _currentHand.Count -1; i >= 0; i--)
        {
            Card discardedCard = _currentHand[i];
            discardedCard.DiscardCard();
            _currentHand.Remove(discardedCard);
            _deck.Discard(discardedCard);
        }
        DiscardPileModified?.Invoke();
        _handDriver.LoadNewHand(_currentHand);
    }

}

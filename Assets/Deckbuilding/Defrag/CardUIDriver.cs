using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUIDriver : MonoBehaviour
{
    //refs
    [SerializeField] TextMeshProUGUI _cardName = null;
    [SerializeField] Image _backgroundImage = null;
    [SerializeField] Image _cardImage = null;
    [SerializeField] Image _frameImage = null;
    [SerializeField] ImageAnimation _cardAnimation = null;
    //[SerializeField] Animation _frameAnimation = null;

    //state
    Card _card;

    public void LoadCard(Card card)
    {
        _card = card;
        RenderCard();
    }

    public void RenderCard()
    {
        if (_card)
        {
            _cardName.enabled = true;
            _backgroundImage.enabled = true;
            _frameImage.enabled = true;
            _cardImage.enabled = true;

            _cardName.text = _card.CardName;
            _backgroundImage.sprite = _card.Background;
            _frameImage.sprite = _card.Frame;
            _cardImage.sprite = _card.StaticSprite;
            _cardAnimation.LoadSprites(_card.HighlightFrames);
        }
        else
        {
            Debug.LogWarning("No Card to render!");
            BlankCard();
        }
    }

    public void BlankCard()
    {
        _cardName.enabled = false;
        _backgroundImage.enabled = false;
        _frameImage.enabled = false;
        _cardImage.enabled = false;
        //_frameAnimation.RemoveClip("clip");
        _cardAnimation.UnloadSprites();

    }

    public void HandleMouseEnter()
    {
        if (_card) _cardAnimation.IsPlaying = true;
    }

    public void HandleMouseExit()
    {
        if (_card) _cardAnimation.IsPlaying = false;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Card")]
public class Card : ScriptableObject
{
    #region Parameters inherent to Cards

    [Tooltip("Displayed Title Case name of the card")]
    [SerializeField] string _cardName = "Default Card";
    public string CardName => _cardName;

    [Tooltip("Current upgrade level of the card.")]
    [SerializeField] int _upgradeLevel = 0;
    public int UpgradeLevel => _upgradeLevel;

    [SerializeField] Sprite _background = null;
    public Sprite Background => _background;

    [SerializeField] Sprite _frame = null;
    public Sprite Frame => _frame;

    [Tooltip("Static artwork of the card for when not highlighted")]
    [SerializeField] Sprite _staticSprite = null;
    public Sprite StaticSprite => _staticSprite;


    //[Tooltip("Animation for the card when highlighted")]
    //[SerializeField] AnimationClip _highlightAnimation;
    //public AnimationClip HighlightAnimation => _highlightAnimation;

    [SerializeField] Sprite[] _highlightFrames = null;
    public Sprite[] HighlightFrames => _highlightFrames;

    #endregion

    #region Things that you do with Cards



    /// <summary>
    /// Called anytime this Card is drawn and presented faceup to the gameboard
    /// </summary>
    public void RevealCard()
    {
        //execute Reveal effect
    }


    /// <summary>
    /// Called anytime this Card is selected for use. This DOES NOT check if action point 
    /// prereqs are met, and DOES NOT modify remaining action points.
    /// </summary>
    public void UseCard()
    {        
        //execute Use effect
    }

    /// <summary>
    /// Called anytime this Card is discarded as a result of another Card's action.
    /// It is NOT called at the mass-discard at turn end.
    /// </summary>
    public void DiscardCard()
    {
        //execute Discard effect
    }

    #endregion




}

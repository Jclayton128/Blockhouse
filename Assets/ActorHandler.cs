using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// This hold the dice of each character. Dice themselves are immutable (ie, same type and number
/// of faces. However, the dice faces may change, especially for player characters
/// </summary>
public class ActorHandler : MonoBehaviour
{
    public Action ActorHighlighted;
    public Action ActorDehighlighted;
    public Action<ActorModes> ActorModeChanged;
    public enum ActorModes { Idling, Acting, Walking, AwaitingTitleScreenSelection}


    //refs
    [SerializeField] DiceHandler[] _diceHandlers = null;
    IFFHandler _iff;


    //settings
    [SerializeField] Dice[] _startingDice = null;
    [SerializeField] ActorLibrary.ActorTypes _actorType = ActorLibrary.ActorTypes.Cleric0;

    //state
    public ActorLibrary.ActorTypes ActorType => _actorType;
    DiceHandler _selectedDiceHandler;
    [SerializeField] ActorModes _actorMode = ActorModes.Idling;
    public ActorModes ActorMode => _actorMode;


    public void Initialize(IFFHandler.Allegiances allegiance)
    {
        GameController.Instance.GameModeChanged += HandleGameModeChanged;
        for (int i = 0; i < _diceHandlers.Length; i++)
        {
            _diceHandlers[i].SetOwningActor(this);
            _diceHandlers[i].LoadWithDice(_startingDice[i]);
            
        }
        HideDice(true);

        _iff = GetComponentInChildren<IFFHandler>();
        _iff.SetAllegiance(allegiance);
    }

    private void OnDestroy()
    {
        GameController.Instance.GameModeChanged -= HandleGameModeChanged;
    }

    private void HandleGameModeChanged(GameController.GameModes newMode)
    {
        switch (newMode)
        {
            case GameController.GameModes.Title:
                _actorMode = ActorModes.Acting;
                break;

            case GameController.GameModes.HeroSelect:
                _actorMode = ActorModes.AwaitingTitleScreenSelection;
                break;

            case GameController.GameModes.WalkingToNextEncounter:
                if (_iff.Allegiance == IFFHandler.Allegiances.Player)
                {
                    //Debug.Log("should be walking");
                    _actorMode = ActorModes.Walking;
                }
                else
                {
                    //do nothing, presumably this actor isn't part of the party and should be swept up
                    _actorMode = ActorModes.Idling;
                }
                break;

            case GameController.GameModes.InEncounter:
                _actorMode = ActorModes.Idling;
                break;
        }
        ActorModeChanged?.Invoke(_actorMode);
    }

    public void SetActorMode(ActorModes newMode)
    {
        _actorMode = newMode;
        ActorModeChanged?.Invoke(_actorMode);
    }


    public void RollDice()
    {
        foreach (var handler in _diceHandlers)
        {
            handler.RollDice();
        }
    }


    #region Show/Hide Dice Handlers

    [ContextMenu("Show Dice")]

    public void ShowDice()
    {
        foreach (var handler in _diceHandlers)
        {
            handler.ShowDice();
        }
    }

    [ContextMenu("Expand Dice")]
    public void ExpandDice()
    {
        if (_iff.Allegiance != IFFHandler.Allegiances.Player) return;
        foreach (var handler in _diceHandlers)
        {
            handler.Expand_Debug();
        }
    }

    [ContextMenu("Compact Dice")]
    public void CompactDice()
    {
        
        foreach (var handler in _diceHandlers)
        {
            handler.Compact_Debug();
        }
    }

    [ContextMenu("Hide Dice")]
    public void HideDice(bool isInstant)
    {
        foreach (var handler in _diceHandlers)
        {
            handler.HideDice(isInstant);
        }
    }

    #endregion

    #region Select/Lock Dice

    public void AttemptSelectDice(DiceHandler selectedDiceHandler)
    {
        if (_selectedDiceHandler == selectedDiceHandler)
        {
            Debug.Log("deselected");
            _selectedDiceHandler.ShowDiceAsDeselected();
            DeselectDice();
        }
        else if (_selectedDiceHandler != null)
        {
            Debug.Log("cannot select a second");
            //Cannot select a different dice if there is already a dice selected.
            return;
        }
        else
        {
            Debug.Log("selected");
            //Locks in the selected dice's active face as the move for this character
            //Command some kind of visual for the DiceHandler to display
            _selectedDiceHandler = selectedDiceHandler;
            _selectedDiceHandler.ShowDiceAsSelected();
        }



    }

    public void DeselectDice()
    {
        //Clears the active face and allows another to be selected.
        _selectedDiceHandler = null;
    }

    #endregion

    #region Select Character

    private void OnMouseEnter()
    {
        if (GameController.Instance.GameMode == GameController.GameModes.Title ||
            GameController.Instance.GameMode == GameController.GameModes.WalkingToNextEncounter) return;

        ActorHighlighted?.Invoke();

        if (_actorMode == ActorModes.AwaitingTitleScreenSelection)
        {
            ShowDice();
        }
    }

  

    private void OnMouseExit()
    {
        if (GameController.Instance.GameMode == GameController.GameModes.Title ||
            GameController.Instance.GameMode == GameController.GameModes.WalkingToNextEncounter) return;

        //ActorDehighlighted?.Invoke();

        if (_actorMode == ActorModes.AwaitingTitleScreenSelection)
        {
            HideDice(false);
        }
    }

    private void OnMouseUpAsButton()
    {
        if (_actorMode == ActorModes.AwaitingTitleScreenSelection)
        {
            //Select this as player char
            //Debug.Log("Selected this player", this);
            HideDice(false);
            _iff.SetAllegiance(IFFHandler.Allegiances.Player);
            ActorController.Instance.AddActorToParty(this);
            GameController.Instance.SetGameMode(GameController.GameModes.WalkingToNextEncounter);
        }
    }

    #endregion

}
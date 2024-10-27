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
    public Action ActorSelected;
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
    bool _areDiceExpanded = true;



    [ContextMenu("Init Debug")]
    public void InitAsPlayer_Debug()
    {
        Initialize(IFFHandler.Allegiances.Player);
    }

    public void Initialize(IFFHandler.Allegiances allegiance)
    {
        GameController.Instance.GameModeChanged += HandleGameModeChanged;
        for (int i = 0; i < _diceHandlers.Length; i++)
        {
            _diceHandlers[i].SetOwningActor(this);
            if (i < _startingDice.Length)
            {
                _diceHandlers[i].LoadWithDice(_startingDice[i]);
            }            
        }
        HideDice(true);
        CompactDice();

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
                    _actorMode = ActorModes.Walking;
                }
                else
                {
                    _actorMode = ActorModes.Idling;
                }
                break;

            case GameController.GameModes.EncounterIntro:
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

    [ContextMenu("Roll Dice")]
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
        //if (_iff.Allegiance != IFFHandler.Allegiances.Player) return;
        foreach (var handler in _diceHandlers)
        {

            handler.Expand_Debug();
        }
        _areDiceExpanded = true;
    }

    [ContextMenu("Compact Dice")]
    public void CompactDice()
    {
        
        foreach (var handler in _diceHandlers)
        {
            handler.Compact_Debug();
        }
        _areDiceExpanded = false;
    }

    [ContextMenu("Hide Dice")]
    public void HideDice(bool isInstant)
    {
        //if (!isInstant && _areDiceExpanded) CompactDice();

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
        if (GameController.Instance.GameMode == GameController.GameModes.Title) return;


        if (_actorMode == ActorModes.AwaitingTitleScreenSelection)
        {
            ExpandDice();
            ShowDice();

            ActorHighlighted?.Invoke();
        }

    }  

    private void OnMouseExit()
    {
        if (_actorMode == ActorModes.AwaitingTitleScreenSelection)
        {
            HideDice(false);
            CompactDice();


        //ActorDehighlighted?.Invoke();
        }

    }

    private void OnMouseUpAsButton()
    {
        if (_actorMode == ActorModes.AwaitingTitleScreenSelection)
        {
            //HideDice(false);
            ActorController.Instance.SelectCharacter(this);
            if (ActorController.Instance.Party.Contains(this))
            {
                ActorSelected?.Invoke();
                _iff.SetAllegiance(IFFHandler.Allegiances.Player);
            }
            else
            {
                ActorDehighlighted?.Invoke();
                _iff.SetAllegiance(IFFHandler.Allegiances.Undefined);
            }


            return;
        }

        if (GameController.Instance.GameMode == GameController.GameModes.EncounterInspection)
        {
            _areDiceExpanded = !_areDiceExpanded;
            if (_areDiceExpanded)
            {

                Invoke(nameof(Delay_HandleInspectClick), 0.4f);
                ActorController.Instance.CompactHideAllPartyDiceExceptThis(this);
                ActorController.Instance.CompactHideAllEncounterDiceExceptThis(this);
            }
            else
            {
                UIController.Instance.HideInspectionPanels();
                //Invoke(nameof(Delay_HandleInspectClick), 0.4f);
                CompactDice();
            }
        }
    }

    private void Delay_HandleInspectClick()
    {
        UIController.Instance.ShowInspectionPanels();
        ExpandDice();
        ShowDice();

        //if (_areDiceExpanded)
        //{
        //    ExpandDice();
        //    ShowDice();

        //}
        //else
        //{
        //    ActorController.Instance.ShowEncounterDice();
        //    ActorController.Instance.ShowPartyDice();
        //}
    }

    #endregion

}

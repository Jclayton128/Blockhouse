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
    [SerializeField] DiceHandler _diceHandler = null;
    public DiceHandler MyDiceHandler => _diceHandler;
    IFFHandler _iff;


    //settings
    [SerializeField] Dice _startingDice = null;
    [SerializeField] ActorLibrary.ActorTypes _actorType = ActorLibrary.ActorTypes.Cleric0;

    //state
    public ActorLibrary.ActorTypes ActorType => _actorType;
    [SerializeField] ActorModes _actorMode = ActorModes.Idling;
    public ActorModes ActorMode => _actorMode;



    public void Initialize(IFFHandler.Allegiances allegiance)
    {
        GameController.Instance.GameModeChanged += HandleGameModeChanged;
        MyDiceHandler.LoadWithDice(_startingDice);
        
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


    #region Select Character

    private void OnMouseEnter()
    {
        if (GameController.Instance.GameMode == GameController.GameModes.Title) return;


        if (_actorMode == ActorModes.AwaitingTitleScreenSelection)
        {
            MyDiceHandler.Vis_ActivateFadeInExpandDice();
            ActorHighlighted?.Invoke();
        }

    }  

    private void OnMouseExit()
    {
        if (_actorMode == ActorModes.AwaitingTitleScreenSelection)
        {
            MyDiceHandler.Vis_CompactFadeAwayDeactivateDice();


        //ActorDehighlighted?.Invoke();
        }

    }

    private void OnMouseUpAsButton()
    {
        if (_actorMode == ActorModes.AwaitingTitleScreenSelection)
        {
            HandleSelectionToParty();
            return;
        }

        if (GameController.Instance.GameMode == GameController.GameModes.EncounterInspection)
        {
            bool isExpanded = MyDiceHandler.Vis_ToggleExpandCompactDice();
            if (isExpanded)
            {
                Invoke(nameof(Delay_HandleInspectClick), 0.4f);
                ActorController.Instance.CompactHideAllDiceExceptThis(this);
            }
            else
            {
                UIController.Instance.HideInspectionPanels();
                MyDiceHandler.Vis_CompactFadeAwayDeactivateDice();
            }
        }
    }

    private void HandleSelectionToParty()
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
    }

    private void Delay_HandleInspectClick()
    {
        UIController.Instance.ShowInspectionPanels();
        MyDiceHandler.Vis_ActivateFadeInExpandDice();
    }

    #endregion

}

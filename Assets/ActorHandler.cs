using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This hold the dice of each character. Dice themselves are immutable (ie, same type and number
/// of faces. However, the dice faces may change, especially for player characters
/// </summary>
public class ActorHandler : MonoBehaviour
{
    //refs
    [SerializeField] DiceHandler[] _diceHandlers = null;


    //settings
    [SerializeField] Dice[] _startingDice = null;

    //state
    DiceHandler _selectedDiceHandler;

    private void Start()
    {
        for (int i = 0; i < _diceHandlers.Length; i++)
        {
            _diceHandlers[i].SetOwningActor(this);
            _diceHandlers[i].LoadWithDice(_startingDice[i]);
            
        }
        ShowDice();
        
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
    public void HideDice()
    {
        foreach (var handler in _diceHandlers)
        {
            handler.HideDice();
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

}

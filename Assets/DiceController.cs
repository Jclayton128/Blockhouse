using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController : MonoBehaviour
{
    [SerializeField] DiceHandler[] _diceInPlay = null;

    [Header("Testing")]
    [SerializeField] Dice[] _diceToLoad_Debug = null;

    //state 
    int count = 0;

    private void Start()
    {
        LoadDiceIntoDiceHandlers_Debug();
    }

    [ContextMenu("Reload Dice Into DiceHandlers")]
    private void LoadDiceIntoDiceHandlers_Debug()
    {
        count = 0;
        foreach (var handler in _diceInPlay)
        {   
            handler.LoadWithDice(_diceToLoad_Debug[count]);
            count++;
        }
    }

    [ContextMenu("Roll All Dice Handlers")]
    public void RollAllDiceHandlers()
    {
        foreach (var dice in _diceInPlay)
        {
            dice.RollDice();
        }
    }
}

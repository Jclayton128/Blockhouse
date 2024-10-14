using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController : MonoBehaviour
{
    [SerializeField] DiceHandler[] _diceInPlay = null;

    [Header("Testing")]
    [SerializeField] Dice[] _diceToLoad_Debug = null;



    private void Start()
    {
        LoadDiceIntoDiceHandlers_Debug();
    }

    private void LoadDiceIntoDiceHandlers_Debug()
    {
        int rand = UnityEngine.Random.Range(0, _diceToLoad_Debug.Length);
        foreach (var handler in _diceInPlay)
        {
            handler.LoadWithDice(_diceToLoad_Debug[rand]);
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

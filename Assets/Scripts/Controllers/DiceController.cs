using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController : MonoBehaviour
{
    //state
    [SerializeField] List<DiceHandler> _diceInUse = new List<DiceHandler>();

    public void AddDiceInUse(DiceHandler dice)
    {
        _diceInUse.Add(dice);
    }

    public void RemoveDiceInUse(DiceHandler dice)
    {
        _diceInUse.Remove(dice);
    }

    public void Vis_HeroSelectMode(DiceHandler heroSelected)
    {
        //foreach (var dice in _diceInUse)
        //{
        //    dice.Vis
        //}
    }

  
}

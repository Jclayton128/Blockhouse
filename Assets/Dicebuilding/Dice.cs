using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dice")]
public class Dice : ScriptableObject
{
    public enum DiceTypes { Light, Medium, Heavy }

    //settings
    int _faceLimit = 6;


    //state
    [SerializeField] DiceTypes _diceType = DiceTypes.Light;
    public DiceTypes DiceType => _diceType;
    [SerializeField] DiceFace[] _diceFaces = null;



    //Validates the dice and then returns it if valid, else Null;
    public DiceFace[] GetFaces()
    {
        foreach (var face in _diceFaces)
        {
            if (face.DiceType != _diceType)
            {
                Debug.LogWarning("Dice Type Mismatch!");
            }
        }



        if (_diceFaces.Length == 0)
        {
            Debug.LogWarning("Dice has no faces on it!");
            return null;
        }
        else if (_diceFaces.Length <= _faceLimit)
        {
            return _diceFaces;
        }
        else
        {
            Debug.LogWarning("Dice has too many faces on it!");
            return null;
        }
    }
}

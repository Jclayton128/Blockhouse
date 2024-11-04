using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dice")]
public class Dice : ScriptableObject
{
    public enum DiceSpeeds { Light, Medium, Heavy, Undefined, Slowed }

    //settings
    int _faceLimit = 6;


    //state
    [SerializeField] DiceFace[] _diceFaces = null;



    //Validates the dice and then returns it if valid, else Null;
    public DiceFace[] GetFaces()
    {
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

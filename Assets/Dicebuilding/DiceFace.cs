using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Dice Face")]
public class DiceFace : ScriptableObject
{
    public enum FaceAlignment { Beast, Arcane, Hearth, Order, Nomad, Discord };
    public enum DiceLevels { Basic, Good, Better, Best };

    [SerializeField] string _faceName;
    [SerializeField] DiceLevels _diceLevel = DiceLevels.Basic;

    [SerializeField] Sprite _faceSprite = null;
    [SerializeField] Dice.DiceSpeeds _diceSpeeds = Dice.DiceSpeeds.Light;

    [SerializeField] FaceAlignment _faceType = FaceAlignment.Beast;



    public string FaceName => _faceName;
    public DiceLevels DiceLevel => _diceLevel;
    public Sprite FaceSprite => _faceSprite;
    public Dice.DiceSpeeds DiceSpeed => _diceSpeeds;
    public FaceAlignment FaceType => _faceType;






    


}

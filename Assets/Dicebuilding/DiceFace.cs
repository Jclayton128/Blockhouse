using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Dice Face")]
public class DiceFace : ScriptableObject
{
    public enum FaceTypes { Beast, Arcane, Hearth, Order, Nomad, Discord };

    [SerializeField] Sprite _faceSprite = null;
    [SerializeField] Dice.DiceTypes _diceType = Dice.DiceTypes.Light;
    [SerializeField] FaceTypes _faceType = FaceTypes.Beast;


    public Sprite FaceSprite => _faceSprite;
    public Dice.DiceTypes DiceType => _diceType;
    public FaceTypes FaceType => _faceType;

    public enum DiceLevels {Basic, Good, Better, Best }
    [SerializeField] DiceLevels _diceLevel = DiceLevels.Basic;
    public DiceLevels DiceLevel => _diceLevel;


    


}

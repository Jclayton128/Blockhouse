using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Dice Face")]
public class DiceFace : ScriptableObject
{
    public enum FaceAlignments { Beast, Arcane, Hearth, Order, Nomad, Discord };
    

    /// <summary>
    ///  MELEE: first enemy. SNIPE: last enemy. BARRAGE: Random single enemy. SPRAY: All enemies. PARTY: all party characters. SELF: self
    /// </summary>
    public enum Ranges { Melee, Snipe, Barrage, Spray, Party, Self}
    public enum DiceLevels { Basic, Good, Better, Best };



    [SerializeField] string _faceName;
    [SerializeField] Dice.DiceSpeeds _diceSpeeds = Dice.DiceSpeeds.Light;
    [SerializeField] Ranges _range = Ranges.Melee;
    [SerializeField] Sprite _faceSprite = null;
    [SerializeField] FaceAlignments _faceType = FaceAlignments.Beast;
    [SerializeField] DiceLevels _diceLevel = DiceLevels.Basic;



    public string FaceName => _faceName;
    public DiceLevels DiceLevel => _diceLevel;
    public Ranges Range => _range;
    public Sprite FaceSprite => _faceSprite;
    public Dice.DiceSpeeds DiceSpeed => _diceSpeeds;
    public FaceAlignments FaceAlignment => _faceType;






    


}

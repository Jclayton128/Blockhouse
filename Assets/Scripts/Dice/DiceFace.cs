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
    public enum Ranges { FirstEnemy, LastEnemy, RandomEnemy, AllEnemy, AllParty, Self}
    public enum Effects { Attack, Defend, Heal, Dodge, Stun, Slow }
    public enum DiceLevels { Basic, Good, Better, Best };
    public enum Animations { Attack, Cast, Cheer}


    [Header("Parameters")]
    [SerializeField] string _faceName;
    [SerializeField] Sprite _faceSprite = null;
    [SerializeField] Dice.DiceSpeeds _diceSpeeds = Dice.DiceSpeeds.Light;
    [SerializeField] Ranges _range = Ranges.FirstEnemy;
    [SerializeField] Effects _effect = Effects.Attack;
    [SerializeField] int _magnitude = 1;
    [SerializeField] Animations _animation = Animations.Attack;

    [Header("Unused")]
    [SerializeField] FaceAlignments _faceType = FaceAlignments.Beast;
    [SerializeField] DiceLevels _diceLevel = DiceLevels.Basic;



    public string FaceName => _faceName;
    public DiceLevels DiceLevel => _diceLevel;
    public Ranges Range => _range;
    public Sprite FaceSprite => _faceSprite;
    public Effects Effect => _effect;
    public int Magnitude => _magnitude;
    public Dice.DiceSpeeds DiceSpeed => _diceSpeeds;
    public FaceAlignments FaceAlignment => _faceType;
    public Animations Animation => _animation;
    public ActorHandler OwningActor;






    


}

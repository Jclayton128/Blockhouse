using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPacket : Object
{
    public DiceFace.Effects Effect;
    public int Magnitude;

    public EffectPacket(DiceFace.Effects effect, int magnitude)
    {
        Effect = effect;
        Magnitude = magnitude;
    }


}

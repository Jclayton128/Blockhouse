using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerLibrary
{
    public static int GoodActor_Layer = 8;
    public static int BadActor_Layer = 9;
    public static int NeutralActor_Layer = 10;

    public static int GoodProjectile_Layer = 11;
    public static int BadProjectile_Layer = 11;

    public static int GoodActor_LayerMask = 1 << 8;
    public static int BadActor_LayerMask = 1 << 9;
    public static int NeutralActor_LayerMask = 1 << 10;
    public static int GoodProjectile_LayerMask = 1 << 11;
    public static int BadProjectile_LayerMask = 1 << 11;

    static int tick;
    static float[] _visualLayers = {0.064f, 0.128f, 0.196f}; //{ 0, 0.064f, 0.128f, 0.196f, 0.256f };

    public static Vector3 GetRandomVisualLayer()
    {
        int rand = UnityEngine.Random.Range(0, _visualLayers.Length);
        return new Vector3(0,_visualLayers[rand], 0);
    }

    public static Vector3 GetNextVisualLayer()
    {
        tick++;
        if (tick >= _visualLayers.Length) tick = 0;
        return new Vector3(0, _visualLayers[tick], 0);
    }
}

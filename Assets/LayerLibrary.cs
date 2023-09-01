using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerLibrary
{
    public static int GoodActor_Layer = 7;
    public static int BadActor_Layer = 8;
    public static int GoodProjectile_Layer = 9;
    public static int BadProjectile_Layer = 10;

    public static int GoodActor_LayerMask = 1 << 7;
    public static int BadActor_LayerMask = 1 << 8;
    public static int GoodProjectile_LayerMask = 1 << 9;
    public static int BadProjectile_LayerMask = 1 << 10;

}

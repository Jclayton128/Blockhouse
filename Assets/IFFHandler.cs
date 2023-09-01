using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFFHandler : MonoBehaviour
{
    public bool IsPlayer = false;
    public bool IsGood = true;
    public ActorLibrary.ActorType ActorType_ = ActorLibrary.ActorType.Cleric0;

    private void Awake()
    {
        if (IsGood)
        {
            gameObject.layer = LayerLibrary.GoodActor_Layer;
        }
        else
        {
            gameObject.layer = LayerLibrary.BadActor_Layer;
        }
    }
}

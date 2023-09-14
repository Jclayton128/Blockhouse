using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFFHandler : MonoBehaviour
{
    
    public bool IsPlayer = false;
    [SerializeField] int _allegiance = 1;
    /// <summary>
    /// Allegiance of 0 is neutral/undef, 1 = good, -1 = evil
    /// </summary>
    public int Allegiance => _allegiance;
    public ActorLibrary.ActorType ActorType_ = ActorLibrary.ActorType.Cleric0;

    private void Awake()
    {
        if (Allegiance == 1)
        {
            gameObject.layer = LayerLibrary.GoodActor_Layer;
        }
        else if (Allegiance == -1)
        {
            gameObject.layer = LayerLibrary.BadActor_Layer;
        }
    }
}

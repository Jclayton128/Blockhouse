using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFFHandler : MonoBehaviour
{
    public enum Allegiances { Player, Enemy, Neutral}
    
    [SerializeField] Allegiances _allegiance = Allegiances.Player;
    public Allegiances Allegiance => _allegiance;

    [SerializeField] Transform _visuals = null;

    public ActorLibrary.ActorType ActorType_ = ActorLibrary.ActorType.Cleric0;

    private void Awake()
    {
        if (Allegiance == Allegiances.Player)
        {
            gameObject.layer = LayerLibrary.GoodActor_Layer;
            _visuals.localScale = new Vector3(1, 1, 1);
        }
        else if (Allegiance == Allegiances.Enemy)
        {
            gameObject.layer = LayerLibrary.BadActor_Layer;
            _visuals.localScale = new Vector3(-1, 1, 1);
        }
        else if (Allegiance == Allegiances.Neutral)
        {
            gameObject.layer = LayerLibrary.NeutralActor_Layer;
            _visuals.localScale = new Vector3(-1, 1, 1);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFFHandler : MonoBehaviour
{
    public enum Allegiances { Player, Enemy, Neutral, Undefined}
    
    [SerializeField] Allegiances _allegiance = Allegiances.Player;
    public Allegiances Allegiance => _allegiance;

    [SerializeField] Transform _visuals = null;

    public ActorLibrary.ActorTypes ActorType_ = ActorLibrary.ActorTypes.Cleric0;

    public void SetAllegiance(Allegiances allegiance)
    {
        _allegiance = allegiance;
        if (_allegiance == Allegiances.Player)
        {
            gameObject.layer = LayerLibrary.GoodActor_Layer;
            _visuals.localScale = new Vector3(1, 1, 1);
        }
        else if (_allegiance == Allegiances.Enemy)
        {
            gameObject.layer = LayerLibrary.BadActor_Layer;
            _visuals.localScale = new Vector3(-1, 1, 1);
        }
        else if (_allegiance == Allegiances.Neutral)
        {
            gameObject.layer = LayerLibrary.NeutralActor_Layer;
            _visuals.localScale = new Vector3(-1, 1, 1);
        }
    }
}

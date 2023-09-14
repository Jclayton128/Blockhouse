using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainProfile_Conquering : MonoBehaviour, BrainProfile
{
    [SerializeField] BuildingHandler _buildingToConquer;
    ActorBrain _ab;
    IFFHandler _ih;
    [SerializeField] float _conquerRate = .2f;

    public void ExecuteStartup(ActorBrain actorBrain)
    {
        _ih = GetComponent<IFFHandler>();
        _ab = actorBrain;
    }

    public void ExecuteUpdate(ActorBrain actorBrain)
    {
        if (_buildingToConquer)
        {
            _buildingToConquer.ContinueConqueringBuilding(_conquerRate);
            _ab.CommandedMoveDir = 0;
        }
        else
        {
            _ab.CommandedMoveDir = _ih.Allegiance;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        BuildingHandler bh;
        if (collision.TryGetComponent<BuildingHandler>(out bh))
        {
            if (bh.Owner != _ih.Allegiance)
            {
                _buildingToConquer = bh;
                bh.BeginConqueringBuilding(_ih.Allegiance);
                _buildingToConquer.BuildingConquered += HandleBuildingConquered;
            }
        }
    }


    public void OnTriggerExit2D(Collider2D collision)
    {
        BuildingHandler bh;
        if (collision.TryGetComponent<BuildingHandler>(out bh))
        {
            if (bh == _buildingToConquer)
            {
                _buildingToConquer.BuildingConquered -= HandleBuildingConquered;
                HandleBuildingConquered();
            }
        }
    }

    private void HandleBuildingConquered()
    {
        _buildingToConquer?.CancelConqueringBuilding();
        _buildingToConquer = null;
    }
}

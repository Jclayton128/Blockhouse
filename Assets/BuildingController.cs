using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    public static BuildingController Instance { get; private set; }

    List<BuildingHandler> _bohs = new List<BuildingHandler>();
    private void Awake()
    {
        Instance = this;
        var gos = GameObject.FindGameObjectsWithTag("Building");
        foreach (var go in gos)
        {
            _bohs.Add(go.GetComponent<BuildingHandler>());
        }
    }

    private void Start()
    {

    }

    [ContextMenu("Test GetFurthestBlockhouse")]
    public void Test()
    {
        var bh_good = GetFurthestBlockhouse(1);
        var bh_bad = GetFurthestBlockhouse(-1);

        Debug.Log($"{bh_good.name} is furthest good. {bh_bad.name} is furthest bad");
    }

    public BuildingHandler GetFurthestBlockhouse(int allegiance)
    {
        BuildingHandler furthestBH = null;
        foreach (var boh in _bohs)
        {
            if (boh.Owner != allegiance) continue;
            if (furthestBH == null)
            {
                furthestBH = boh;
                continue;
            }
            if (allegiance == 1)
            {
                if (boh.transform.position.x > furthestBH.transform.position.x)
                {
                    furthestBH = boh;
                }
            }
            else if (allegiance == -1)
            {
                if (boh.transform.position.x < furthestBH.transform.position.x)
                {
                    furthestBH = boh;
                }
            }

        }
        if (furthestBH == null) Debug.Log($"No blockhouses of {allegiance} allegiance were found");
        return furthestBH;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterLibrary : MonoBehaviour
{
    public static EncounterLibrary Instance { get; private set; }


    //settings
    [SerializeField] Encounter[] _encounters = null;

    private void Awake()
    {
        Instance = this;
    }


    public Encounter FindValidRandomEncounter(float currentPosition)
    {
        List<Encounter> validEncs = new List<Encounter>();

        foreach (var enc in _encounters)
        {
            if (currentPosition >= enc.EarliestPos && currentPosition <= enc.LatestPos)
            {
                validEncs.Add(enc);
            }
        }

        if (validEncs.Count > 0)
        {
            int rand = UnityEngine.Random.Range(0, validEncs.Count);    
            return validEncs[rand];
        }
        else
        {
            Debug.LogWarning("No valid encounters available!");
            return null;
        }

    }

}

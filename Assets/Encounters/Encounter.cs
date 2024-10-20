using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Encounter")]
public class Encounter : ScriptableObject
{
    [SerializeField] float _earliestPos = 10f;
    public float EarliestPos => _earliestPos;
    [SerializeField] float _latestPost = 200f;
    public float LatestPos => _latestPost;

    [SerializeField] ActorLibrary.ActorTypes[] _actors = null;
    public ActorLibrary.ActorTypes[] Actors => _actors;

    [SerializeField] DiceFace[] _rewards = null;
    public DiceFace[] Rewards => _rewards;
}

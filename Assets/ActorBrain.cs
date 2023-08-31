using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorBrain : MonoBehaviour
{
    //settings

    //state
    public int CommandedMoveDir = 0;
    public int CommandedFlyDir = 0;
    public bool IsMovementPaused = false;
    public bool IsPlayer { get; private set; }
    BrainProfile _brainProfile = null;

    void Start()
    {
        _brainProfile = GetComponent<BrainProfile>();
        if (_brainProfile == null)
        {
            Debug.LogWarning("This actor is missing a brain profile");
            return;
        }
        _brainProfile?.ExecuteStartup(this);
    }

    void Update()
    {
        _brainProfile?.ExecuteUpdate(this);
    }
}

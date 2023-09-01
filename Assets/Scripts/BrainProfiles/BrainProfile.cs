using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BrainProfile
{
    public void ExecuteStartup(ActorBrain actorBrain);

    public void ExecuteUpdate(ActorBrain actorBrain);
}

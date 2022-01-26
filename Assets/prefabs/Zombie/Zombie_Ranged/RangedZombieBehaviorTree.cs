using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedZombieBehaviorTree : BehaviorTree
{
    public override void Init(AIController aiController)
    {
        base.Init(aiController);

        Sequence PatrollingSequence = new Sequence(AIC);
        PatrollingSequence.AddChild(new BTTask_MoveToNextPatrolPoint(AIC, 1f));
        PatrollingSequence.AddChild(new BTTask_Wait(AIC, 4));

        SetRootNode(PatrollingSequence);
    }
}

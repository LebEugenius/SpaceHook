using System.Collections;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;

public enum HookStates
{
    Waiting,
    Moving,
    Returning,
    Dead,
    Count
}

public class HookStateFactory
{
    readonly HookStateWaiting.Factory _waitingFactory;
    readonly HookStateMoving.Factory _movingFactory;
    readonly HookStateReturning.Factory _returningFactory;
    readonly HookStateDead.Factory _deadFactory;

    public HookStateFactory(
        HookStateWaiting.Factory waitingFactory,
        HookStateMoving.Factory movingFactory,
        HookStateReturning.Factory returningFactory,
        HookStateDead.Factory deadFactory)
    {
        _waitingFactory = waitingFactory;
        _movingFactory = movingFactory;
        _returningFactory = returningFactory;
        _deadFactory = deadFactory;
    }

    public HookState CreateState(HookStates state)
    {
        switch (state)
        {
            case HookStates.Waiting:
            {
                return _waitingFactory.Create();
            }
            case HookStates.Moving:
            {
                return _movingFactory.Create();
            }
            case HookStates.Returning:
            {
                return _returningFactory.Create();
            }
            case HookStates.Dead:
            {
                return _deadFactory.Create();
            }
        }

        throw Assert.CreateException();
    }
}

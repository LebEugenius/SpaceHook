using System.Collections;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;

public enum AlienStates
{
    Waiting,
    Moving,
    Catched,
    Count
}

public class AlienStateFactory
{
    readonly AlienStateWaiting.Factory _waitingFactory;
    readonly AlienStateMoving.Factory _movingFactory;
    readonly AlienStateCatched.Factory _catchedFactory;

    public AlienStateFactory(
        AlienStateWaiting.Factory waitingFactory,
        AlienStateMoving.Factory movingFactory,
        AlienStateCatched.Factory catchedFactory)
    {
        _waitingFactory = waitingFactory;
        _movingFactory = movingFactory;
        _catchedFactory = catchedFactory;
    }

    public AlienState CreateState(AlienStates state)
    {
        switch (state)
        {
            case AlienStates.Waiting:
            {
                return _waitingFactory.Create();
            }
            case AlienStates.Moving:
            {
                return _movingFactory.Create();
            }
            case AlienStates.Catched:
            {
                return _catchedFactory.Create();
            }
        }

        throw Assert.CreateException();
    }
}

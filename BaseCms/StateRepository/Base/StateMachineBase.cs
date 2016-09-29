using System.Collections.Generic;

namespace BaseCms.StateRepository.Base
{
    public abstract class StateMachineBase
    {
        public abstract StateBase GetState(int value);

        public abstract IEnumerable<StateBase> GetStates();
    }
}

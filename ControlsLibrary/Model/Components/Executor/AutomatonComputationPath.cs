using System.Collections.Generic;
using System.Linq;
using ControlsLibrary.Infrastructure;
using GraphX.Common;

namespace ControlsLibrary.Model.Components.Executor
{
    public class AutomatonComputationPath : BaseNotifyPropertyChanged
    {
        private State state;
        private ExecutionStatusEnum status;
        private readonly List<State> stateHistory = new List<State>();

        public IReadOnlyList<State> StateHistory => stateHistory;

        public IReadOnlyList<IAutomatonMemory> MemoryList { get; }

        public State State
        {
            get => state;
            private set
            {
                stateHistory.Add(value);
                if (value != null)
                {
                    value.IsCurrent = true;
                }

                Set(ref state, value);
            }
        }

        public ExecutionStatusEnum Status
        {
            get => status;
            private set => Set(ref status, value);
        }

        public AutomatonComputationPath(IReadOnlyList<IAutomatonMemory> memoryList, State initialState)
        {
            MemoryList = memoryList.Select(memory => memory.Copy()).ToList();
            State = initialState;
            Status = ExecutionStatusEnum.Running;
            UpdateStatus();
        }

        public AutomatonComputationPath(AutomatonComputationPath other)
        {
            MemoryList = other.MemoryList.Select(memory => memory.Copy()).ToList();
            State = other.State;
            Status = other.Status;
            stateHistory = new List<State>(other.StateHistory);
        }

        public void TakeTransition(Transition transition)
        {
            State = transition.Target;
            MemoryList.ForEach(memory => memory.TakeTransition(transition));
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            if (state.IsFinal && MemoryList.All(memory => memory.IsReadyToTerminate))
            {
                Status = ExecutionStatusEnum.Accepted;
            }
            else if (MemoryList.Any(memory => memory.RequiresTermination))
            {
                Status = state.IsFinal ? ExecutionStatusEnum.Accepted : ExecutionStatusEnum.Rejected;
            }
        }

        public void GoToFailedState()
        {
            State = null;
            Status = ExecutionStatusEnum.Rejected;
        }
    }
}
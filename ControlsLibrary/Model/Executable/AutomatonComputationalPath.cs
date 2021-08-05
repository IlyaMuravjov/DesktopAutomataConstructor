using System.Collections.Generic;
using System.Linq;
using ControlsLibrary.Infrastructure;
using GraphX.Common;

namespace ControlsLibrary.Model.Executable
{
    public class AutomatonComputationalPath : BaseNotifyPropertyChanged
    {
        private State state;
        private ExecutionStatusEnum status;
        private readonly List<State> stateHistory = new List<State>();

        public IReadOnlyList<State> StateHistory => stateHistory;

        public IReadOnlyList<IAutomatonComponent> Components { get; }

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

        public AutomatonComputationalPath(IReadOnlyList<IAutomatonComponent> components, State initialState)
        {
            Components = components.Select(component => component.Copy()).ToList();
            State = initialState;
            Status = ExecutionStatusEnum.Running;
            UpdateStatus();
        }

        public AutomatonComputationalPath(AutomatonComputationalPath other)
        {
            Components = other.Components.Select(component => component.Copy()).ToList();
            State = other.State;
            Status = other.Status;
            stateHistory = new List<State>(other.StateHistory);
        }

        public void TakeTransition(Transition transition)
        {
            State = transition.Target;
            Components.ForEach(component => component.TakeTransition(transition));
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            if (state.IsFinal && Components.All(component => component.IsReadyToTerminate))
            {
                Status = ExecutionStatusEnum.Accepted;
            }
            else if (Components.Any(component => component.RequiresTermination))
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
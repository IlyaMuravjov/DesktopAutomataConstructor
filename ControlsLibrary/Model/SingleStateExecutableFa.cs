using System.Collections.Generic;
using System.Linq;
using ControlsLibrary.Infrastructure;
using GraphX.Common;

namespace ControlsLibrary.Model
{
    public class SingleStateExecutableFa : BaseNotifyPropertyChanged
    {
        private State state;
        private FaStatusEnum status;
        private readonly List<State> stateHistory = new List<State>();

        public IReadOnlyList<State> StateHistory => stateHistory;

        public IReadOnlyList<IFaComponent> Components { get; }

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

        public FaStatusEnum Status
        {
            get => status;
            private set => Set(ref status, value);
        }

        public SingleStateExecutableFa(IReadOnlyList<IFaComponent> components, State initialState)
        {
            Components = components.Select(component => component.Copy()).ToList();
            State = initialState;
            Status = FaStatusEnum.Running;
            UpdateStatus();
        }

        public SingleStateExecutableFa(SingleStateExecutableFa other)
        {
            Components = other.Components.Select(component => component.Copy()).ToList();
            State = other.State;
            Status = other.Status;
            stateHistory = new List<State>(other.StateHistory);
        }

        public void TakeTransition(Transition transition)
        {
            State = transition.Target;
            Components
                .Zip(transition.Components)
                .ForEach(elm => elm.First.TakeTransition(elm.Second));
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            if (state.IsFinal && Components.All(component => component.IsReadyToTerminate))
            {
                Status = FaStatusEnum.Accepted;
            }
            else if (Components.Any(component => component.RequiresTermination))
            {
                Status = state.IsFinal ? FaStatusEnum.Accepted : FaStatusEnum.Rejected;
            }
        }

        public void GoToFailedState()
        {
            State = null;
            Status = FaStatusEnum.Rejected;
        }
    }
}
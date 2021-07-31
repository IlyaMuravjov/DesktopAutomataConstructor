using System.Collections.Generic;
using System.Linq;
using ControlsLibrary.Infrastructure.Events;
using GraphX.Common;

namespace ControlsLibrary.Model.Analyzers
{
    public abstract class BaseStateFinder
    {
        protected EditableFa Fa { get; }

        private readonly HashSet<State> matchingStates;
        public IReadOnlyCollection<State> MatchingStates => matchingStates;

        protected BaseStateFinder(EditableFa fa)
        {
            Fa = fa;
            matchingStates = Fa.States.Where(IsMatchingState).ToHashSet();
            fa.StateAdded += OnStateAdded;
            fa.StateRemoved += OnStateRemoved;
        }

        protected abstract bool IsMatchingState(State state);

        private void OnStateAdded(object sender, ElementAddedEventArgs<State> e)
        {
            if (IsMatchingState(e.NewElement))
            {
                matchingStates.Add(e.NewElement);
            }
        }

        private void OnStateRemoved(object sender, ElementRemovedEventArgs<State> e) =>
            matchingStates.Remove(e.OldElement);

        private void Reanalyze(State state)
        {
            if (IsMatchingState(state))
            {
                matchingStates.Add(state);
            }
            else
            {
                matchingStates.Remove(state);
            }
        }

        protected void ReanalyzeOnTransitionFilterModified()
        {
            void SubscribeToState(State state)
            {
                Fa.Transitions[state].TransitionFilterModified += (sender, e) => Reanalyze(state);
            }

            Fa.StateAdded += (sender, e) => SubscribeToState(e.NewElement);
            Fa.States.ForEach(SubscribeToState);
        }

        protected void ReanalyzeOnPropertyChanged(string propertyName)
        {
            void SubscribeToState(State state)
            {
                state.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == propertyName)
                    {
                        Reanalyze(state);
                    }
                };
            }

            Fa.StateAdded += (sender, e) => SubscribeToState(e.NewElement);
            Fa.States.ForEach(SubscribeToState);
        }
    }
}
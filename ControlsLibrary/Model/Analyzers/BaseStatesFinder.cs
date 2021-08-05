using System.Collections.Generic;
using System.Linq;
using ControlsLibrary.Infrastructure.Events;
using GraphX.Common;

namespace ControlsLibrary.Model.Analyzers
{
    public abstract class BaseStateFinder
    {
        protected EditableAutomaton Automaton { get; }

        private readonly HashSet<State> matchingStates;
        public IReadOnlyCollection<State> MatchingStates => matchingStates;

        protected BaseStateFinder(EditableAutomaton automaton)
        {
            Automaton = automaton;
            matchingStates = Automaton.States.Where(IsMatchingState).ToHashSet();
            automaton.StateAdded += OnStateAdded;
            automaton.StateRemoved += OnStateRemoved;
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
                Automaton.Transitions[state].TransitionFilterModified += (sender, e) => Reanalyze(state);
            }

            Automaton.StateAdded += (sender, e) => SubscribeToState(e.NewElement);
            Automaton.States.ForEach(SubscribeToState);
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

            Automaton.StateAdded += (sender, e) => SubscribeToState(e.NewElement);
            Automaton.States.ForEach(SubscribeToState);
        }
    }
}
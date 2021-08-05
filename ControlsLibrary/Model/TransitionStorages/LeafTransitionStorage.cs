using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ControlsLibrary.Infrastructure;

namespace ControlsLibrary.Model.TransitionStorages
{
    public class LeafTransitionStorage : ITransitionStorage
    {
        private readonly HashSet<Transition> transitions = new HashSet<Transition>();

        public bool IsDeterministic => transitions.Count <= 1;
        public bool IsEmpty => transitions.Count == 0;
        public IReadOnlyCollection<Transition> Transitions => transitions;

        public IReadOnlyCollection<Transition> EpsilonTransitions => transitions
            .Where(transition => transition.SideEffects.Flatten().All(sideEffect => sideEffect.Value == null))
            .ToHashSet();

        public event EventHandler<EventArgs> BecomeEmpty;

        public void AddTransition(Transition transition, IReadOnlyList<TransitionProperty> filterProperties, int propertyIndex)
        {
            Debug.Assert(filterProperties.Count == propertyIndex);
            transitions.Add(transition);
        }

        public void RemoveTransition(Transition transition, IReadOnlyList<TransitionProperty> filterProperties, int propertyIndex)
        {
            Debug.Assert(filterProperties.Count == propertyIndex);
            transitions.Remove(transition);
            if (IsEmpty)
            {
                BecomeEmpty?.Invoke(this, new EventArgs());
            }
        }

        public IReadOnlyCollection<Transition> GetPossibleTransitions(IReadOnlyList<object> filterValues, int propertyIndex)
        {
            Debug.Assert(filterValues.Count == propertyIndex);
            return transitions;
        }
    }
}
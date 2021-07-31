using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ControlsLibrary.Infrastructure;

namespace ControlsLibrary.Model.TransitionCollections
{
    public class LeafTransitionStorage : ITransitionStorage
    {
        private readonly HashSet<Transition> transitions = new HashSet<Transition>();

        public bool IsDeterministic => transitions.Count <= 1;
        public bool IsEmpty => transitions.Count == 0;
        public IReadOnlyCollection<Transition> Transitions => transitions;

        public IReadOnlyCollection<Transition> EpsilonTransitions => transitions
            .Where(
                transition => transition.Components.All(component =>
                    component.SideEffect.Properties.Flatten().All(prop => prop.Value == null)
                )
            ).ToHashSet();

        public event EventHandler<EventArgs> BecomeEmpty;

        public void AddTransition(Transition transition, IReadOnlyList<Property> filterProperties, int propertyIndex)
        {
            AssertAllFilterPropertiesHandled(filterProperties, propertyIndex);
            transitions.Add(transition);
        }

        public void RemoveTransition(Transition transition, IReadOnlyList<Property> filterProperties, int propertyIndex)
        {
            AssertAllFilterPropertiesHandled(filterProperties, propertyIndex);
            transitions.Remove(transition);
            if (IsEmpty)
            {
                BecomeEmpty?.Invoke(this, new EventArgs());
            }
        }

        public IReadOnlyCollection<Transition> GetPossibleTransitions(IReadOnlyList<Property> filterProperties, int propertyIndex)
        {
            AssertAllFilterPropertiesHandled(filterProperties, propertyIndex);
            return transitions;
        }

        public IReadOnlyCollection<Transition> GetTransitionsWithExactFilters(IReadOnlyList<Property> filterProperties, int propertyIndex)
        {
            AssertAllFilterPropertiesHandled(filterProperties, propertyIndex);
            return transitions;
        }

        private static void AssertAllFilterPropertiesHandled(IReadOnlyList<Property> filterProperties, int propertyIndex) =>
            Debug.Assert(filterProperties.Count == propertyIndex);
    }
}
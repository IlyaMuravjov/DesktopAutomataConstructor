using System;
using System.Collections.Generic;
using ControlsLibrary.Infrastructure;

namespace ControlsLibrary.Model.TransitionCollections
{
    public interface ITransitionStorage
    {
        public bool IsDeterministic { get; }
        public bool IsEmpty { get; }

        public IReadOnlyCollection<Transition> Transitions { get; }

        public IReadOnlyCollection<Transition> EpsilonTransitions { get; }

        public event EventHandler<EventArgs> BecomeEmpty;

        public void AddTransition(Transition transition, IReadOnlyList<Property> filterProperties, int propertyIndex);

        public void RemoveTransition(Transition transition, IReadOnlyList<Property> filterProperties, int propertyIndex);

        public IReadOnlyCollection<Transition> GetPossibleTransitions(IReadOnlyList<Property> filterProperties, int propertyIndex);

        public IReadOnlyCollection<Transition> GetTransitionsWithExactFilters(IReadOnlyList<Property> filterProperties, int propertyIndex);
    }
}
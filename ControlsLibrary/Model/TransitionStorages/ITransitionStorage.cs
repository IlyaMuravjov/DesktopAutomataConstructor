using System;
using System.Collections.Generic;

namespace ControlsLibrary.Model.TransitionStorages
{
    public interface ITransitionStorage
    {
        public bool IsDeterministic { get; }
        public bool IsEmpty { get; }

        public IReadOnlyCollection<Transition> Transitions { get; }

        public IReadOnlyCollection<Transition> EpsilonTransitions { get; }

        public event EventHandler<EventArgs> BecomeEmpty;

        public void AddTransition(Transition transition, IReadOnlyList<TransitionProperty> filterProperties, int propertyIndex);

        public void RemoveTransition(Transition transition, IReadOnlyList<TransitionProperty> filterProperties, int propertyIndex);

        public IReadOnlyCollection<Transition> GetPossibleTransitions(IReadOnlyList<object> filterValues, int propertyIndex);
    }
}
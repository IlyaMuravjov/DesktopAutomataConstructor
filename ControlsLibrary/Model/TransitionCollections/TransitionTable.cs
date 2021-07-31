using System;
using System.Collections.Generic;
using System.Linq;
using ControlsLibrary.Infrastructure;

namespace ControlsLibrary.Model.TransitionCollections
{
    public class TransitionTable
    {
        private readonly ITransitionStorage transitionStorage;
        public event EventHandler<EventArgs> TransitionFilterModified;

        public TransitionTable(IReadOnlyList<IFaComponent> components)
        {
            transitionStorage =
                FlattenFilterProperties(components.Select(component => component.DefaultFilter))
                    .Aggregate(
                        (Func<ITransitionStorage>) (() => new LeafTransitionStorage()),
                        (storageFactory, expectedValue) => () => new BranchTransitionStorage(storageFactory)
                    )
                    .Invoke();
        }

        public bool IsDeterministic => transitionStorage.IsDeterministic;

        // TODO decide which transitions are considered "epsilon"
        // 1. Is it enough to have one epsilon side effect?
        // 2. Is it enough to have one epsilon side filter?
        // 3. Should all filters and side effects be epsilon? (consistent with EpsilonTransitions)

        // public bool HasEpsilon { get; }

        public IReadOnlyCollection<Transition> Transitions => transitionStorage.Transitions;

        // transitions that have no other side effects besides changing current state
        // (in other words, all filters and side effects of these transitions have null value)
        public IReadOnlyCollection<Transition> EpsilonTransitions => transitionStorage.EpsilonTransitions;

        public void AddTransition(Transition transition)
        {
            DoAddTransition(transition);
            transition.FilterModifying += (sender, e) => DoRemoveTransition(transition);
            transition.FilterModified += (sender, e) =>
            {
                DoAddTransition(transition);
                TransitionFilterModified?.Invoke(this, e);
            };
            TransitionFilterModified?.Invoke(this, new EventArgs());
        }

        private void DoAddTransition(Transition transition)
        {
            transitionStorage.AddTransition(transition, FlattenFilterProperties(transition), 0);
        }

        // TODO maybe return bool
        public void RemoveTransition(Transition transition)
        {
            DoRemoveTransition(transition);
            TransitionFilterModified?.Invoke(this, new EventArgs());
        }

        private void DoRemoveTransition(Transition transition)
        {
            transitionStorage.RemoveTransition(transition, FlattenFilterProperties(transition), 0);
        }

        // if we ask for (1,0,0) then (1, null (epsilon), 0) will suffice
        public IReadOnlyCollection<Transition> GetPossibleTransitions(IReadOnlyList<ITransitionFilter> filters) =>
            transitionStorage.GetPossibleTransitions(FlattenFilterProperties(filters), 0);

        // if we ask for (1,0,0) then (1, null (epsilon), 0) won't suffice
        public IReadOnlyCollection<Transition> GetTransitionsWithExactFilters(IReadOnlyList<ITransitionFilter> filters) =>
            transitionStorage.GetTransitionsWithExactFilters(FlattenFilterProperties(filters), 0);

        private static List<Property> FlattenFilterProperties(Transition transition) =>
            FlattenFilterProperties(transition.Components.Select(component => component.Filter));

        private static List<Property> FlattenFilterProperties(IEnumerable<ITransitionFilter> filters) =>
            filters.SelectMany(filter => filter.Properties.Flatten()).ToList();
    }
}
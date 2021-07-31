using System;
using System.Collections.Generic;
using System.Linq;
using ControlsLibrary.Infrastructure;

namespace ControlsLibrary.Model.TransitionCollections
{
    public class BranchTransitionStorage : ITransitionStorage
    {
        private readonly Func<ITransitionStorage> subStorageFactory;
        private readonly ITransitionStorage epsilonSubStorage;
        private readonly Dictionary<object, ITransitionStorage> subStorages = new Dictionary<object, ITransitionStorage>();

        public BranchTransitionStorage(Func<ITransitionStorage> subStorageFactory)
        {
            this.subStorageFactory = subStorageFactory;
            epsilonSubStorage = subStorageFactory();
        }

        public bool IsDeterministic =>
            epsilonSubStorage.Transitions.Count == 0 && subStorages.Values.All(subStorage => subStorage.IsDeterministic);

        public bool IsEmpty => subStorages.Count == 0 && epsilonSubStorage.IsEmpty;

        public IReadOnlyCollection<Transition> Transitions =>
            subStorages.Values.SelectMany(subStorage => subStorage.Transitions).Concat(epsilonSubStorage.Transitions).ToHashSet();

        public IReadOnlyCollection<Transition> EpsilonTransitions => epsilonSubStorage.EpsilonTransitions;

        public event EventHandler<EventArgs> BecomeEmpty;

        public void AddTransition(Transition transition, IReadOnlyList<Property> filterProperties, int propertyIndex) =>
            GetOrCreateSubStorage(filterProperties[propertyIndex]).AddTransition(transition, filterProperties, propertyIndex + 1);

        public void RemoveTransition(Transition transition, IReadOnlyList<Property> filterProperties, int propertyIndex)
        {
            GetSubStorage(filterProperties[propertyIndex])?.RemoveTransition(transition, filterProperties, propertyIndex + 1);
            if (IsEmpty)
            {
                BecomeEmpty?.Invoke(this, new EventArgs());
            }
        }

        public IReadOnlyCollection<Transition> GetPossibleTransitions(IReadOnlyList<Property> filterProperties, int propertyIndex)
        {
            var epsilonTransitions = epsilonSubStorage.GetPossibleTransitions(filterProperties, propertyIndex + 1);
            var subStorage = GetSubStorage(filterProperties[propertyIndex]);
            return subStorage == null
                ? epsilonTransitions
                : epsilonTransitions.Concat(subStorage.GetPossibleTransitions(filterProperties, propertyIndex + 1)).ToHashSet();
        }


        public IReadOnlyCollection<Transition> GetTransitionsWithExactFilters(IReadOnlyList<Property> filterProperties, int propertyIndex) =>
            GetSubStorage(filterProperties[propertyIndex])?.GetTransitionsWithExactFilters(filterProperties, propertyIndex + 1) ?? new HashSet<Transition>();

        private ITransitionStorage GetSubStorage(Property filterProperty) =>
            filterProperty.Value == null ? epsilonSubStorage :
            subStorages.ContainsKey(filterProperty.Value) ? subStorages[filterProperty.Value] :
            null;

        private ITransitionStorage GetOrCreateSubStorage(Property filterProperty)
        {
            var subStorage = GetSubStorage(filterProperty);
            if (subStorage == null)
            {
                subStorage = subStorageFactory();
                subStorages[filterProperty.Value] = subStorage;
                subStorage.BecomeEmpty += (sender, e) => subStorages.Remove(filterProperty.Value);
            }

            return subStorage;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using ControlsLibrary.Model.TransitionProperty;

namespace ControlsLibrary.Model.TransitionStorages
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

        public void AddTransition(Transition transition, IReadOnlyList<ITransitionProperty> filterProperties, int propertyIndex) =>
            GetOrCreateSubStorage(filterProperties[propertyIndex].Value).AddTransition(transition, filterProperties, propertyIndex + 1);

        public void RemoveTransition(Transition transition, IReadOnlyList<ITransitionProperty> filterProperties, int propertyIndex)
        {
            GetSubStorage(filterProperties[propertyIndex].Value)?.RemoveTransition(transition, filterProperties, propertyIndex + 1);
            if (IsEmpty)
            {
                BecomeEmpty?.Invoke(this, new EventArgs());
            }
        }

        public IReadOnlyCollection<Transition> GetPossibleTransitions(IReadOnlyList<object> filterValues, int propertyIndex)
        {
            var epsilonTransitions = epsilonSubStorage.GetPossibleTransitions(filterValues, propertyIndex + 1);
            var subStorage = GetSubStorage(filterValues[propertyIndex]);
            return subStorage == null
                ? epsilonTransitions
                : epsilonTransitions.Concat(subStorage.GetPossibleTransitions(filterValues, propertyIndex + 1)).ToHashSet();
        }

        private ITransitionStorage GetSubStorage(object filterValue) =>
            filterValue == null ? epsilonSubStorage :
            subStorages.ContainsKey(filterValue) ? subStorages[filterValue] :
            null;

        private ITransitionStorage GetOrCreateSubStorage(object filterValue)
        {
            var subStorage = GetSubStorage(filterValue);
            if (subStorage == null)
            {
                subStorage = subStorageFactory();
                subStorages[filterValue] = subStorage;
                subStorage.BecomeEmpty += (sender, e) => subStorages.Remove(filterValue);
            }

            return subStorage;
        }
    }
}
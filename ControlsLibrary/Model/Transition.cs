using System;
using System.Collections.Generic;
using System.Linq;
using ControlsLibrary.Infrastructure;
using ControlsLibrary.Model.TransitionProperty.Generic;
using ControlsLibrary.Model.TransitionProperty;
using GraphX.Common;

namespace ControlsLibrary.Model
{
    public class Transition
    {
        public State Source { get; }
        public State Target { get; }

        // if performance boost needed here, indices can be stored in TransitionPropertyDescriptor and used to retrieve property from lists instead of dictionary
        private readonly Dictionary<ITransitionPropertyDescriptor, ITransitionProperty> properties = new Dictionary<ITransitionPropertyDescriptor, ITransitionProperty>();
        public IReadOnlyList<IReadOnlyList<ITransitionProperty>> Filters { get; }
        public IReadOnlyList<IReadOnlyList<ITransitionProperty>> SideEffects { get; }

        public Transition(
            State source,
            State target,
            IReadOnlyList<IReadOnlyList<ITransitionPropertyDescriptor>> filters,
            IReadOnlyList<IReadOnlyList<ITransitionPropertyDescriptor>> sideEffects
        )
        {
            Source = source;
            Target = target;
            Filters = filters.Select(descriptorList => descriptorList.Select(CreateProperty).ToList()).ToList();
            SideEffects = sideEffects.Select(descriptorList => descriptorList.Select(CreateProperty).ToList()).ToList();
            Filters.Flatten().ForEach(filter =>
            {
                filter.ValueChanging += (sender, e) => FilterChanging?.Invoke(this, e);
                filter.ValueChanged += (sender, e) => FilterChanged?.Invoke(this, e);
            });
            SideEffects.Flatten().ForEach(sideEffect =>
                sideEffect.ValueChanged += (sender, e) => SideEffectChanged?.Invoke(this, e)
            );
        }

        public event EventHandler<EventArgs> FilterChanging;
        public event EventHandler<EventArgs> FilterChanged;

        public event EventHandler<EventArgs> SideEffectChanged;

        public ITransitionProperty<T> GetProperty<T>(ITransitionPropertyDescriptor<T> descriptor) =>
            (ITransitionProperty<T>) properties[descriptor];

        public T Get<T>(ITransitionPropertyDescriptor<T> descriptor) => GetProperty(descriptor).Value;
        public void Set<T>(ITransitionPropertyDescriptor<T> descriptor, T value) => GetProperty(descriptor).Value = value;

        private ITransitionProperty CreateProperty(ITransitionPropertyDescriptor descriptor)
        {
            var property = descriptor.CreateProperty();
            properties[descriptor] = property;
            return property;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using ControlsLibrary.Infrastructure;
using GraphX.Common;

namespace ControlsLibrary.Model
{
    public class Transition
    {
        public State Source { get; }
        public State Target { get; }

        // if performance boost needed here, indices can be stored in TransitionPropertyDescriptor and used to retrieve property from lists instead of dictionary
        private readonly Dictionary<TransitionPropertyDescriptor, TransitionProperty> properties = new Dictionary<TransitionPropertyDescriptor, TransitionProperty>();
        public IReadOnlyList<IReadOnlyList<TransitionProperty>> Filters { get; }
        public IReadOnlyList<IReadOnlyList<TransitionProperty>> SideEffects { get; }

        public Transition(
            State source,
            State target,
            IReadOnlyList<IReadOnlyList<TransitionPropertyDescriptor>> filters,
            IReadOnlyList<IReadOnlyList<TransitionPropertyDescriptor>> sideEffects
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

        public object this[TransitionPropertyDescriptor descriptor]
        {
            get => properties[descriptor].Value;
            set => properties[descriptor].Value = value;
        }

        private TransitionProperty CreateProperty(TransitionPropertyDescriptor descriptor)
        {
            var property = new TransitionProperty(descriptor);
            properties[descriptor] = property;
            return property;
        }
    }
}
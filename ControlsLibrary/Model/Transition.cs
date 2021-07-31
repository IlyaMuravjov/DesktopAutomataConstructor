using System;
using System.Collections.Generic;

namespace ControlsLibrary.Model
{
    public class Transition
    {
        public State Source { get; }
        public State Target { get; }
        public IReadOnlyList<TransitionComponent> Components { get; }

        public Transition(State source, State target, IReadOnlyList<TransitionComponent> components)
        {
            Source = source;
            Target = target;
            Components = components;
            foreach (var component in components)
            {
                component.FilterModifying += (sender, e) => FilterModifying?.Invoke(this, e);
                component.FilterModified += (sender, e) => FilterModified?.Invoke(this, e);

                component.SideEffectModified += (sender, e) => SideEffectModified?.Invoke(this, e);
            }
        }

        public event EventHandler<EventArgs> FilterModifying;
        public event EventHandler<EventArgs> FilterModified;

        public event EventHandler<EventArgs> SideEffectModified;
    }
}
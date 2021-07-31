using System;
using ControlsLibrary.Infrastructure;

namespace ControlsLibrary.Model
{
    public class TransitionComponent
    {
        public ITransitionFilter Filter { get; }
        public ITransitionSideEffect SideEffect { get; }

        public event EventHandler<EventArgs> FilterModifying;
        public event EventHandler<EventArgs> FilterModified;

        public event EventHandler<EventArgs> SideEffectModified;

        public TransitionComponent(ITransitionFilter filter, ITransitionSideEffect sideEffect)
        {
            Filter = filter;
            SideEffect = sideEffect;
            foreach (var property in Filter.Properties.Flatten())
            {
                property.ValueChanging += (sender, e) => FilterModifying?.Invoke(this, e);
                property.ValueChanged += (sender, e) => FilterModified?.Invoke(this, e);
            }

            foreach (var property in SideEffect.Properties.Flatten())
            {
                property.ValueChanged += (sender, e) => SideEffectModified?.Invoke(this, e);
            }
        }
    }
}
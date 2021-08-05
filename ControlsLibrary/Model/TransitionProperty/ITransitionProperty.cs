using System;

namespace ControlsLibrary.Model.TransitionProperty
{
    public interface ITransitionProperty
    {
        public ITransitionPropertyDescriptor Descriptor { get; }
        public object Value { get; }
        public event EventHandler<EventArgs> ValueChanging;
        public event EventHandler<EventArgs> ValueChanged;
    }
}
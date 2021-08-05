using System;
using ControlsLibrary.Infrastructure;
using ControlsLibrary.Model.TransitionProperty.Generic;

namespace ControlsLibrary.Model.TransitionProperty.Impl
{
    public class TransitionProperty<T> : BaseNotifyPropertyChanged, ITransitionProperty<T>
    {
        public ITransitionPropertyDescriptor<T> Descriptor { get; }
        ITransitionPropertyDescriptor ITransitionProperty.Descriptor => Descriptor;

        private T value;
        
        public T Value
        {
            get => value;
            set
            {
                ValueChanging?.Invoke(this, new EventArgs());
                Set(ref this.value, value);
                ValueChanged?.Invoke(this, new EventArgs());
            }
        }

        object ITransitionProperty.Value => Value;

        public event EventHandler<EventArgs> ValueChanging;
        public event EventHandler<EventArgs> ValueChanged;

        public TransitionProperty(ITransitionPropertyDescriptor<T> descriptor, T value)
        {
            Descriptor = descriptor;
            Value = value;
        }
    }
}
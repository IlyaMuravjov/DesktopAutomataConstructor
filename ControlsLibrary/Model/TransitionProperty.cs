using System;
using ControlsLibrary.Infrastructure;

namespace ControlsLibrary.Model
{
    public class TransitionProperty : BaseNotifyPropertyChanged
    {
        public TransitionPropertyDescriptor Descriptor { get; }
        private object value;

        public object Value
        {
            get => value;
            set
            {
                ValueChanging?.Invoke(this, new EventArgs());
                Set(ref this.value, value);
                ValueChanged?.Invoke(this, new EventArgs());
            }
        }

        public event EventHandler<EventArgs> ValueChanging;
        public event EventHandler<EventArgs> ValueChanged;

        public TransitionProperty(TransitionPropertyDescriptor descriptor)
        {
            Descriptor = descriptor;
            Value = descriptor.DefaultValue;
        }
    }
}
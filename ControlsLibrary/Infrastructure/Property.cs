using System;

namespace ControlsLibrary.Infrastructure
{
    public class Property : BaseNotifyPropertyChanged
    {
        public Type Type { get; }
        private object value = null;

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

        public Property(Type type)
        {
            Type = type;
        }
    }
}
using System;

namespace ControlsLibrary.Model
{
    public class TransitionPropertyDescriptor
    {
        public Type Type { get; }
        public object DefaultValue { get; }

        public TransitionPropertyDescriptor(Type type, object defaultValue)
        {
            Type = type;
            DefaultValue = defaultValue;
        }
    }
}
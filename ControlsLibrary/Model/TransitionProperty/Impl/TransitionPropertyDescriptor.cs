using System;
using ControlsLibrary.Model.TransitionProperty.Generic;

namespace ControlsLibrary.Model.TransitionProperty.Impl
{
    public class TransitionPropertyDescriptor<T> : ITransitionPropertyDescriptor<T>
    {
        public Type Type { get; }

        private readonly T defaultValue;

        public TransitionPropertyDescriptor(T defaultValue)
        {
            Type = typeof(T);
            this.defaultValue = defaultValue;
        }

        public ITransitionProperty<T> CreateProperty() => new TransitionProperty<T>(this, defaultValue);

        ITransitionProperty ITransitionPropertyDescriptor.CreateProperty() => CreateProperty();
    }
}
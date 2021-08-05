using System;

namespace ControlsLibrary.Model.TransitionProperty
{
    public interface ITransitionPropertyDescriptor
    {
        public Type Type { get; }
        public ITransitionProperty CreateProperty();
    }
}
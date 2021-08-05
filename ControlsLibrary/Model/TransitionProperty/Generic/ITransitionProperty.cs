namespace ControlsLibrary.Model.TransitionProperty.Generic
{
    public interface ITransitionProperty<T> : ITransitionProperty
    {
        public new T Value { get; set; }
        public new ITransitionPropertyDescriptor<T> Descriptor { get; }
    }
}
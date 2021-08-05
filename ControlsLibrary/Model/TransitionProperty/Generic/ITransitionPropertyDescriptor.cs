namespace ControlsLibrary.Model.TransitionProperty.Generic
{
    public interface ITransitionPropertyDescriptor<T> : ITransitionPropertyDescriptor
    {
        public new ITransitionProperty<T> CreateProperty();
    }
}
using System.Collections.Generic;
using ControlsLibrary.Model.TransitionProperty;

namespace ControlsLibrary.Model
{
    public interface IAutomatonMemory
    {
        public IReadOnlyList<IReadOnlyList<ITransitionPropertyDescriptor>> FilterDescriptors { get; }
        public IReadOnlyList<IReadOnlyList<ITransitionPropertyDescriptor>> SideEffectDescriptors { get; }
        public IReadOnlyList<IReadOnlyList<object>> CurrentFilters { get; }
        public bool IsReadyToTerminate { get; }
        public bool RequiresTermination { get; }
        public void TakeTransition(Transition transition);
        public IAutomatonMemory Copy();
    }
}
using System.Collections.Generic;

namespace ControlsLibrary.Model
{
    public interface IFaComponent
    {
        public IReadOnlyList<IReadOnlyList<TransitionPropertyDescriptor>> FilterDescriptors { get; }
        public IReadOnlyList<IReadOnlyList<TransitionPropertyDescriptor>> SideEffectDescriptors { get; }
        public IReadOnlyList<IReadOnlyList<object>> CurrentFilters { get; }
        public bool IsReadyToTerminate { get; }
        public bool RequiresTermination { get; }
        public void TakeTransition(Transition transition);
        public IFaComponent Copy();
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ControlsLibrary.Infrastructure;
using ControlsLibrary.Model.TransitionProperty;
using ControlsLibrary.Model.TransitionProperty.Generic;
using ControlsLibrary.Model.TransitionProperty.Impl;

namespace ControlsLibrary.Model.Tapes
{
    public abstract class BaseTape : BaseNotifyPropertyChanged, IAutomatonComponent
    {
        private int position;
        public ObservableCollection<char> Data { get; }

        public int Position
        {
            get => position;
            set => Set(ref position, value);
        }

        public IReadOnlyList<IReadOnlyList<ITransitionPropertyDescriptor>> FilterDescriptors => new[] { new [] { ExpectedChar } };
        public abstract IReadOnlyList<IReadOnlyList<ITransitionPropertyDescriptor>> SideEffectDescriptors { get; }
        public IReadOnlyList<IReadOnlyList<object>> CurrentFilters => new[] {new object[] {Data[Position]}};
        public ITransitionPropertyDescriptor<char?> ExpectedChar { get; } = new TransitionPropertyDescriptor<char?>(null);
        public abstract bool IsReadyToTerminate { get; }
        public abstract bool RequiresTermination { get; }

        protected BaseTape()
        {
            Data = new ObservableCollection<char>();
            Position = 0;
        }

        protected BaseTape(BaseTape other)
        {
            Data = new ObservableCollection<char>(other.Data);
            Position = other.Position;
        }

        public abstract void TakeTransition(Transition transition);
        public abstract IAutomatonComponent Copy();
    }
}
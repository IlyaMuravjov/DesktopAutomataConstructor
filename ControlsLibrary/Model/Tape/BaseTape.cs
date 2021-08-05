using System.Collections.Generic;
using System.Collections.ObjectModel;
using ControlsLibrary.Infrastructure;

namespace ControlsLibrary.Model.Tape
{
    public abstract class BaseTape : BaseNotifyPropertyChanged, IFaComponent
    {
        private int position;
        public ObservableCollection<char> Data { get; }

        public int Position
        {
            get => position;
            set => Set(ref position, value);
        }

        public IReadOnlyList<IReadOnlyList<TransitionPropertyDescriptor>> FilterDescriptors => new[] { new [] { ExpectedChar } };
        public abstract IReadOnlyList<IReadOnlyList<TransitionPropertyDescriptor>> SideEffectDescriptors { get; }
        public IReadOnlyList<IReadOnlyList<object>> CurrentFilters => new[] {new object[] {Data[Position]}};
        public TransitionPropertyDescriptor ExpectedChar { get; } = new TransitionPropertyDescriptor(typeof(char?), null);
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
        public abstract IFaComponent Copy();
    }
}
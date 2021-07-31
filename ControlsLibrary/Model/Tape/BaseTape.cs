using System.Collections.ObjectModel;
using ControlsLibrary.Infrastructure;
using ControlsLibrary.Model.SimpleImpl;

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

        public ITransitionFilter DefaultFilter => new CharTransitionFilter();
        public abstract ITransitionSideEffect DefaultSideEffect { get; }
        public ITransitionFilter CurrentFilter => new CharTransitionFilter {ExpectedChar = Data[position]};
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

        public abstract void TakeTransition(TransitionComponent transition);
        public abstract IFaComponent Copy();
    }
}
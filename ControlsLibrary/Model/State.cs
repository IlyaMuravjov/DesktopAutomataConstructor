using ControlsLibrary.Infrastructure;

namespace ControlsLibrary.Model
{
    public class State : BaseNotifyPropertyChanged
    {
        private string name;
        private bool isInitial = false;
        private bool isFinal = false;
        private bool isCurrent = false;
        private long id;

        public string Name
        {
            get => name;
            set => Set(ref name, value);
        }

        public bool IsInitial
        {
            get => isInitial;
            set => Set(ref isInitial, value);
        }

        public bool IsFinal
        {
            get => isFinal;
            set => Set(ref isFinal, value);
        }

        public bool IsCurrent
        {
            get => isCurrent;
            set => Set(ref isCurrent, value);
        }

        public long ID
        {
            get => id;
            set => Set(ref id, value);
        }
    }
}
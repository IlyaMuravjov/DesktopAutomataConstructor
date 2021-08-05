using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ControlsLibrary.Infrastructure;
using ControlsLibrary.Model.Analyzers;
using GraphX.Common;

namespace ControlsLibrary.Model
{
    public class MultiStateExecutableFa : BaseNotifyPropertyChanged
    {
        private readonly EditableFa editableFa;
        public ObservableCollection<SingleStateExecutableFa> SingleStateFAs { get; }

        private FaStatusEnum status;

        public FaStatusEnum Status
        {
            get => status;
            private set => Set(ref status, value);
        }

        public MultiStateExecutableFa(EditableFa editableFa)
        {
            if (!editableFa.IsExecutable())
            {
                throw new InvalidOperationException(); // TODO error message
            }

            this.editableFa = editableFa;
            SingleStateFAs = new ObservableCollection<SingleStateExecutableFa>(
                editableFa.GetInitialStates()
                    .Select(state => new SingleStateExecutableFa(editableFa.Components, state))
            );
            UpdateStatus();
        }

        public void Run()
        {
            // TODO pause when too many forks are created or too many steps are taken
            while (Status == FaStatusEnum.Running)
            {
                TakeStep();
            }
        }

        public void TakeStep()
        {
            var forks = new List<SingleStateExecutableFa>();
            var runningFAs = SingleStateFAs.Where(fa => fa.Status == FaStatusEnum.Running).ToList();
            runningFAs.ForEach(fa => fa.State.IsCurrent = false);
            runningFAs.ForEach(fa =>
            {
                var transitions = editableFa.Transitions[fa.State].GetPossibleTransitions(fa.Components);
                if (transitions.Count == 0)
                {
                    fa.GoToFailedState();
                }
                else
                {
                    transitions
                        .Skip(1)
                        .ForEach(transition =>
                        {
                            var fork = new SingleStateExecutableFa(fa);
                            fork.TakeTransition(transition);
                            forks.Add(fork);
                        });
                    fa.TakeTransition(transitions.First());
                }
            });
            forks.ForEach(fork => SingleStateFAs.Add(fork));
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            Status = SingleStateFAs.Any(fa => fa.Status == FaStatusEnum.Accepted) ? FaStatusEnum.Accepted :
                SingleStateFAs.Any(fa => fa.Status == FaStatusEnum.Running) ? FaStatusEnum.Running :
                FaStatusEnum.Rejected;
        }
    }
}
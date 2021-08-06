using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ControlsLibrary.Infrastructure;
using GraphX.Common;

namespace ControlsLibrary.Model.Modules.Executor
{
    public class AutomatonExecutor : BaseNotifyPropertyChanged
    {
        private readonly Automaton automaton;
        public ObservableCollection<AutomatonComputationPath> ComputationPaths { get; }

        private ExecutionStatusEnum status;

        public ExecutionStatusEnum Status
        {
            get => status;
            private set => Set(ref status, value);
        }

        public AutomatonExecutor(Automaton automaton)
        {
            if (!automaton.IsExecutable())
            {
                throw new InvalidOperationException(); // TODO error message
            }

            this.automaton = automaton;
            ComputationPaths = new ObservableCollection<AutomatonComputationPath>(
                automaton.GetInitialStates()
                    .Select(state => new AutomatonComputationPath(automaton.MemoryList, state))
            );
            UpdateStatus();
        }

        public static AutomatonExecutor Create(Automaton automaton) => new AutomatonExecutor(automaton);

        public void Run()
        {
            // TODO pause when too many forks are created or too many steps are taken
            while (Status == ExecutionStatusEnum.Running)
            {
                TakeStep();
            }
        }

        public void TakeStep()
        {
            var forks = new List<AutomatonComputationPath>();
            var runningPaths = ComputationPaths.Where(path => path.Status == ExecutionStatusEnum.Running).ToList();
            runningPaths.ForEach(runningPath => runningPath.State.IsCurrent = false);
            runningPaths.ForEach(runningPath =>
            {
                var transitions = automaton.Transitions[runningPath.State].GetPossibleTransitions(runningPath.MemoryList);
                if (transitions.Count == 0)
                {
                    runningPath.GoToFailedState();
                }
                else
                {
                    transitions
                        .Skip(1)
                        .ForEach(transition =>
                        {
                            var fork = new AutomatonComputationPath(runningPath);
                            fork.TakeTransition(transition);
                            forks.Add(fork);
                        });
                    runningPath.TakeTransition(transitions.First());
                }
            });
            forks.ForEach(fork => ComputationPaths.Add(fork));
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            Status = ComputationPaths.Any(path => path.Status == ExecutionStatusEnum.Accepted) ? ExecutionStatusEnum.Accepted :
                ComputationPaths.Any(path => path.Status == ExecutionStatusEnum.Running) ? ExecutionStatusEnum.Running :
                ExecutionStatusEnum.Rejected;
        }
    }
}
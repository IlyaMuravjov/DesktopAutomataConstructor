using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ControlsLibrary.Infrastructure;
using ControlsLibrary.Model.Analyzers;
using GraphX.Common;

namespace ControlsLibrary.Model.Executable
{
    public class ExecutableAutomaton : BaseNotifyPropertyChanged
    {
        private readonly EditableAutomaton editableAutomaton;
        public ObservableCollection<AutomatonComputationalPath> ComputationalPaths { get; }

        private ExecutionStatusEnum status;

        public ExecutionStatusEnum Status
        {
            get => status;
            private set => Set(ref status, value);
        }

        public ExecutableAutomaton(EditableAutomaton editableAutomaton)
        {
            if (!editableAutomaton.IsExecutable())
            {
                throw new InvalidOperationException(); // TODO error message
            }

            this.editableAutomaton = editableAutomaton;
            ComputationalPaths = new ObservableCollection<AutomatonComputationalPath>(
                editableAutomaton.GetInitialStates()
                    .Select(state => new AutomatonComputationalPath(editableAutomaton.Components, state))
            );
            UpdateStatus();
        }

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
            var forks = new List<AutomatonComputationalPath>();
            var runningPaths = ComputationalPaths.Where(path => path.Status == ExecutionStatusEnum.Running).ToList();
            runningPaths.ForEach(runningPath => runningPath.State.IsCurrent = false);
            runningPaths.ForEach(runningPath =>
            {
                var transitions = editableAutomaton.Transitions[runningPath.State].GetPossibleTransitions(runningPath.Components);
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
                            var fork = new AutomatonComputationalPath(runningPath);
                            fork.TakeTransition(transition);
                            forks.Add(fork);
                        });
                    runningPath.TakeTransition(transitions.First());
                }
            });
            forks.ForEach(fork => ComputationalPaths.Add(fork));
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            Status = ComputationalPaths.Any(path => path.Status == ExecutionStatusEnum.Accepted) ? ExecutionStatusEnum.Accepted :
                ComputationalPaths.Any(path => path.Status == ExecutionStatusEnum.Running) ? ExecutionStatusEnum.Running :
                ExecutionStatusEnum.Rejected;
        }
    }
}
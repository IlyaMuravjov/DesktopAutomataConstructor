using System.Collections.Generic;
using ControlsLibrary.Model.Modules.Executor;

namespace ControlsLibrary.Model.Modules
{
    public static class AutomatonComponentUtils
    {
        public static bool IsExecutable(this Automaton automaton) =>
            automaton.GetInitialStates().Count > 0 && automaton.GetFinalStates().Count > 0;

        public static AutomatonExecutor GetExecutor(this Automaton automaton) =>
            automaton.GetModule(AutomatonExecutor.Create);

        public static IReadOnlyCollection<State> GetInitialStates(this Automaton automaton) =>
            automaton.GetModule(InitialStatesFinder.Create).MatchingStates;

        public static IReadOnlyCollection<State> GetFinalStates(this Automaton automaton) =>
            automaton.GetModule(FinalStatesFinder.Create).MatchingStates;

        public static IReadOnlyCollection<State> GetNonDeterministicStates(this Automaton automaton) =>
            automaton.GetModule(NondeterministicStatesFinder.Create).MatchingStates;
    }
}
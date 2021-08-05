using System.Collections.Generic;
using ControlsLibrary.Model.Components.Executor;

namespace ControlsLibrary.Model.Components
{
    public static class AutomatonComponentUtils
    {
        public static bool IsExecutable(this Automaton automaton) =>
            automaton.GetInitialStates().Count > 0 && automaton.GetFinalStates().Count > 0;

        public static AutomatonExecutor GetExecutor(this Automaton automaton) =>
            automaton.GetComponent(AutomatonExecutor.Create);

        public static IReadOnlyCollection<State> GetInitialStates(this Automaton automaton) =>
            automaton.GetComponent(InitialStatesFinder.Create).MatchingStates;

        public static IReadOnlyCollection<State> GetFinalStates(this Automaton automaton) =>
            automaton.GetComponent(FinalStatesFinder.Create).MatchingStates;

        public static IReadOnlyCollection<State> GetNonDeterministicStates(this Automaton automaton) =>
            automaton.GetComponent(NondeterministicStatesFinder.Create).MatchingStates;
    }
}
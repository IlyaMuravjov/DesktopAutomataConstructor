using System.Collections.Generic;

namespace ControlsLibrary.Model.Analyzers
{
    public static class AutomatonAnalysisUtils
    {
        public static bool IsExecutable(this EditableAutomaton automaton) =>
            automaton.GetInitialStates().Count > 0 && automaton.GetFinalStates().Count > 0;

        public static IReadOnlyCollection<State> GetInitialStates(this EditableAutomaton automaton) =>
            automaton.GetAnalyzer(InitialStatesFinder.Create).MatchingStates;

        public static IReadOnlyCollection<State> GetFinalStates(this EditableAutomaton automaton) =>
            automaton.GetAnalyzer(FinalStatesFinder.Create).MatchingStates;

        public static IReadOnlyCollection<State> GetNonDeterministicStates(this EditableAutomaton automaton) =>
            automaton.GetAnalyzer(NondeterministicStatesFinder.Create).MatchingStates;
    }
}
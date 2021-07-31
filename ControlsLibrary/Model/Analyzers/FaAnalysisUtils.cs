using System.Collections.Generic;

namespace ControlsLibrary.Model.Analyzers
{
    public static class FaAnalysisUtils
    {
        public static bool IsExecutable(this EditableFa fa) =>
            fa.GetInitialStates().Count > 0 && fa.GetFinalStates().Count > 0;

        public static IReadOnlyCollection<State> GetInitialStates(this EditableFa fa) =>
            fa.GetAnalyzer(InitialStatesFinder.Create).MatchingStates;

        public static IReadOnlyCollection<State> GetFinalStates(this EditableFa fa) =>
            fa.GetAnalyzer(FinalStatesFinder.Create).MatchingStates;

        public static IReadOnlyCollection<State> GetNonDeterministicStates(this EditableFa fa) =>
            fa.GetAnalyzer(NondeterministicStatesFinder.Create).MatchingStates;
    }
}
namespace ControlsLibrary.Model.Analyzers
{
    public class FinalStatesFinder : BaseStateFinder
    {
        public FinalStatesFinder(EditableAutomaton automaton) : base(automaton)
        {
            ReanalyzeOnPropertyChanged(nameof(State.IsFinal));
        }

        protected override bool IsMatchingState(State state) => state.IsFinal;

        public static FinalStatesFinder Create(EditableAutomaton automaton) => new FinalStatesFinder(automaton);
    }
}
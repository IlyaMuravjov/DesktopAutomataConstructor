namespace ControlsLibrary.Model.Analyzers
{
    public class InitialStatesFinder : BaseStateFinder
    {
        public InitialStatesFinder(EditableAutomaton automaton) : base(automaton)
        {
            ReanalyzeOnPropertyChanged(nameof(State.IsInitial));
        }

        protected override bool IsMatchingState(State state) => state.IsInitial;

        public static InitialStatesFinder Create(EditableAutomaton automaton) => new InitialStatesFinder(automaton);
    }
}
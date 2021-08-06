namespace ControlsLibrary.Model.Modules
{
    public class InitialStatesFinder : BaseStateFinder
    {
        public InitialStatesFinder(Automaton automaton) : base(automaton)
        {
            ReanalyzeOnPropertyChanged(nameof(State.IsInitial));
        }

        protected override bool IsMatchingState(State state) => state.IsInitial;

        public static InitialStatesFinder Create(Automaton automaton) => new InitialStatesFinder(automaton);
    }
}
namespace ControlsLibrary.Model.Modules
{
    public class FinalStatesFinder : BaseStateFinder
    {
        public FinalStatesFinder(Automaton automaton) : base(automaton)
        {
            ReanalyzeOnPropertyChanged(nameof(State.IsFinal));
        }

        protected override bool IsMatchingState(State state) => state.IsFinal;

        public static FinalStatesFinder Create(Automaton automaton) => new FinalStatesFinder(automaton);
    }
}
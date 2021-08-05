namespace ControlsLibrary.Model.Components
{
    public class NondeterministicStatesFinder : BaseStateFinder
    {
        public NondeterministicStatesFinder(Automaton automaton) : base(automaton)
        {
            ReanalyzeOnTransitionFilterModified();
        }

        protected override bool IsMatchingState(State state) => !Automaton.Transitions[state].IsDeterministic;

        public static NondeterministicStatesFinder Create(Automaton automaton) => new NondeterministicStatesFinder(automaton);
    }
}
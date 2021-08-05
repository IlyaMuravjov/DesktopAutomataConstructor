namespace ControlsLibrary.Model.Analyzers
{
    public class NondeterministicStatesFinder : BaseStateFinder
    {
        public NondeterministicStatesFinder(EditableAutomaton automaton) : base(automaton)
        {
            ReanalyzeOnTransitionFilterModified();
        }

        protected override bool IsMatchingState(State state) => !Automaton.Transitions[state].IsDeterministic;

        public static NondeterministicStatesFinder Create(EditableAutomaton automaton) => new NondeterministicStatesFinder(automaton);
    }
}
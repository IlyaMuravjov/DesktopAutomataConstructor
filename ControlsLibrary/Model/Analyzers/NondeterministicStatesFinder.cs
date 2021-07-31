namespace ControlsLibrary.Model.Analyzers
{
    public class NondeterministicStatesFinder : BaseStateFinder
    {
        public NondeterministicStatesFinder(EditableFa fa) : base(fa)
        {
            ReanalyzeOnTransitionFilterModified();
        }

        protected override bool IsMatchingState(State state) => !Fa.Transitions[state].IsDeterministic;

        public static NondeterministicStatesFinder Create(EditableFa fa) => new NondeterministicStatesFinder(fa);
    }
}
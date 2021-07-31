namespace ControlsLibrary.Model.Analyzers
{
    public class FinalStatesFinder : BaseStateFinder
    {
        public FinalStatesFinder(EditableFa fa) : base(fa)
        {
            ReanalyzeOnPropertyChanged(nameof(State.IsFinal));
        }

        protected override bool IsMatchingState(State state) => state.IsFinal;

        public static FinalStatesFinder Create(EditableFa fa) => new FinalStatesFinder(fa);
    }
}
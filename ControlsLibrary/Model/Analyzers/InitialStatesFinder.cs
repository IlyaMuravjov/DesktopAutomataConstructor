namespace ControlsLibrary.Model.Analyzers
{
    public class InitialStatesFinder : BaseStateFinder
    {
        public InitialStatesFinder(EditableFa fa) : base(fa)
        {
            ReanalyzeOnPropertyChanged(nameof(State.IsInitial));
        }

        protected override bool IsMatchingState(State state) => state.IsInitial;

        public static InitialStatesFinder Create(EditableFa fa) => new InitialStatesFinder(fa);
    }
}
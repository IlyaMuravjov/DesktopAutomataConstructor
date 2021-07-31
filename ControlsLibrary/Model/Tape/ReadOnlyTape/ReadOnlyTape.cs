using ControlsLibrary.Model.SimpleImpl;

namespace ControlsLibrary.Model.Tape.ReadOnlyTape
{
    public class ReadOnlyTape : BaseTape
    {
        private bool EndReached => Data.Count == Position;
        public override bool IsReadyToTerminate => EndReached;
        public override bool RequiresTermination => EndReached;
        public override ITransitionSideEffect DefaultSideEffect => EmptyTransitionSideEffect.Instance;

        public ReadOnlyTape()
        {
        }

        public ReadOnlyTape(BaseTape other) : base(other)
        {
        }

        public override void TakeTransition(TransitionComponent transition)
        {
            if (((CharTransitionFilter) transition.Filter).ExpectedChar != null)
            {
                Position++;
            }
        }

        public override IFaComponent Copy() => new ReadOnlyTape(this);
    }
}
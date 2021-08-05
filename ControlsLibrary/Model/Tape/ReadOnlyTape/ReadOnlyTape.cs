using System.Collections.Generic;

namespace ControlsLibrary.Model.Tape.ReadOnlyTape
{
    public class ReadOnlyTape : BaseTape
    {
        private bool EndReached => Data.Count == Position;
        public override bool IsReadyToTerminate => EndReached;
        public override bool RequiresTermination => EndReached;
        public override IReadOnlyList<IReadOnlyList<TransitionPropertyDescriptor>> SideEffectDescriptors => new TransitionPropertyDescriptor[][] { };

        public ReadOnlyTape()
        {
        }

        public ReadOnlyTape(BaseTape other) : base(other)
        {
        }

        public override void TakeTransition(Transition transition)
        {
            if (transition[ExpectedChar] != null)
            {
                Position++;
            }
        }

        public override IFaComponent Copy() => new ReadOnlyTape(this);
    }
}
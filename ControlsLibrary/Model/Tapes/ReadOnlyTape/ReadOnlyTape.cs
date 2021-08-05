using System.Collections.Generic;
using ControlsLibrary.Model.TransitionProperty;

namespace ControlsLibrary.Model.Tapes.ReadOnlyTape
{
    public class ReadOnlyTape : BaseTape
    {
        private bool EndReached => Data.Count == Position;
        public override bool IsReadyToTerminate => EndReached;
        public override bool RequiresTermination => EndReached;
        public override IReadOnlyList<IReadOnlyList<ITransitionPropertyDescriptor>> SideEffectDescriptors => new ITransitionPropertyDescriptor[][] { };

        public ReadOnlyTape()
        {
        }

        public ReadOnlyTape(BaseTape other) : base(other)
        {
        }

        public override void TakeTransition(Transition transition)
        {
            if (transition.Get(ExpectedChar) != null)
            {
                Position++;
            }
        }

        public override IAutomatonComponent Copy() => new ReadOnlyTape(this);
    }
}
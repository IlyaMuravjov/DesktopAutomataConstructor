using System;

namespace ControlsLibrary.Model.Tape.ReadWriteTape
{
    public class ReadWriteTape : BaseTape
    {
        public override ITransitionSideEffect DefaultSideEffect => new ReadWriteTapeTransitionSideEffect();
        public override bool IsReadyToTerminate => true;
        public override bool RequiresTermination => false;

        public ReadWriteTape()
        {
        }

        public ReadWriteTape(BaseTape other) : base(other)
        {
        }

        public override void TakeTransition(TransitionComponent transition)
        {
            var sideEffect = (ReadWriteTapeTransitionSideEffect) transition.SideEffect;
            if (sideEffect.NewChar != null)
            {
                Data[Position] = (char) sideEffect.NewChar;
            }

            switch (sideEffect.HeadMove)
            {
                case null:
                    break;
                case HeadMoveEnum.Right:
                    Position++;
                    break;
                case HeadMoveEnum.Left:
                    Position--;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override IFaComponent Copy() => new ReadWriteTape(this);
    }
}
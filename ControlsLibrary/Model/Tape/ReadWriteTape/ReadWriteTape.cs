using System;
using System.Collections.Generic;

namespace ControlsLibrary.Model.Tape.ReadWriteTape
{
    public class ReadWriteTape : BaseTape
    {
        public override bool IsReadyToTerminate => true;
        public override bool RequiresTermination => false;
        public TransitionPropertyDescriptor NewChar { get; } = new TransitionPropertyDescriptor(typeof(char?), null);
        public TransitionPropertyDescriptor HeadMove { get; } = new TransitionPropertyDescriptor(typeof(HeadMoveEnum), HeadMoveEnum.Right);
        public override IReadOnlyList<IReadOnlyList<TransitionPropertyDescriptor>> SideEffectDescriptors => new[] {new[] {NewChar}, new[] {HeadMove}};

        public ReadWriteTape()
        {
        }

        public ReadWriteTape(BaseTape other) : base(other)
        {
        }

        public override void TakeTransition(Transition transition)
        {
            if (transition[NewChar] != null)
            {
                Data[Position] = (char) transition[NewChar];
            }

            switch (transition[HeadMove])
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
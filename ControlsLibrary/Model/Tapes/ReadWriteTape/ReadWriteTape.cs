using System;
using System.Collections.Generic;
using ControlsLibrary.Model.TransitionProperty;
using ControlsLibrary.Model.TransitionProperty.Generic;
using ControlsLibrary.Model.TransitionProperty.Impl;

namespace ControlsLibrary.Model.Tapes.ReadWriteTape
{
    public class ReadWriteTape : BaseTape
    {
        public override bool IsReadyToTerminate => true;
        public override bool RequiresTermination => false;
        public ITransitionPropertyDescriptor<char?> NewChar { get; } = new TransitionPropertyDescriptor<char?>(null);
        public ITransitionPropertyDescriptor<HeadMoveEnum?> HeadMove { get; } = new TransitionPropertyDescriptor<HeadMoveEnum?>(HeadMoveEnum.Right);
        public override IReadOnlyList<IReadOnlyList<ITransitionPropertyDescriptor>> SideEffectDescriptors => new[] {new ITransitionPropertyDescriptor[] {NewChar}, new ITransitionPropertyDescriptor[] {HeadMove}};

        public ReadWriteTape()
        {
        }

        public ReadWriteTape(BaseTape other) : base(other)
        {
        }

        public override void TakeTransition(Transition transition)
        {
            if (transition.Get(NewChar) != null)
            {
                Data[Position] = transition.Get(NewChar).Value;
            }

            switch (transition.Get(HeadMove))
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

        public override IAutomatonMemory Copy() => new ReadWriteTape(this);
    }
}
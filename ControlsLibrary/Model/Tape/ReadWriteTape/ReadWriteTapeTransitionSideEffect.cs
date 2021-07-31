using System.Collections.Generic;
using ControlsLibrary.Infrastructure;

namespace ControlsLibrary.Model.Tape.ReadWriteTape
{
    public class ReadWriteTapeTransitionSideEffect : ITransitionSideEffect
    {
        public IReadOnlyList<IReadOnlyList<Property>> Properties { get; } = new List<List<Property>>
        {
            new List<Property> {new Property(typeof(char?)) {Value = null}},
            new List<Property> {new Property(typeof(HeadMoveEnum)) {Value = HeadMoveEnum.Right}}
        };

        public char? NewChar
        {
            get => (char?) Properties[0][0].Value;
            set => Properties[0][0].Value = value;
        }

        public HeadMoveEnum? HeadMove
        {
            get => (HeadMoveEnum?) Properties[1][0].Value;
            set => Properties[1][0].Value = value;
        }
    }
}
using System.Collections.Generic;
using ControlsLibrary.Infrastructure;

namespace ControlsLibrary.Model.SimpleImpl
{
    public class EmptyTransitionSideEffect : ITransitionSideEffect
    {
        public static EmptyTransitionSideEffect Instance { get; } = new EmptyTransitionSideEffect();
        public IReadOnlyList<IReadOnlyList<Property>> Properties { get; } = new List<List<Property>>();
    }
}
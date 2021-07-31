using System.Collections.Generic;
using ControlsLibrary.Infrastructure;

namespace ControlsLibrary.Model
{
    public interface ITransitionSideEffect
    {
        public IReadOnlyList<IReadOnlyList<Property>> Properties { get; }
    }
}
using System.Collections.Generic;
using ControlsLibrary.Infrastructure;

namespace ControlsLibrary.Model
{
    public interface ITransitionFilter
    {
        public IReadOnlyList<IReadOnlyList<Property>> Properties { get; }
    }
}
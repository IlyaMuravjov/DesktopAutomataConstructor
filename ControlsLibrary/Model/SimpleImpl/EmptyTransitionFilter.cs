using System.Collections.Generic;
using ControlsLibrary.Infrastructure;

namespace ControlsLibrary.Model.SimpleImpl
{
    public class EmptyTransitionFilter : ITransitionFilter
    {
        public IReadOnlyList<IReadOnlyList<Property>> Properties { get; } = new List<List<Property>>();
    }
}
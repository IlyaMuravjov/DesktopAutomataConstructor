using System.Collections.Generic;
using ControlsLibrary.Infrastructure;

namespace ControlsLibrary.Model.SimpleImpl
{
    public class CharTransitionFilter : ITransitionFilter
    {
        public IReadOnlyList<IReadOnlyList<Property>> Properties { get; } = new List<List<Property>>
        {
            new List<Property> {new Property(typeof(char?)) {Value = null}}
        };

        public char? ExpectedChar
        {
            get => (char?) Properties[0][0].Value;
            set => Properties[0][0].Value = value;
        }
    }
}
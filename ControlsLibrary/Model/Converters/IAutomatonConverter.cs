using System.Collections.Generic;

namespace ControlsLibrary.Model.Converters
{
    public interface IAutomatonConverter
    {
        public bool IsCompatibleWithComponents(IReadOnlyList<IAutomatonComponent> components);
        public EditableAutomaton Convert(EditableAutomaton automaton);
    }
}
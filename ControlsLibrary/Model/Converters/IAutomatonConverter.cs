using System.Collections.Generic;

namespace ControlsLibrary.Model.Converters
{
    public interface IAutomatonConverter
    {
        public bool IsCompatibleWithMemory(IReadOnlyList<IAutomatonMemory> memoryList);
        public Automaton Convert(Automaton automaton);
    }
}
using System.Collections.Generic;

namespace ControlsLibrary.Model.Converter
{
    public interface IFaConverter
    {
        public bool IsCompatibleWithComponents(IReadOnlyList<IFaComponent> components);
        public EditableFa Convert(EditableFa fa);
    }
}
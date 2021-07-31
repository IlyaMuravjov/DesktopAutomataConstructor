using System;

namespace ControlsLibrary.Infrastructure.Events
{
    public delegate void ElementRemoved<T>(object sender, ElementRemovedEventArgs<T> e);

    public class ElementRemovedEventArgs<T> : EventArgs
    {
        public T OldElement { get; }

        public ElementRemovedEventArgs(T oldElement)
        {
            OldElement = oldElement;
        }
    }
}
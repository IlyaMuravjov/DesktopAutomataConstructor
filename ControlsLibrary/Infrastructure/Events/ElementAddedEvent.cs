using System;

namespace ControlsLibrary.Infrastructure.Events
{
    public delegate void ElementAdded<T>(object sender, ElementAddedEventArgs<T> e);

    public class ElementAddedEventArgs<T> : EventArgs
    {
        public T NewElement { get; }

        public ElementAddedEventArgs(T newElement)
        {
            NewElement = newElement;
        }
    }
}
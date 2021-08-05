using System.Collections.Generic;
using System.Linq;
using ControlsLibrary.Infrastructure.Events;
using ControlsLibrary.Model.Analyzers;
using ControlsLibrary.Model.TransitionCollections;

namespace ControlsLibrary.Model
{
    public class EditableFa
    {
        private readonly ISet<long> usedStateIDs = new HashSet<long>();

        // intended type `Dictionary<FaAnalyzerFactory<T>, T>` isn't representable in C# type system
        private readonly Dictionary<object, object> analyzers = new Dictionary<object, object>();
        public IReadOnlyList<IFaComponent> Components { get; }
        private readonly Dictionary<State, TransitionTable> transitions = new Dictionary<State, TransitionTable>();
        public IReadOnlyDictionary<State, TransitionTable> Transitions => transitions;
        public IReadOnlyCollection<State> States => transitions.Keys;
        private readonly Dictionary<State, ISet<Transition>> incomingTransitions = new Dictionary<State, ISet<Transition>>();

        public event ElementAdded<State> StateAdded;
        public event ElementRemoved<State> StateRemoved;
        public event ElementAdded<Transition> TransitionAdded;
        public event ElementRemoved<Transition> TransitionRemoved;

        public EditableFa(IReadOnlyList<IFaComponent> components)
        {
            Components = components;
        }

        public void AddState(State state)
        {
            long id = 0;
            while (usedStateIDs.Contains(id)) id++;
            usedStateIDs.Add(id);
            state.ID = id;
            state.Name ??= "S" + id;
            transitions[state] = new TransitionTable(Components);
            incomingTransitions[state] = new HashSet<Transition>();
            StateAdded?.Invoke(this, new ElementAddedEventArgs<State>(state));
        }

        public void RemoveState(State state)
        {
            transitions[state].Transitions.ToList().ForEach(RemoveTransition);
            incomingTransitions[state].ToList().ForEach(RemoveTransition);
            transitions.Remove(state);
            incomingTransitions.Remove(state);
            usedStateIDs.Remove(state.ID);
            StateRemoved?.Invoke(this, new ElementRemovedEventArgs<State>(state));
        }

        public Transition AddTransition(State source, State target)
        {
            var transition = new Transition(
                source,
                target,
                Components.SelectMany(component => component.FilterDescriptors).ToList(),
                Components.SelectMany(component => component.FilterDescriptors).ToList()
            );
            transitions[source].AddTransition(transition);
            incomingTransitions[target].Add(transition);
            TransitionAdded?.Invoke(this, new ElementAddedEventArgs<Transition>(transition));
            return transition;
        }

        public void RemoveTransition(Transition transition)
        {
            transitions[transition.Source].RemoveTransition(transition);
            incomingTransitions[transition.Target].Remove(transition);
            TransitionRemoved?.Invoke(this, new ElementRemovedEventArgs<Transition>(transition));
        }

        public T GetAnalyzer<T>(FaAnalyzerFactory<T> analyzerFactory)
        {
            if (!analyzers.ContainsKey(analyzerFactory))
            {
                analyzers[analyzerFactory] = analyzerFactory(this);
            }

            return (T) analyzers[analyzerFactory];
        }
    }
}
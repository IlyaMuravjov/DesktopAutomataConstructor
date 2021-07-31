using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ControlsLibrary.Model.Analyzers;
using ControlsLibrary.Model.SimpleImpl;
using ControlsLibrary.Model.Tape.ReadOnlyTape;
using GraphX.Common;

namespace ControlsLibrary.Model.Converter
{
    public class NfaToDfaConverter : IFaConverter
    {
        public bool IsCompatibleWithComponents(IReadOnlyList<IFaComponent> components) =>
            components.Count == 1 && components[0] is ReadOnlyTape;

        public EditableFa Convert(EditableFa nfa)
        {
            Debug.Assert(IsCompatibleWithComponents(nfa.Components));
            if (!nfa.IsExecutable() || nfa.GetNonDeterministicStates().Count == 0)
            {
                throw new InvalidOperationException(); // TODO error message
            }

            var dfa = new EditableFa(nfa.Components);

            var unhandledQueue = new Queue<(HashSet<State>, State)>();
            var handledStates = new HashSet<State>();
            var statesToCompositeMap = new Dictionary<HashSet<State>, State>(HashSet<State>.CreateSetComparer());

            var initStates = EpsilonClosure(nfa, nfa.GetInitialStates());
            var initState = CreateCompositeState(initStates);
            initState.IsInitial = true;
            unhandledQueue.Enqueue((initStates, initState));
            statesToCompositeMap[initStates] = initState;

            dfa.AddState(initState);

            while (unhandledQueue.Count != 0)
            {
                var (sourceStates, source) = unhandledQueue.Dequeue();
                if (!handledStates.Add(source))
                {
                    continue;
                }

                foreach (var grouping in sourceStates
                    .SelectMany(state => nfa.Transitions[state].Transitions)
                    .GroupBy(transition => ((CharTransitionFilter) transition.Components[0].Filter).ExpectedChar))
                {
                    if (grouping.Key == null)
                    {
                        continue;
                    }

                    var expectedChar = (char) grouping.Key;
                    var targetStates = EpsilonClosure(nfa, grouping.Select(transition => transition.Target));
                    if (targetStates.Count == 0)
                    {
                        continue;
                    }

                    if (!statesToCompositeMap.TryGetValue(targetStates, out var target))
                    {
                        target = CreateCompositeState(targetStates);
                        statesToCompositeMap[targetStates] = target;
                        dfa.AddState(target);
                        unhandledQueue.Enqueue((targetStates, target));
                    }

                    var newTransition = dfa.AddTransition(source, target);
                    ((CharTransitionFilter) newTransition.Components[0].Filter).ExpectedChar = expectedChar;
                }
            }

            return dfa;
        }

        // TODO move it elsewhere since EpsilonClosure can be used for different tasks (e.g. "Step with closure")
        private static HashSet<State> EpsilonClosure(EditableFa fa, IEnumerable<State> states)
        {
            var handledStates = new HashSet<State>();
            var unhandledStateQueue = new Queue<State>(states);
            while (unhandledStateQueue.Count != 0)
            {
                var state = unhandledStateQueue.Dequeue();
                if (!handledStates.Add(state))
                {
                    continue;
                }

                fa.Transitions[state].EpsilonTransitions
                    .ForEach(transition => unhandledStateQueue.Enqueue(transition.Target));
            }

            return handledStates;
        }

        private static State CreateCompositeState(ICollection<State> states) =>
            new State
            {
                Name = String.Join(",", states.Select(v => v.Name).OrderBy(str => str)),
                IsFinal = states.Any(v => v.IsFinal),
                IsInitial = false
            };
    }
}
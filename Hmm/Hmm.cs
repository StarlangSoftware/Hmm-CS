using System;
using System.Collections.Generic;
using DataStructure;
using Math;

namespace Hmm
{
    public abstract class Hmm<TState, TSymbol>
    {
        protected Matrix TransitionProbabilities;
        protected readonly Dictionary<TState, int> StateIndexes;
        protected readonly HmmState<TState, TSymbol>[] States;
        protected readonly int StateCount;
        protected abstract void CalculatePi(List<TState>[] observations);
        protected abstract void CalculateTransitionProbabilities(List<TState>[] observations);
        public abstract List<TState> Viterbi(List<TSymbol> s);

        /**
         * <summary>A constructor of {@link Hmm} class which takes a {@link Set} of states, an array of observations (which also
         * consists of an array of states) and an array of instances (which also consists of an array of emitted symbols).
         * The constructor initializes the state array with the set of states and uses observations and emitted symbols
         * to calculate the emission probabilities for those states.</summary>
         *
         * <param name="states">A {@link Set} of states, consisting of all possible states for this problem.</param>
         * <param name="observations">An array of instances, where each instance consists of an array of states.</param>
         * <param name="emittedSymbols">An array of instances, where each instance consists of an array of symbols.</param>
         */
        public Hmm(HashSet<TState> states, List<TState>[] observations, List<TSymbol>[] emittedSymbols)
        {
            var i = 0;
            StateCount = states.Count;
            this.States = new HmmState<TState, TSymbol>[StateCount];
            StateIndexes = new Dictionary<TState, int>();
            foreach (var state in states)
            {
                StateIndexes[state] = i;
                i++;
            }

            CalculatePi(observations);
            i = 0;
            foreach (var state in states)
            {
                var emissionProbabilities =
                    CalculateEmissionProbabilities(state, observations, emittedSymbols);
                this.States[i] = new HmmState<TState, TSymbol>(state, emissionProbabilities);
                i++;
            }

            CalculateTransitionProbabilities(observations);
        }

        /**
         * <summary>calculateEmissionProbabilities calculates the emission probabilities for a specific state. The method takes the state,
         * an array of observations (which also consists of an array of states) and an array of instances (which also consists
         * of an array of emitted symbols).</summary>
         *
         * <param name="state">The state for which emission probabilities will be calculated.</param>
         * <param name="observations">An array of instances, where each instance consists of an array of states.</param>
         * <param name="emittedSymbols">An array of instances, where each instance consists of an array of symbols.</param>
         * <returns>A {@link HashMap} Emission probabilities for a single state. Contains a probability for each symbol emitted.</returns>
         */
        protected Dictionary<TSymbol, double> CalculateEmissionProbabilities(TState state, List<TState>[] observations,
            List<TSymbol>[] emittedSymbols)
        {
            var counts = new CounterHashMap<TSymbol>();
            var emissionProbabilities = new Dictionary<TSymbol, double>();
            for (var i = 0; i < observations.Length; i++)
            {
                for (var j = 0; j < observations[i].Count; j++)
                {
                    var currentState = observations[i][j];
                    var currentSymbol = emittedSymbols[i][j];
                    if (currentState.Equals(state))
                    {
                        counts.Put(currentSymbol);
                    }
                }
            }

            double sum = counts.SumOfCounts();
            foreach (var symbol in counts.Keys)
            {
                emissionProbabilities[symbol] = counts[symbol] / sum;
            }

            return emissionProbabilities;
        }

        /**
         * <summary>safeLog calculates the logarithm of a number. If the number is less than 0, the logarithm is not defined, therefore
         * the function returns -Infinity.</summary>
         *
         * <param name="x">Input number</param>
         * <returns>the logarithm of x. If x less than 0 return -infinity.</returns>
         */
        protected double SafeLog(double x)
        {
            if (x <= 0)
            {
                return int.MinValue;
            }

            return System.Math.Log(x);
        }
    }
}
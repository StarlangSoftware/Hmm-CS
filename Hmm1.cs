using System.Collections.Generic;
using Math;

namespace Hmm
{
    public class Hmm1<TState, TSymbol> : Hmm<TState, TSymbol>
    {
        private Vector _pi;

        /**
         * <summary>A constructor of {@link Hmm1} class which takes a {@link Set} of states, an array of observations (which also
         * consists of an array of states) and an array of instances (which also consists of an array of emitted symbols).
         * The constructor calls its super method to calculate the emission probabilities for those states.</summary>
         *
         * <param name="states">A {@link Set} of states, consisting of all possible states for this problem.</param>
         * <param name="observations">An array of instances, where each instance consists of an array of states.</param>
         * <param name="emittedSymbols">An array of instances, where each instance consists of an array of symbols.</param>
         */
        public Hmm1(HashSet<TState> states, List<TState>[] observations, List<TSymbol>[] emittedSymbols) : base(states,
            observations, emittedSymbols)
        {
        }

        /**
         * <summary>calculatePi calculates the prior probability vector (initial probabilities for each state) from a set of
         * observations. For each observation, the function extracts the first state in that observation. Normalizing the
         * counts of the states returns us the prior probabilities for each state.</summary>
         *
         * <param name="observations">A set of observations used to calculate the prior probabilities.</param>
         */
        protected override void CalculatePi(List<TState>[] observations)
        {
            _pi = new Vector(StateCount, 0.0);
            foreach (var observation in observations)
            {
                int index = StateIndexes[observation[0]];
                _pi.AddValue(index, 1.0);
            }

            _pi.L1Normalize();
        }

        /**
         * <summary>calculateTransitionProbabilities calculates the transition probabilities matrix from each state to another state.
         * For each observation and for each transition in each observation, the function gets the states. Normalizing the
         * counts of the pair of states returns us the transition probabilities.</summary>
         *
         * <param name="observations">A set of observations used to calculate the transition probabilities.</param>
         */
        protected override void CalculateTransitionProbabilities(List<TState>[] observations)
        {
            TransitionProbabilities = new Matrix(StateCount, StateCount);
            foreach (var current in observations)
            {
                for (int j = 0; j < current.Count - 1; j++)
                {
                    var from = StateIndexes[current[j]];
                    var to = StateIndexes[current[j + 1]];
                    TransitionProbabilities.Increment(from, to);
                }
            }

            TransitionProbabilities.ColumnWiseNormalize();
        }

        /**
         * <summary>logOfColumn calculates the logarithm of each value in a specific column in the transition probability matrix.</summary>
         *
         * <param name="column">Column index of the transition probability matrix.</param>
         * <returns>A vector consisting of the logarithm of each value in the column in the transition probability matrix.</returns>
         */
        private Vector LogOfColumn(int column)
        {
            var result = new Vector(0, 0);
            int i;
            for (i = 0; i < StateCount; i++)
            {
                result.Add(SafeLog(TransitionProbabilities.GetValue(i, column)));
            }

            return result;
        }

        /**
         * <summary>viterbi calculates the most probable state sequence for a set of observed symbols.</summary>
         *
         * <param name="s">A set of observed symbols.</param>
         * <returns>The most probable state sequence as an {@link ArrayList}.</returns>
         */
        public override List<TState> Viterbi(List<TSymbol> s)
        {
            int i;
            int t;
            double observationLikelihood;
            var sequenceLength = s.Count;
            var gamma = new Matrix(sequenceLength, StateCount);
            var phi = new Matrix(sequenceLength, StateCount);
            var qs = new Vector(sequenceLength, 0);
            var result = new List<TState>();
            /*Initialize*/
            var emission = s[0];
            for (i = 0; i < StateCount; i++)
            {
                observationLikelihood = States[i].GetEmitProb(emission);
                gamma.SetValue(0, i, SafeLog(_pi.GetValue(i)) + SafeLog(observationLikelihood));
            }

            /*Iterate Dynamic Programming*/
            for (t = 1; t < sequenceLength; t++)
            {
                emission = s[t];
                for (var j = 0; j < StateCount; j++)
                {
                    var tempArray = LogOfColumn(j);
                    tempArray.Add(gamma.GetRow(t - 1));
                    var maxIndex = tempArray.MaxIndex();
                    observationLikelihood = States[j].GetEmitProb(emission);
                    gamma.SetValue(t, j, tempArray.GetValue(maxIndex) + SafeLog(observationLikelihood));
                    phi.SetValue(t, j, maxIndex);
                }
            }

            /*Backtrack pointers*/
            qs.SetValue(sequenceLength - 1, gamma.GetRow(sequenceLength - 1).MaxIndex());
            result.Insert(0, States[(int) qs.GetValue(sequenceLength - 1)].GetState());
            for (i = sequenceLength - 2; i >= 0; i--)
            {
                qs.SetValue(i, phi.GetValue(i + 1, (int) qs.GetValue(i + 1)));
                result.Insert(0, States[(int) qs.GetValue(i)].GetState());
            }

            return result;
        }
    }
}
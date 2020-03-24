using System.Collections.Generic;
using Math;

namespace Hmm
{
    public class Hmm2<TState, TSymbol> : Hmm<TState, TSymbol>
    {
        private Matrix _pi;

        /**
         * <summary>A constructor of {@link Hmm2} class which takes a {@link Set} of states, an array of observations (which also
         * consists of an array of states) and an array of instances (which also consists of an array of emitted symbols).
         * The constructor calls its super method to calculate the emission probabilities for those states.</summary>
         *
         * <param name="states">A {@link Set} of states, consisting of all possible states for this problem.</param>
         * <param name="observations">An array of instances, where each instance consists of an array of states.</param>
         * <param name="emittedSymbols">An array of instances, where each instance consists of an array of symbols.</param>
         */
        public Hmm2(HashSet<TState> states, List<TState>[] observations, List<TSymbol>[] emittedSymbols) : base(states,
            observations, emittedSymbols)
        {
        }

        /**
         * <summary>calculatePi calculates the prior probability matrix (initial probabilities for each state combinations)
         * from a set of observations. For each observation, the function extracts the first and second states in
         * that observation.  Normalizing the counts of the pair of states returns us the prior probabilities for each
         * pair of states.</summary>
         *
         * <param name="observations">A set of observations used to calculate the prior probabilities.</param>
         */
        protected override void CalculatePi(List<TState>[] observations)
        {
            _pi = new Matrix(StateCount, StateCount);
            foreach (var observation in observations)
            {
                var first = StateIndexes[observation[0]];
                var second = StateIndexes[observation[1]];
                _pi.Increment(first, second);
            }

            _pi.ColumnWiseNormalize();
        }

        /**
         * <summary>calculateTransitionProbabilities calculates the transition probabilities matrix from each state to another state.
         * For each observation and for each transition in each observation, the function gets the states. Normalizing the
         * counts of the three of states returns us the transition probabilities.</summary>
         *
         * <param name="observations">A set of observations used to calculate the transition probabilities.</param>
         */
        protected override void CalculateTransitionProbabilities(List<TState>[] observations)
        {
            TransitionProbabilities = new Matrix(StateCount * StateCount, StateCount);
            foreach (var current in observations)
            {
                for (var j = 0; j < current.Count - 2; j++)
                {
                    var from1 = StateIndexes[current[j]];
                    var from2 = StateIndexes[current[j + 1]];
                    var to = StateIndexes[current[j + 2]];
                    TransitionProbabilities.Increment(from1 * StateCount + from2, to);
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
                result.Add(SafeLog(TransitionProbabilities.GetValue(i * StateCount + column / StateCount,
                    column % StateCount)));
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
            int i, j, t;
            double observationLikelihood;
            var sequenceLength = s.Count;
            var gamma = new Matrix(sequenceLength, StateCount * StateCount);
            var phi = new Matrix(sequenceLength, StateCount * StateCount);
            var qs = new Vector(sequenceLength, 0);
            var result = new List<TState>();
            /*Initialize*/
            var emission1 = s[0];
            var emission2 = s[1];
            for (i = 0; i < StateCount; i++)
            {
                for (j = 0; j < StateCount; j++)
                {
                    observationLikelihood = States[i].GetEmitProb(emission1) * States[j].GetEmitProb(emission2);
                    gamma.SetValue(1, i * StateCount + j, SafeLog(_pi.GetValue(i, j)) + SafeLog(observationLikelihood));
                }
            }

            /*Iterate Dynamic Programming*/
            for (t = 2; t < sequenceLength; t++)
            {
                var emission = s[t];
                for (j = 0; j < StateCount * StateCount; j++)
                {
                    var current = LogOfColumn(j);
                    var previous = gamma.GetRow(t - 1).SkipVector(StateCount, j / StateCount);
                    current.Add(previous);
                    var maxIndex = current.MaxIndex();
                    observationLikelihood = States[j % StateCount].GetEmitProb(emission);
                    gamma.SetValue(t, j, current.GetValue(maxIndex) + SafeLog(observationLikelihood));
                    phi.SetValue(t, j, maxIndex * StateCount + j / StateCount);
                }
            }

            /*Backtrack pointers*/
            qs.SetValue(sequenceLength - 1, gamma.GetRow(sequenceLength - 1).MaxIndex());
            result.Insert(0, States[((int) qs.GetValue(sequenceLength - 1)) % StateCount].GetState());
            for (i = sequenceLength - 2; i >= 1; i--)
            {
                qs.SetValue(i, phi.GetValue(i + 1, (int) qs.GetValue(i + 1)));
                result.Insert(0, States[((int) qs.GetValue(i)) % StateCount].GetState());
            }

            result.Insert(0, States[((int) qs.GetValue(1)) / StateCount].GetState());
            return result;
        }
    }
}
using System.Collections.Generic;

namespace Hmm
{
    public class HmmState<TState, TSymbol>
    {
        private readonly Dictionary<TSymbol, double> _emissionProbabilities;
        private readonly TState _state;

        /**
         * <summary>A constructor of {@link HmmState} class which takes a {@link State} and emission probabilities as inputs and
         * initializes corresponding class variable with these inputs.</summary>
         *
         * <param name="state">Data for this state.</param>
         * <param name="emissionProbabilities">Emission probabilities for this state</param>
         */
        public HmmState(TState state, Dictionary<TSymbol, double> emissionProbabilities)
        {
            this._state = state;
            this._emissionProbabilities = emissionProbabilities;
        }

        /**
         * <summary>Accessor method for the state variable.</summary>
         *
         * <returns>state variable.</returns>
        */
        public TState GetState()
        {
            return _state;
        }

        /**
         * <summary>getEmitProb method returns the emission probability for a specific symbol.</summary>
         *
         * <param name="symbol">Symbol for which the emission probability will be get.</param>
         * <returns>Emission probability for a specific symbol.</returns>
         */
        public double GetEmitProb(TSymbol symbol)
        {
            return _emissionProbabilities.GetValueOrDefault(symbol, 0.0);
        }
    }
}
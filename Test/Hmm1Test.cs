using System.Collections.Generic;
using Hmm;
using NUnit.Framework;

namespace Test
{
    public class Hmm1Test
    {
        [Test]
        public void TestViterbi()
        {
            var observationCount = 5;
            var states = new HashSet<string> {"HOT", "COLD"};
            var observations = new List<string>[observationCount];
            var emittedSymbols = new List<int>[observationCount];
            for (var i = 0; i < observationCount; i++)
            {
                observations[i] = new List<string>();
                emittedSymbols[i] = new List<int>();
            }

            observations[0].Add("HOT");
            observations[0].Add("HOT");
            observations[0].Add("HOT");
            emittedSymbols[0].Add(3);
            emittedSymbols[0].Add(2);
            emittedSymbols[0].Add(3);
            observations[1].Add("HOT");
            observations[1].Add("COLD");
            observations[1].Add("COLD");
            observations[1].Add("COLD");
            emittedSymbols[1].Add(2);
            emittedSymbols[1].Add(2);
            emittedSymbols[1].Add(1);
            emittedSymbols[1].Add(1);
            observations[2].Add("HOT");
            observations[2].Add("COLD");
            observations[2].Add("HOT");
            observations[2].Add("COLD");
            emittedSymbols[2].Add(3);
            emittedSymbols[2].Add(1);
            emittedSymbols[2].Add(2);
            emittedSymbols[2].Add(1);
            observations[3].Add("COLD");
            observations[3].Add("COLD");
            observations[3].Add("COLD");
            observations[3].Add("HOT");
            observations[3].Add("HOT");
            emittedSymbols[3].Add(3);
            emittedSymbols[3].Add(1);
            emittedSymbols[3].Add(2);
            emittedSymbols[3].Add(2);
            emittedSymbols[3].Add(3);
            observations[4].Add("COLD");
            observations[4].Add("HOT");
            observations[4].Add("HOT");
            observations[4].Add("COLD");
            observations[4].Add("COLD");
            emittedSymbols[4].Add(1);
            emittedSymbols[4].Add(2);
            emittedSymbols[4].Add(3);
            emittedSymbols[4].Add(2);
            emittedSymbols[4].Add(1);
            Hmm<string, int> hmm1 = new Hmm1<string, int>(states, observations, emittedSymbols);
            var observed = new List<int>
            {
                1,
                1,
                1,
                1,
                1,
                1
            };
            var observedStates = hmm1.Viterbi(observed);
            Assert.AreEqual("COLD", observedStates[0]);
            Assert.AreEqual("COLD", observedStates[1]);
            Assert.AreEqual("COLD", observedStates[2]);
            Assert.AreEqual("COLD", observedStates[3]);
            Assert.AreEqual("COLD", observedStates[4]);
            Assert.AreEqual("COLD", observedStates[5]);
            observed = new List<int>
            {
                1,
                2,
                3,
                3,
                2,
                1
            };
            observedStates = hmm1.Viterbi(observed);
            Assert.AreEqual("COLD", observedStates[0]);
            Assert.AreEqual("HOT", observedStates[1]);
            Assert.AreEqual("HOT", observedStates[2]);
            Assert.AreEqual("HOT", observedStates[3]);
            Assert.AreEqual("HOT", observedStates[4]);
            Assert.AreEqual("COLD", observedStates[5]);
            observed = new List<int>
            {
                3,
                3,
                3,
                3,
                3,
                3
            };
            observedStates = hmm1.Viterbi(observed);
            Assert.AreEqual("HOT", observedStates[0]);
            Assert.AreEqual("HOT", observedStates[1]);
            Assert.AreEqual("HOT", observedStates[2]);
            Assert.AreEqual("HOT", observedStates[3]);
            Assert.AreEqual("HOT", observedStates[4]);
            Assert.AreEqual("HOT", observedStates[5]);
        }
    }
}
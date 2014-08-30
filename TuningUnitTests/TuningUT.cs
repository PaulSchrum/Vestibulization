using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RxSpatial;

namespace TuningUnitTests
{
    [TestClass]
    public class TuningUT
    {

        private Tuning setupTuning1()
        {
            var tn = new Tuning();
            tn.AverageStillValue = 0.2;
            tn.SquelchThreshold = 0.05;
            return tn;
        }

        [TestMethod]
        public void ValueWithinSquelchThreshold_returnsZero()
        {
            var tn = setupTuning1();
            Double expected = 0.0;
            Double actual = tn.SquelchFilter(0.22);
            Assert.AreEqual(expected: expected, actual: actual, delta: 0.0000001);
        }

        [TestMethod]
        public void ValueOutsideSquelchThreshold_returnsCorrectAdjustedValue()
        {
            var tn = setupTuning1();
            Double expected = -0.1;
            Double actual = tn.SquelchFilter(0.1);
            Assert.AreEqual(expected: expected, actual: actual, delta: 0.0000001);
        }

    }
}

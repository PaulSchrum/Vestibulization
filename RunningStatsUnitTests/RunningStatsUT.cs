using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RxSpatial;

namespace RunningStatsUnitTests
{
   [TestClass]
   public class RunningStatsUT
   {
      [TestMethod]
      public void RunningStats_Average_IsCorrect()
      {
         RunningStats stats = new RunningStats(10);
         stats.Add(17.8384669509595);
         stats.Add(18.2649061833689);

         Assert.AreEqual(
            expected: 18.05168,
            actual: stats.RunningAverage,
            delta: 0.0001
            );

         stats.Add(17.8384669509595);

         Assert.AreEqual(
            expected: 17.98061,
            actual: stats.RunningAverage,
            delta: 0.0001
            );

         stats.Add(20.8235415778252);
         stats.Add(18.2649061833689);
         stats.Add(20.3971023454158);
         stats.Add(16.9855884861407);
         stats.Add(17.4120277185501);
         stats.Add(20.3971023454158);
         stats.Add(18.2649061833689);

         Assert.AreEqual(
            expected: 18.6487,
            actual: stats.RunningAverage,
            delta: 0.0001
            );

         stats.Add(19.9706631130064);

         Assert.AreEqual(
            expected: 18.86192,
            actual: stats.RunningAverage,
            delta: 0.0001
            );

         stats.Add(16.9855884861407);
         stats.Add(20.3971023454158);
         stats.Add(18.2649061833689);
         stats.Add(16.9855884861407);
         stats.Add(19.1177846481876);
         stats.Add(20.8235415778252);
         stats.Add(19.9706631130064);
         stats.Add(21.2499808102345);
         stats.Add(19.544223880597);
         stats.Add(18.2649061833689);
         stats.Add(20.8235415778252);

         Assert.AreEqual(
            expected: 19.544224,
            actual: stats.RunningAverage,
            delta: 0.0001
            );

      }
   }
}

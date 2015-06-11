using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PrimaryUnitTests
{
   [TestClass]
   public class AccelerometerUnitTests
   {
      ProcessedDataObserver observer = null;

      [TestMethod]
      public void ProcessedData_1_NoMotion_ArrivesAt()
      {
         observer = new ProcessedDataObserver(
            @"C:\SourceModules\ComputerVision\Vestibulization\PrimaryUnitTests\TestFiles\noMotion.csv"
            );
      }

   }
}

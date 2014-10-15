using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RxSpatial
{
   public class Tuning
   {
      public Double AverageStillValue { get; set; }
      public Double SquelchThreshold { get; set; }
      public RunningStats runAverage { get; set; }

      public Tuning() { }

      public Tuning(Double aveStillVal, Double squelchThreshold)
      {
         AverageStillValue = aveStillVal;
         SquelchThreshold = squelchThreshold;
         runAverage = new RunningStats(12);
      }

      public Tuning(Double aveStillVal, Double squelchThreshold, int runningAverageSampleSize) : this(aveStillVal, squelchThreshold)
      {
         runAverage = new RunningStats(runningAverageSampleSize);
      }

      public void AddDataPoint(Double val)
      {
         runAverage.Add(val);
      }

      public Double GetReducedValue()
      {
         return SquelchFilter(runAverage.RunningAverage);
      }

      public Double SquelchFilter (Double inputVal)
      {
         Double outputVal = inputVal;

         if (inputVal < AverageStillValue + SquelchThreshold &&
             inputVal > AverageStillValue - SquelchThreshold)
            outputVal = 0.0;
         
         else
            outputVal = inputVal - AverageStillValue;

         return outputVal;
      }

   }
}

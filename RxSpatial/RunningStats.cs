using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RxSpatial
{
   /// <summary>
   /// Uses a wrap-around index plus addition and subtraction
   /// to keep a time-efficient running average of an array
   /// of Double values.
   /// </summary>
   public class RunningStats
   { // by Paul Schrum July 2014
      protected int Count { get; set; }
      protected int CurrentIndex { get; set; }
      protected int FirstCountUpIndex { get; set; }

      public int SampleSize { get; set; }
      public Double RunningAverage { get; protected set; }
      //public Double RunningMAD { get; protected set; }
      protected List<Double> values = null;
      public RunningStats RunningAverageDeviation { get; private set; }
      protected bool TrackRAD { get; set; }

      protected RunningStats(int sampleSize, bool trackRAD)
      {
         SampleSize = sampleSize;
         values = new List<double>(SampleSize);
         Count = CurrentIndex = FirstCountUpIndex = 0;
         RunningAverage = 0.0;
         TrackRAD = trackRAD;
         if (true == TrackRAD)
            RunningAverageDeviation = new RunningStats(sampleSize, false);
         else
            RunningAverageDeviation = null;
      }

      public RunningStats(int sampleSize) : this (sampleSize, true)
      { }

      public void Add(Double val)
      {
         if (FirstCountUpIndex < SampleSize)
         {
            values.Add(val);
            FirstCountUpIndex++;
            RunningAverage += (val - RunningAverage) / FirstCountUpIndex;
         }
         else
         {
            RunningAverage -= values[CurrentIndex] / SampleSize;
            values[CurrentIndex] = val;
            RunningAverage += val / SampleSize;
            incrementCurrentIndex();
         }
         if (true == this.TrackRAD)
         {
            this.RunningAverageDeviation.Add(val - RunningAverage);
         }
      }

      protected void incrementCurrentIndex()
      {
         CurrentIndex++;
         if (CurrentIndex >= SampleSize) CurrentIndex = 0;
      }

   }
}

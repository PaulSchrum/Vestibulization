using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Diagnostics;
using System.Threading;

namespace RxSpatial
{
   public class AccelerometerFrame_raw
   {
      protected AccelerometerFrame_raw(AccelerometerFrame_raw other)
      {
         Acceleration = other.Acceleration;
         RotationRate = other.RotationRate;
         TimeStampTicks = other.TimeStampTicks;
      }

      internal AccelerometerFrame_raw(
         Double accX,
         Double accY,
         Double accZ,
         Double rotX,
         Double rotY,
         Double rotZ
         )
      {
         TimeStampTicks = stopwatch.ElapsedTicks;
         Acceleration = new Vector3D(accX, accY, accZ);
         RotationRate = new Vector3D(rotX, rotY, rotZ);
      }

      
      public Vector3D Acceleration { get; internal set; }
      public Vector3D RotationRate { get; internal set; }
      public long TimeStampTicks { get; internal set; }
      public Double TimeStampSeconds { get { return TimeStampTicks / ticksPerSecond; } }

      public override string ToString()
      {
         sb.Clear();
         sb.AppendFormat("{0:0.########}", this.TimeStampSeconds).Append(",");
         sb.Append(Acceleration.X).Append(",");
         sb.Append(Acceleration.Y).Append(",");
         sb.Append(Acceleration.Z).Append(",");
         sb.Append(RotationRate.X).Append(",");
         sb.Append(RotationRate.Y).Append(",");
         sb.Append(RotationRate.Z);
         return sb.ToString();
      }
      private static StringBuilder sb {get; set;}

      static public System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
      static internal protected Double ticksPerSecond { get; set; }
      static AccelerometerFrame_raw()
      {
         sb = new StringBuilder();
         ticksPerSecond = Stopwatch.Frequency;
         //return;

         stopwatch.Start();
         Thread.Sleep(2);
         var milliseconds1 = stopwatch.ElapsedMilliseconds;
         var ticks1 = stopwatch.ElapsedTicks;
         Thread.Sleep(10);
         var milliseconds2 = stopwatch.ElapsedMilliseconds;
         var ticks2 = stopwatch.ElapsedTicks;

         var totalTicks = ticks2 - ticks1;
         var totalMs = milliseconds2 - milliseconds1;

         var ticksPerSecond_ = 1000.0 * ((Double)totalTicks / (Double)totalMs);
         //var freq = Stopwatch.Frequency;
      }

   }

}

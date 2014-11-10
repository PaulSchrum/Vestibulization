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

      static public System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
      static protected Double ticksPerSecond { get; set; }
      static AccelerometerFrame_raw()
      {
         stopwatch.Start();
         Thread.Sleep(2);
         var milliseconds1 = stopwatch.ElapsedMilliseconds;
         var ticks1 = stopwatch.ElapsedTicks;
         Thread.Sleep(10);
         var milliseconds2 = stopwatch.ElapsedMilliseconds;
         var ticks2 = stopwatch.ElapsedTicks;

         var totalTicks = ticks2 - ticks1;
         var totalMs = milliseconds2 - milliseconds1;

         ticksPerSecond = 1000.0 * (Double)(totalTicks / totalMs);
      }

   }

}

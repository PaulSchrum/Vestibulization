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
         setValuesFromDoubles(accX, accY, accZ, rotX, rotY, rotZ);
      }

      internal AccelerometerFrame_raw(String[] valueStrings)
      {
         if(valueStrings.Length != 7) 
            throw new ArgumentException("valueString does not contain exactly 7 strings.");
         Double tsSeconds = Double.Parse(valueStrings[0]);
         Double ax = Double.Parse(valueStrings[1]);
         Double ay = Double.Parse(valueStrings[2]);
         Double az = Double.Parse(valueStrings[3]);
         Double rx = Double.Parse(valueStrings[4]);
         Double ry = Double.Parse(valueStrings[5]);
         Double rz = Double.Parse(valueStrings[6]);
         setValuesFromDoubles(ax, ay, az, rx, ry, rz);
         setTicksFromSeconds(tsSeconds);
      }

      private void setValuesFromDoubles
         (double accX, double accY, double accZ, double rotX, double rotY, double rotZ)
      {
         Acceleration = new Vector3D(accX, accY, accZ);
         RotationRate = new Vector3D(rotX, rotY, rotZ);
      }

      public Vector3D Acceleration { get; internal set; }
      public Vector3D RotationRate { get; internal set; }
      public long TimeStampTicks { get; internal set; }
      public Double TimeStampSeconds { get { return TimeStampTicks / ticksPerSecond; } }

      internal void setTicksFromSeconds(Double seconds)
      {
         TimeStampTicks = (long)(seconds * AccelerometerFrame_raw.ticksPerSecond);
      }

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
      }


      internal static void OverrideTicksPerSecond(double ticksPerSecond_)
      {
         ticksPerSecond = ticksPerSecond_;
      }
   }

}

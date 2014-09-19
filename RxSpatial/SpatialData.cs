using Phidgets;
using Phidgets.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace RxSpatial
{
   public struct SpatialData
   {
      public Double AccelerationX;
      public Double AccelerationY;
      public Double AccelerationZ;
      public Double AngularRateX;
      public Double AngularRateY;
      public Double AngularRateZ;
      public long milliseconds;

      private static int count;

      public static Stopwatch stopwatch = new Stopwatch();
      static SpatialData()
      {
          stopwatch.Start();
      }

      public SpatialData(Spatial spatial, SpatialDataEventArgs e)
      {
         AccelerationX = AccelerationY = AccelerationZ = 0.0;
         AngularRateX = AngularRateY = AngularRateZ = 0.0;
         milliseconds = stopwatch.ElapsedMilliseconds;
         if (null == spatial) return;
         if (spatial.accelerometerAxes.Count > 0 &&
            e != null)
         {
            AccelerationX = e.spatialData[0].Acceleration[0];
            AccelerationY = e.spatialData[0].Acceleration[1];
            AccelerationZ = e.spatialData[0].Acceleration[2];
         }
         if (spatial.gyroAxes.Count > 0)
         {
            AngularRateX = spatial.gyroAxes[0].AngularRate;
            AngularRateY = spatial.gyroAxes[1].AngularRate;
            AngularRateZ = spatial.gyroAxes[2].AngularRate;
         }
         //if (null != e)
         //   milliseconds = e.spatialData[0].Timestamp;
      }

      internal void WriteToStreamAscii(System.IO.StreamWriter outputFileStream)
      {
         if (null == outputFileStream) return;
         if(0 == count)
         {
            count++;
            outputFileStream.WriteLine
               ("Milliseconds,Accel.X,Accel.Y,Accel.Z,AngularRate.X,AngularRate.Y,AngularRate.Z");
         }
         String saveString = 
            milliseconds.ToString() + "," +
            AccelerationX.ToString() + "," +
            AccelerationY.ToString() + "," +
            AccelerationZ.ToString() + "," +
            AngularRateX.ToString() + "," +
            AngularRateY.ToString() + "," +
            AngularRateZ.ToString();
         outputFileStream.WriteLine(saveString);
      }
   }
}

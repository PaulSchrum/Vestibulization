using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RxSpatial
{
   public sealed class AccelerometerFrame
   {
      internal AccelerometerFrame(
         Double accX,
         Double accY,
         Double accZ,
         Double rotX,
         Double rotY,
         Double rotZ
         )
      {
         RawData = new RawData(
            accX, accY, accZ, rotX, rotY, rotZ);
      }

      public RawData RawData { get; internal set; }
      public SmoothedData SmoothedData { get; internal set; }
   }
}

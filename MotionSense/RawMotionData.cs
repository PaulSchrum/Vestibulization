using GeometryCore.Lines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotionSense
{
   public class RawMotionData
   {
      internal RawMotionData(
         Double AccelX,
         Double AccelY,
         Double AccelZ,
         Double RotateX,
         Double RotateY,
         Double RotateZ,
         Double MagX,
         Double MagY,
         Double MagZ
         )
      {
         Acceleration = new Vector(AccelX, AccelY, AccelZ);
         Rotation = new Vector(RotateX, RotateY, RotateZ);
         MagneticField = new Vector(MagX, MagY, MagZ);
      }

      public Vector Acceleration { get; internal set; }
      public Vector Rotation { get; internal set; }
      public Vector MagneticField { get; internal set; }
   }
}

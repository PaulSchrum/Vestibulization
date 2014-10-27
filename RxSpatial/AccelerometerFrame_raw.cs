using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace RxSpatial
{
   public class AccelerometerFrame_raw
   {
      internal AccelerometerFrame_raw(
         Double accX,
         Double accY,
         Double accZ,
         Double rotX,
         Double rotY,
         Double rotZ
         )
      {
          Acceleration = new Vector3D(accX, accY, accZ);
          Rotation = new Vector3D(rotX, rotY, rotZ);
      }

      public Vector3D Acceleration { get; internal set; }
      public Vector3D Rotation { get; internal set; }
   }
}

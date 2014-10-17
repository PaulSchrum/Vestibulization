using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace RxSpatial
{
   public sealed class RawData
   {
      internal RawData(
         Double AccelX,
         Double AccelY,
         Double AccelZ,
         Double RotateX,
         Double RotateY,
         Double RotateZ
         )
      {
         Acceleration = new Vector3D(AccelX,AccelY,AccelZ);
         Rotation = new Vector3D(RotateX,RotateY,RotateZ);
      }

      public Vector3D Acceleration { get; internal set; }
      public Vector3D Rotation { get; internal set; }
   }
}

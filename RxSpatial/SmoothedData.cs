using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace RxSpatial
{
   public sealed class SmoothedData
   {
      public Vector3D Jerk;            // Space Vector
      public Vector3D Acceleration;    // Space Vector
      public Vector3D Velocity;        // Space Vector
      public Vector3D Position;        // Space Vector

      public Vector3D Rotation;        // Degrees per second
      public Vector3D Orientation;     // Degrees
      public Vector3D LookDirection;   // Space Vector
      public Vector3D UpDirection;     // Space Vector
   }
}

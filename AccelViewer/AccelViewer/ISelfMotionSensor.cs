using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MotionSense;

namespace AccelViewer
{
   public interface ISelfMotionSensor
   {
      event VectorEventFire OnAcclerometerFire;
      event VectorEventFire OnGyroscopeFire;
      event VectorEventFire OnMagnetometerFire;
   }
}

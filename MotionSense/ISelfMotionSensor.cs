using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotionSense
{
   public interface ISelfMotionSensor
   {
      /* * /
      event EventArgs<VectorFrameEventArgs> Acceleration;
      /* */
   }

   public delegate void VectorEventFire(Object obj, VectorFrameEventArgs e);

}

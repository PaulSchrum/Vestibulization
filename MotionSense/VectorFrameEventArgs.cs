using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GeometryCore.Lines;

namespace MotionSense
{
   public class VectorFrameEventArgs : EventArgs
   {
      public Vector Frame { get; private set; }

      public VectorFrameEventArgs(Double x_, Double y_, Double z_)
      {
         this.Frame = new Vector(x_, y_, z_);
      }
   }
}

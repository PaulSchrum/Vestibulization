using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phidgets;
using Phidgets.Events;
using System.Reactive.Linq;
using System.Reactive;

namespace RxSpatial.Streamers
{
   public abstract class SpatialDataStreamer_raw
   {
      public event EventHandler AttachedStateChanged;
      public event EventHandler CalibratingStateChanged;
      public event EventHandler WriteStateChanged;

      protected static SpatialDataStreamer_raw singleton;
      public static SpatialDataStreamer_raw Create(
         AccelerometerType accType = AccelerometerType.Phidgets1056_333,
         String dataFileName = null)
      {
         if(null == singleton)
         {
            if (accType == AccelerometerType.Phidgets1056_333)
               singleton = new SpatialDataStreamer_rawPhidgets1056_333();
            else if (accType == AccelerometerType.File)
               singleton = new SpatialDataStreamer_rawFile(dataFileName);
         }
         return singleton;
      }

      //IDisposable subscription;
      public IObservable<AccelerometerFrame_raw> DataStream { get; protected set; }

      protected SpatialDataStreamer_raw()
      {
      }

      private bool isAttached_;
      public bool IsAttached
      {
         get { return isAttached_; }
         set
         {
            isAttached_ = value;
            if (null == AttachedStateChanged) return;
            AttachedStateChanged(this, new BooleanState(isAttached_));
         }
      }
   }

   public class BooleanState : EventArgs
   {
      internal BooleanState(bool isTrue_) { this.IsTrue = isTrue_; }
      public bool IsTrue;
   }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phidgets;
using Phidgets.Events;
using System.Reactive.Linq;
using System.Reactive;

namespace RxSpatial
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
            singleton = new SpatialDataStreamer_rawPhidgets1056_333();
         }
         return singleton;
      }

      //private IDisposable subscription;
      public IObservable<AccelerometerFrame_raw> DeviceDataStream { get; protected set; }

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

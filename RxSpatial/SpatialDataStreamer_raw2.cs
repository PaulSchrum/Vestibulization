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
   public class SpatialDataStreamer_raw2
   {
      private static Spatial spatial = null;

      public event EventHandler AttachedStateChanged;
      public event EventHandler CalibratingStateChanged;
      public event EventHandler WriteStateChanged;

      private static SpatialDataStreamer_raw2 singleton;
      public static SpatialDataStreamer_raw2 Create()
      {
         if(null == singleton)
         {
            singleton = new SpatialDataStreamer_raw2();
         }
         return singleton;
      }

      private IObservable<AccelerometerFrame_raw> SetupDeviceStream()
      {
         var stream =
            (from evt in spatialEvents
            let e = evt.EventArgs
            select new AccelerometerFrame_raw
            (
               e.spatialData[0].Acceleration[0],
               e.spatialData[0].Acceleration[1],
               e.spatialData[0].Acceleration[2],
               e.spatialData[0].AngularRate[0],
               e.spatialData[0].AngularRate[1],
               e.spatialData[0].AngularRate[2]
            )).Publish();
         stream.Connect();
         return stream;
      }

      //private IDisposable subscription;
      private IObservable<EventPattern<SpatialDataEventArgs>> spatialEvents;

      public IObservable<AccelerometerFrame_raw> DeviceDataStream { get; private set; }

      private SpatialDataStreamer_raw2()
      {
         IsAttached = false;
         spatial = new Spatial();
         spatial.close();
         spatial.Attach += new AttachEventHandler(spatial_Attach);
         spatial.Detach += new DetachEventHandler(spatial_Detach);
         //accelEventHandler = new SpatialDataEventHandler(spatial_SpatialData_raw);
         //spatial.SpatialData += accelEventHandler;
         spatialEvents = System.Reactive.Linq.Observable.FromEventPattern
            <SpatialDataEventHandler, SpatialDataEventArgs>(
            handler => handler.Invoke,
            h => spatial.SpatialData += h,
            h => spatial.SpatialData -= h);
         //subscription = spatialEvents.Subscribe();
         DeviceDataStream = SetupDeviceStream();
         spatial.open(-1);
      }

      private void spatial_Attach(object sender, AttachEventArgs e)
      {
         IsAttached = true;
      }

      private void spatial_Detach(object sender, DetachEventArgs e)
      {
         IsAttached = false;
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

}

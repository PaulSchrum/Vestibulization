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
   public class SpatialDataStreamer_rawPhidgets1056_333 : SpatialDataStreamer_raw
   {
      private static Spatial spatial = null;
      private IObservable<EventPattern<SpatialDataEventArgs>> spatialEvents;

      internal SpatialDataStreamer_rawPhidgets1056_333()
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
         DataStream = SetupDeviceStream();
         spatial.open(-1);
      }

      private IObservable<AccelerometerFrame_raw> SetupDeviceStream()
      {
         var stream =
            (from evt in spatialEvents
             let e = evt.EventArgs
             select new AccelerometerFrame_raw
             (
                accX: e.spatialData[0].Acceleration[0],
                accY: e.spatialData[0].Acceleration[1],
                accZ: e.spatialData[0].Acceleration[2],
                rotX: e.spatialData[0].AngularRate[0],
                rotY: e.spatialData[0].AngularRate[1],
                rotZ: e.spatialData[0].AngularRate[2]
             )).Publish();
         stream.Connect();
         return stream;
      }

      private void spatial_Attach(object sender, AttachEventArgs e)
      {
         IsAttached = true;
      }

      private void spatial_Detach(object sender, DetachEventArgs e)
      {
         IsAttached = false;
      }

   }
}

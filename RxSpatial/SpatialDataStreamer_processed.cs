using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Phidgets.Events;

namespace RxSpatial
{
   public class SpatialDataStreamer_processed
   {
      public IObservable<AccelerometerFrame_processed> DeviceDataStream { get; private set; }
      private SpatialDataStreamer_raw rawStreamer = null;
      public SpatialDataStreamer_processed()
      {
         rawStreamer = SpatialDataStreamer_raw.Create(AccelerometerType.Phidgets1056_333);
         DeviceDataStream = SetupDeviceStream();
         DeviceDataStreamDebug = SetupDebugDeviceStream();
      }

      public IObservable<AccelerometerFrame_raw> DeviceDataStreamDebug { get; private set; }
      private IObservable<AccelerometerFrame_raw> SetupDebugDeviceStream()
      {
         var v = rawStreamer.DeviceDataStream;
         return v;
      }

      private IObservable<AccelerometerFrame_processed> SetupDeviceStream()
      {
         return rawStreamer.DeviceDataStream
            .Scan(new AccelerometerFrame_processed(),
            (AccelerometerFrame_processed prevProcessedFrame, AccelerometerFrame_raw newRawFrame) =>
            {
               return new AccelerometerFrame_processed(
                  newRawFrame, prevProcessedFrame);
            }
            );

      }

   }

}

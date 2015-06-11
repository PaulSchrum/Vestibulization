using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
//using System.Reactive;
using RxSpatial;

namespace RxSpatial.Streamers
{
   public class SpatialDataStreamer_processed
   {
      public IObservable<AccelerometerFrame_processed> DeviceDataStream { get; private set; }
      private SpatialDataStreamer_raw rawStreamer = null;
      public SpatialDataStreamer_processed(
         AccelerometerType accType = AccelerometerType.Phidgets1056_333,
         String dataFileName = null)
      {
         rawStreamer = SpatialDataStreamer_raw.Create(accType, dataFileName);
         DeviceDataStream = SetupDeviceStream();
         var v = DeviceDataStream;
         DeviceDataStreamDebug = SetupDebugDeviceStream();
      }

      public void Go_forRawFile()
      {
         if (rawStreamer is SpatialDataStreamer_rawFile)
            (rawStreamer as SpatialDataStreamer_rawFile).Go();
      }

      public IObservable<AccelerometerFrame_raw> DeviceDataStreamDebug { get; private set; }
      private IObservable<AccelerometerFrame_raw> SetupDebugDeviceStream()
      {
         var v = rawStreamer.DataStream;
         return v;
      }

      private IObservable<AccelerometerFrame_processed> SetupDeviceStream()
      {
         return rawStreamer.DataStream
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

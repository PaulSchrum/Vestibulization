using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RxSpatial.Streamers;
using RxSpatial;

namespace PrimaryUnitTests
{
   internal class ProcessedDataObserver :
      IObserver<AccelerometerFrame_processed>,
      IDisposable
   {
      public AccelerometerFrame_processed latestFrame;
      SpatialDataStreamer_processed accelStream;
      IDisposable streamSubscription;
      private Action whenComplete_ { get; set; }
      internal bool isComplete { get; set; }

      public ProcessedDataObserver(String sourceFileName, Action WhenComplete = null)
      {
         isComplete = false;
         whenComplete_ = WhenComplete;
         accelStream = new SpatialDataStreamer_processed(
            AccelerometerType.File, sourceFileName);
         streamSubscription = accelStream.DeviceDataStream.Subscribe(this);
      }

      public void OnCompleted()
      {
         isComplete = true;
         if(null != whenComplete_) whenComplete_();
      }

      public void OnError(Exception error)
      {
         throw new NotImplementedException();
      }

      public void OnNext(AccelerometerFrame_processed value)
      {
         latestFrame = value;
      }

      public void Dispose()
      {
         if (null == streamSubscription) return;
         streamSubscription.Dispose();
      }
   }
}

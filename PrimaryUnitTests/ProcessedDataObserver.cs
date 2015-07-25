using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RxSpatial.Streamers;
using RxSpatial;
using System.Diagnostics;

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
      public Stopwatch sw { get; protected set; }
      private List<timeFrame> timings { get; set; }

      public ProcessedDataObserver(String sourceFileName, Action WhenComplete = null)
      {
         timings = new List<timeFrame>();
         sw = new Stopwatch();
         isComplete = false;
         whenComplete_ = WhenComplete;
         accelStream = new SpatialDataStreamer_processed(
            AccelerometerType.File, sourceFileName);
         streamSubscription = accelStream.DeviceDataStream.Subscribe(this);
      }

      public void Go()
      {
         sw.Start();
         accelStream.Go_forRawFile();
      }

      public void OnCompleted()
      {
         timings.Add(new timeFrame(sw.Elapsed, "OnNext"));
         isComplete = true;
         if(null != whenComplete_) whenComplete_();
      }

      public void OnError(Exception error)
      {
         throw new NotImplementedException();
      }

      public void OnNext(AccelerometerFrame_processed value)
      {
         timings.Add(new timeFrame(sw.Elapsed, "OnNext " + value.ToString()));
         latestFrame = value;
      }

      public void Dispose()
      {
         if (null == streamSubscription) return;
         streamSubscription.Dispose();
      }

      private class timeFrame
      {
         public TimeSpan timespn { get; set; }
         public String comment { get; set; }
         public int order { get; set; }
         public timeFrame(TimeSpan ticks_, String comment_)
         {
            timespn = ticks_;
            comment = comment_;
            order = number++;
         }

         static int number = 0;
      }
   }

}

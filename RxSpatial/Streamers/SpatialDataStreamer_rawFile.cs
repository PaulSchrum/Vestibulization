using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reactive.Linq;
using System.Diagnostics;
using System.Timers;
using Microsoft.Reactive.Testing;
using System.Reactive;

namespace RxSpatial.Streamers
{
   public class SpatialDataStreamer_rawFile : SpatialDataStreamer_raw
   {
      internal List<AccelerometerFrame_raw> allFrames { get; set; }
      private Double ticksPerSecond_ { get; set; }
      private TestScheduler sched { get; set; }

      internal SpatialDataStreamer_rawFile(String filename)
      {
         if(null == filename) throw new ArgumentNullException("filename");
         if(!File.Exists(filename)) throw new FileNotFoundException(filename);
         var filesize = (new FileInfo(filename)).Length;
         allFrames = new List<AccelerometerFrame_raw>();

         using(var file = new StreamReader(filename))
         {
            long count = 0;
            String line;
            while((line = file.ReadLine()) != null)
            {
               if(count == 0)
               {
                  count++;
                  ticksPerSecond_ = Double.Parse(line.Split(',').FirstOrDefault().Trim());
                  AccelerometerFrame_raw.OverrideTicksPerSecond(ticksPerSecond_);
                  continue;
               }
               var parsedLine = line.Split(',').Select(s => s.Trim()).ToArray();
               //Make a new frame and add it to the list
               var newFrame = new AccelerometerFrame_raw(parsedLine);
               allFrames.Add(newFrame);
            }
         }
         if (allFrames == null) throw new Exception("member allFrames unexpectedly null.");
         if (allFrames.Count < 2) throw new Exception("member allFrames unexpectedly empty.");

         DataStream = SetupDeviceStream();
      }

      private IObservable<AccelerometerFrame_raw> SetupDeviceStream()
      {
         var framesArray = new Recorded<Notification<AccelerometerFrame_raw>>[allFrames.Count+1];
         int i = 0;
         long timeStamp = 0;
         foreach(var item in allFrames)
         {
            timeStamp += (long) (item.TimeStampSeconds * 1000.0);
            framesArray[i] = new Recorded<Notification<AccelerometerFrame_raw>>(
               timeStamp, 
               Notification.CreateOnNext(item));
            i++;
         }
         framesArray[i] = new Recorded<Notification<AccelerometerFrame_raw>>(
               timeStamp + 10, 
               Notification.CreateOnCompleted<AccelerometerFrame_raw>());

         sched = new TestScheduler();
         var preStream =sched.CreateColdObservable(framesArray);
         var stream = preStream.Publish();
         stream.Connect();
         return stream;
      }

      public void Go()
      {
         if (null != sched) sched.Start();
      }
   }
}

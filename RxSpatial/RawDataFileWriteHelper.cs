using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace RxSpatial
{
   public class RawDataFileWriteHelper :
      IObserver<AccelerometerFrame_raw>
   {
      private List<AccelerometerFrame_raw> allFrames { get; set; }
      RxSpatial.SpatialDataStreamer_raw2 accelerometerDataStream = null;
      IDisposable StreamSubscription { get; set; }
      private String filename_ { get; set; }
      private Timer recordingTimer_ { get; set; }
      private Timer startDelayTimer_ { get; set; }
      private Action recordingStateChanged_ { get; set; }

      private bool isRecording_;
      public bool isRecording
      {  get { return isRecording_; }
         private set { isRecording_ = value; this.recordingStateChanged_(); }
      }
      //Next: Make recording state appear on the dialog box
      //then make a recording
      //then make a class to read the recording and play it back

      public RawDataFileWriteHelper(String filename, TimeSpan startDelay, TimeSpan duration,
         Action recordingStateChanged = null)
      {
         isRecording = false;
         recordingStateChanged_ = recordingStateChanged;
         allFrames = new List<AccelerometerFrame_raw>();
         this.filename_ = filename;
         accelerometerDataStream = SpatialDataStreamer_raw2.Create();
         StreamSubscription =
            accelerometerDataStream.DeviceDataStream.Subscribe(this);
         startDelayTimer_ = new Timer(callback: _ => StartRecording(duration),
            state: null, dueTime: startDelay, period: Timeout.InfiniteTimeSpan);
      }

      public void StartRecording(TimeSpan duration)
      {
         isRecording = true;
         recordingTimer_ = new Timer(
            callback: _ => StopRecordingAndSave(this.filename_),
            state: null,
            dueTime: duration,
            period: System.Threading.Timeout.InfiniteTimeSpan
            );
      }

      public void StopRecordingAndSave(String filename)
      {
         StreamSubscription.Dispose();
         isRecording = false;

         if (allFrames.Count == 0)
            return;

         using (var file = new StreamWriter(filename))
         {
            foreach (var frame in allFrames)
            {
               file.WriteLine(frame.ToString());
            }
         }
         allFrames.Clear();
      }

      public void OnCompleted()
      {
         startDelayTimer_ = null;
         recordingTimer_ = null;
         StopRecordingAndSave(this.filename_);
      }

      public void OnError(Exception error)
      {
         throw new NotImplementedException("Exception: ", error);
      }

      public void OnNext(AccelerometerFrame_raw value)
      {
         if(isRecording)
            allFrames.Add(value);
      }
   }
}

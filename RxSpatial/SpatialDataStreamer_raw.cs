using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Phidgets;  //needed for the spatial class and the phidgets exception class
using Phidgets.Events;  //needed for the phidget event handling
using System.ComponentModel;
using System.Windows.Media.Media3D;
using System.IO;
using System.Diagnostics;


namespace RxSpatial
{
   public class SpatialDataStreamer_raw : IObservable<AccelerometerFrame_raw>,
      IDisposable
   {
      protected Spatial spatial=null;
      protected AccelerometerFrame_raw accelFrame_raw { get; set; }
      public SpatialData LastDataPoint { get; set; }
      //public List<SpatialData> SpatialDataList=new List<SpatialData>();  // move to child class later
      protected RunningStats RunningStat;
      protected Vector3D velocity;  // move to child class later
      protected Vector3D position;  // move to child class later

      //protected Tuning Tune_AccelMagnitude = new Tuning(aveStillVal: 0.9981376, squelchThreshold: 0.0007);
      //protected Tuning Tune_GyroX = new Tuning(aveStillVal: -0.66485, squelchThreshold: 0.22);
      //protected Tuning Tune_GyroY = new Tuning(aveStillVal: 0.0298, squelchThreshold: 0.17);
      //protected Tuning Tune_GyroZ = new Tuning(aveStillVal: 0.2774, squelchThreshold: 0.28);
      //protected Tuning Tune_GyroMagnitude = new Tuning(aveStillVal: 0.258, squelchThreshold: 0.17);

      public event EventHandler AttachedStateChanged;
      public event EventHandler CalibratingStateChanged;
      public event EventHandler WriteStateChanged;

      protected long cnt = 0;
 
      public SpatialDataStreamer_raw()
      {
         velocity = new Vector3D();
         position = new Vector3D();
         RunningStat = new RunningStats(10);
         //this.OptionText = "RAD Z";
         //this.OptionText = "Read/sec";
         //this.OptionText = "POS Z";
         this.OptionText = "Wrt State";
         this.IsAttached = false;
         outputFileStream = null;
         writeState = WriteState.Closed;
         spatial = new Spatial();
         spatial.close();
         spatial.Attach += new AttachEventHandler(spatial_Attach);
         spatial.Detach += new DetachEventHandler(spatial_Detach);
         accelEventHandler = new SpatialDataEventHandler(spatial_SpatialData_raw);
         spatial.SpatialData += accelEventHandler;
         spatial.open(-1);
      }

      protected Phidgets.Events.SpatialDataEventHandler accelEventHandler;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="observer"></param>
      /// <returns></returns>
      /// <see cref=">http://msdn.microsoft.com/en-us/library/dd782981(v=vs.110).aspx"/>
      public IDisposable Subscribe(IObserver<AccelerometerFrame_raw> observer)
      {
         if (!accelObservers.Contains(observer))
            accelObservers.Add(observer);
         return new Unsubscriber<AccelerometerFrame_raw>(accelObservers, observer);
      }
      private List<IObserver<AccelerometerFrame_raw>> accelObservers= new List<IObserver<AccelerometerFrame_raw>>();


      protected StreamWriter outputFileStream=null;
      protected Stopwatch runningTime;
      private WriteState writeState_;
      protected TimeSpan StartRecordingDelay;
      protected TimeSpan RecordingDuration;
      protected bool SaveAsCSV;
      public void SetPersistanceSettings(String FileName, TimeSpan startRecordingDelay,
         TimeSpan recordingDuration, bool saveAsCSV)
      {
         writeState = WriteState.NotOpenYet;
         if (FileName.Length > 0)
         {
            try
            {
               outputFileStream = new StreamWriter(FileName);
            }
            catch (Exception)
            {
               writeState = WriteState.Closed;
            }
         }
         SaveAsCSV = saveAsCSV;
         RecordingDuration = recordingDuration;
         StartRecordingDelay = startRecordingDelay;
         runningTime = new Stopwatch();
         writeState = WriteState.NotOpenYet;
         runningTime.Start();
      }

      private void spatial_Detach(object sender, DetachEventArgs e)
      {
         this.IsAttached = false;
      }

      protected void inTakeData(object sender, SpatialDataEventArgs e)
      {
         AccelX = AccelY = AccelZ = GyroX = GyroY = GyroZ = 0.0;
         this.LastDataPoint = new SpatialData(spatial, e);
         if (spatial.accelerometerAxes.Count > 0)
         {
            AccelX = e.spatialData[0].Acceleration[0];
            AccelY = e.spatialData[0].Acceleration[1];
            AccelZ = e.spatialData[0].Acceleration[2];
         }
         if (spatial.gyroAxes.Count > 0)
         {
            GyroX = e.spatialData[0].AngularRate[0];
            GyroY = e.spatialData[0].AngularRate[1];
            GyroZ = e.spatialData[0].AngularRate[2];
         }

         accelFrame_raw = new AccelerometerFrame_raw(
            AccelX,
            AccelY,
            AccelZ,
            GyroX,
            GyroY,
            GyroZ
            );
      }

      protected Double AccelX;
      protected Double AccelY;
      protected Double AccelZ;
      protected Double GyroX;
      protected Double GyroY;
      protected Double GyroZ;
      private void spatial_SpatialData_raw(object sender, SpatialDataEventArgs e)
      {
         inTakeData(sender, e);
         if (null == accelFrame_raw) return;
         //if (++cnt % 5 != 0) return;
         //if (SpatialDataList.Count < 100)
         //   this.SpatialDataList.Add(this.LastDataPoint);

         this.TotalAccel = Math.Sqrt(AccelX * AccelX + AccelY * AccelY + AccelZ * AccelZ);
         RunningStat.Add(AccelZ);


         updatePositionAndStuff(accelFrame_raw);
         foreach (var observer in this.accelObservers)
            observer.OnNext(accelFrame_raw);


         writeToFileIfNeeded();

         //if (RunningStat.RunningAverageDeviation.RunningAverage < 0.0)
         //   this.OptionValue = String.Format("{0:0.0000}", RunningStat.RunningAverageDeviation.RunningAverage);
         //else if (Math.Abs(RunningStat.RunningAverageDeviation.RunningAverage) < 0.00001)
         //   this.OptionValue = "  0.0000";
         //else
         //   this.OptionValue = String.Format("+{0:0.0000}", RunningStat.RunningAverageDeviation.RunningAverage);
         
         //this.cnt++;
         //if (this.seconds != DateTime.Now.Second)
         //{
         //   this.seconds = DateTime.Now.Second;
         //   this.OptionValue = this.cnt.ToString();
         //   this.cnt = 0;
         //}
      }
      // protected int seconds = 0;

      private void spatial_Attach(object sender, AttachEventArgs e)
      {
         this.IsAttached = true;
      }

      protected void updatePositionAndStuff(AccelerometerFrame_raw frame)
      {
         if (RunningStat.RunningAverageDeviation.RunningAverage > 0.002 ||
             RunningStat.RunningAverageDeviation.RunningAverage < -0.002)
         {
            velocity.X += frame.Acceleration.X;
            velocity.Y += frame.Acceleration.Y;
            velocity.Z += (1.0 - RunningStat.RunningAverageDeviation.RunningAverage) / 100;

            position.X += velocity.X;
            position.Y += velocity.Y;
            position.Z += velocity.Z;

            //this.OptionValue = position.Z.ToString();
            this.OptionValue = writeState.ToString();
            //this.OptionValue = RunningStat.RunningAverageDeviation.RunningAverage.ToString();
         }
         else
         {
            velocity.Z = 0;
         }
      }

      protected void writeToFileIfNeeded()
      {
         if (writeState == WriteState.Closed) return;
         if (this.RecordingDuration == TimeSpan.MinValue)
         {
            writeState = WriteState.Closed;
            return;
         }
         if(writeState == WriteState.NotOpenYet)
         {
            if (runningTime.Elapsed < this.StartRecordingDelay)
               return;
            writeState = WriteState.OpenNow;
         }
         if(runningTime.Elapsed > this.StartRecordingDelay + this.RecordingDuration)
         {
            writeState = WriteState.Closed;
            outputFileStream.Dispose();
            outputFileStream = null;
            return;
         }
         LastDataPoint.WriteToStreamAscii(outputFileStream);
      }

      public WriteState writeState
      {
         get { return writeState_; }
         set 
         { 
            writeState_ = value;
            OptionValue = writeState_.ToString();
            if (null == WriteStateChanged) return;
            WriteStateChanged(this, new WriteStateEventArg(writeState_));
         }
      }

      private bool isCalibrating_;
      public bool IsCalibrating
      {
         get { return isCalibrating_; }
         set
         {
            isCalibrating_ = value;
            if (null == CalibratingStateChanged) return;
            CalibratingStateChanged(this, new BooleanState(isCalibrating_));
         }
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

      private Double totalAccel_;
      public Double TotalAccel
      {
         get { return totalAccel_; }
         private set 
         {
            totalAccel_ = value;
         }
      }

      private String optionText_;
      public String OptionText
      {
         get { return optionText_; }
         set
         {
            optionText_ = value;
         }
      }

      private String optionValue_;
      public String OptionValue
      {
         get { return optionValue_; }
         set
         {
            optionValue_ = value;
         }
      }

       public void Dispose()
       {
          spatial.close();
          if (null != outputFileStream) outputFileStream.Dispose();
       }

       internal class Unsubscriber<T> : IDisposable
       {
          private List<IObserver<T>> _observers;
          private IObserver<T> _observer;

          public Unsubscriber(
             List<IObserver<T>> observers,
             IObserver<T> observer)
          {
             this._observers = observers;
             this._observer = observer;
          }

          public void Dispose()
          {
             if (_observer != null && _observers.Contains(_observer))
                _observers.Remove(_observer);
          }
       }

   }

   public enum WriteState
   {
      NotOpenYet,
      OpenNow,
      Closed
   }

   public static class WriteStateWhatever
   {
      public static String ToString(this WriteState ws)
      {
         if (ws == WriteState.Closed) return "Closed";
         if (ws == WriteState.NotOpenYet) return "Not Open Yet";
         if (ws == WriteState.OpenNow) return "Open Now";
         return "Err: Write State";
      }
   }

   public class BooleanState : EventArgs
   {
      internal BooleanState(bool isTrue_) { this.IsTrue = isTrue_; }
      public bool IsTrue;
   }

   public class WriteStateEventArg : EventArgs
   {
      internal WriteStateEventArg(WriteState arg)
         { this.WriteState = arg; }

      public WriteState WriteState;
   }

}

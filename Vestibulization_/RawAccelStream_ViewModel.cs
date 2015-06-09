using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RxSpatial;

namespace Vestibulization
{
   public class RawAccelStream_ViewModel : 
      IObserver<AccelerometerFrame_raw>
      , INotifyPropertyChanged, IDisposable
   {
      RxSpatial.SpatialDataStreamer_raw accelStream_raw=null;
      IDisposable StreamSubscription { get; set; }
      private RawDataFileWriteHelper dataStreamRecorder { get; set; }

      public RawAccelStream_ViewModel()
      {
         ctor_RawStream();
         //OptionGyro = "+";
         //dataStreamRecorder = new RawDataFileWriteHelper( filename:
         //   @"C:\Users\Paul\Documents\Life\Presentations\Acceleromoter to 3d mouse\dev\test1.csv",
         //   startDelay: TimeSpan.FromSeconds(1.5), duration: TimeSpan.FromSeconds(3.0),
         //   recordingStateChanged: handleRecordingStateChanged);
         
         //ctor_processedStream();
      }


      private void ctor_RawStream()
      {
         accelStream_raw = SpatialDataStreamer_raw.Create();
         StreamSubscription = 
            accelStream_raw
            .DeviceDataStream.Subscribe(this);
         accelStream_raw.AttachedStateChanged += AttachedStateChanged;
         accelStream_raw.CalibratingStateChanged += CalibratingStateChanged;
         OptionValue = "Raw 2";
      }

      //RxSpatial.SpatialDataStreamer_processed accelStream_processed = null;
      //private void ctor_processedStream()
      //{
      //   accelStream_processed = new SpatialDataStreamer_processed();
      //   StreamSubscription = accelStream_processed.DeviceDataStreamDebug.Subscribe(this);
      //   OptionValue = "proc";
      //}

      public void OnCompleted()
      {
         throw new NotImplementedException();
      }

      public void OnError(Exception error)
      {
         throw error;
      }

      public void 
         OnNext(AccelerometerFrame_raw accelFrame)
      {
         if (null == accelFrame) return;
         AccelX = accelFrame.Acceleration.X;
         AccelY = accelFrame.Acceleration.Y;
         AccelZ = accelFrame.Acceleration.Z;
         GyroX = accelFrame.RotationRate.X;
         GyroY = accelFrame.RotationRate.Y;
         GyroZ = accelFrame.RotationRate.Z;
      }

      private Double accelX_;
      public Double AccelX
      {
         get { return accelX_; }
         set
         {
            accelX_ = value;
            RaisePropertyChanged("AccelX");
         }
      }

      private Double accelY_;
      public Double AccelY
      {
         get { return accelY_; }
         set
         {
            accelY_ = value;
            RaisePropertyChanged("AccelY");
         }
      }

      private Double accelZ_;
      public Double AccelZ
      {
         get { return accelZ_; }
         set
         {
            accelZ_ = value;
            RaisePropertyChanged("AccelZ");
         }
      }

      private Double gyroX_;
      public Double GyroX
      {
         get { return gyroX_; }
         set
         {
            gyroX_ = value;
            if (gyroX_ > 360.0) gyroX_ = 0.0;
            if (gyroX_ < -360.0) gyroX_ = 0.0;
            RaisePropertyChanged("GyroX");
         }
      }

      private Double gyroY_;
      public Double GyroY
      {
         get { return gyroY_; }
         set
         {
            gyroY_ = value;
            if (gyroY_ > 360.0) gyroY_ = 0.0;
            if (gyroY_ < -360.0) gyroY_ = 0.0;
            RaisePropertyChanged("GyroY");
         }
      }

      private Double gyroZ_;
      public Double GyroZ
      {
         get { return gyroZ_; }
         set
         {
            gyroZ_ = value;
            if (gyroZ_ > 360.0) gyroZ_ = 0.0;
            if (gyroZ_ < -360.0) gyroZ_ = 0.0;
            RaisePropertyChanged("GyroZ");
         }
      }

      private Double compassX_;
      public Double CompassX
      {
         get { return compassX_; }
         set
         {
            compassX_ = value;
            RaisePropertyChanged("CompassX");
         }
      }

      private Double compassY_;
      public Double CompassY
      {
         get { return compassY_; }
         set
         {
            compassY_ = value;
            RaisePropertyChanged("CompassY");
         }
      }

      private Double compassZ_;
      public Double CompassZ
      {
         get { return compassZ_; }
         set
         {
            compassZ_ = value;
            RaisePropertyChanged("CompassZ");
         }
      }

      private Double totalAccel_;
      public Double TotalAccel
      {
         get { return totalAccel_; }
         private set
         {
            totalAccel_ = value;
            RaisePropertyChanged("TotalAccel");
         }
      }


      private String optionGyro_;
      public String OptionGyro
      {
         get { return optionGyro_; }
         set
         {
            optionGyro_ = value;
            RaisePropertyChanged("OptionGyro");
         }
      }

      private String optionText_;
      public String OptionText
      {
         get { return optionText_; }
         set
         {
            optionText_ = value;
            RaisePropertyChanged("OptionText");
         }
      }

      private String optionValue_;
      public String OptionValue
      {
         get { return optionValue_; }
         set
         {
            optionValue_ = value;
            RaisePropertyChanged("OptionValue");
         }
      }

      private void AttachedStateChanged(Object sender, EventArgs e)
      {
         if (null == e) return;
         bool isAttached = (e as BooleanState).IsTrue;
         if (true == isAttached)
            OptionText = "Is Attached.";
         else
            OptionText = "Not Attached.";
      }

      private void CalibratingStateChanged(Object sender, EventArgs e)
      {
         if (null == e) return;
         bool isCalibrating = (e as BooleanState).IsTrue;
         if (true == isCalibrating)
            OptionText = "Is Calibrating.";
         else
            OptionText = "Done Calibrating.";
      }

      private void handleRecordingStateChanged(bool isRecording)
      {
         if (isRecording == true) OptionGyro = "Now Writing";
         else OptionGyro = ".";
      }

      public event PropertyChangedEventHandler PropertyChanged;
      public void RaisePropertyChanged(String prop)
      {
         if (null != PropertyChanged)
         {
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
         }
      }

      public void Dispose()
      {
         if (null != StreamSubscription)
         {
            StreamSubscription.Dispose();
            StreamSubscription = null;
         }
         if (null != accelStream_raw)
         {
            accelStream_raw.AttachedStateChanged -= AttachedStateChanged;
            accelStream_raw.CalibratingStateChanged -= CalibratingStateChanged;
         }
      }
   }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using RxSpatial;

namespace Vestibulization
{
   public class ProcessedAccelStream_ViewModel : 
      IObserver<AccelerometerFrame_processed>
      , INotifyPropertyChanged, IDisposable
   {
      RxSpatial.SpatialDataStreamer_processed accelStream;
      IDisposable streamSubscription;

      public ProcessedAccelStream_ViewModel()
      {
         accelStream = new SpatialDataStreamer_processed();
         streamSubscription = accelStream.DeviceDataStream.Subscribe(this);
         //accelStream.AttachedStateChanged += AttachedStateChanged;
         //accelStream.CalibratingStateChanged += CalibratingStateChanged;
      }

      public void OnCompleted()
      {
         throw new NotImplementedException();
      }

      public void OnError(Exception error)
      {
         throw error;
      }

      public void OnNext(
         AccelerometerFrame_processed accelFrame)
      {
         if (null == accelFrame) return;
         AccelX = accelFrame.TrueAcceleration.X;
         AccelY = accelFrame.TrueAcceleration.Y;
         AccelZ = accelFrame.TrueAcceleration.Z;
         VelX = accelFrame.Velocity.X * g_secToMPH;
         VelY = accelFrame.Velocity.Y * g_secToMPH;
         VelZ = accelFrame.Velocity.Z * g_secToMPH;
         PosX = accelFrame.Position.X * g_sec2ToInches;
         PosY = accelFrame.Position.Y * g_sec2ToInches;
         PosZ = accelFrame.Position.Z * g_sec2ToInches;
         OrientX = accelFrame.Orientation.X;
         OrientY = accelFrame.Orientation.Y;
         OrientZ = accelFrame.Orientation.Z;
         TimeLapsed = accelFrame.SecondsSinceLast;
         FrameUsage = accelFrame.frameProcessingTimePercent;
         AverageFrameUsage = AccelerometerFrame_processed.AverageFrameProcessingTimePercent.RunningAverage;
      }

      private const Double g_secToMPH = 21.9368;
      private const Double g_sec2ToInches = 705.7978;

      private Double averageFrameUsage_;
      public Double AverageFrameUsage
      {
         get { return averageFrameUsage_; }
         set
         {
            averageFrameUsage_ = value;
            RaisePropertyChanged("AverageFrameUsage");
         }
      }

      private Double frameUsage_;
      public Double FrameUsage
      {
         get { return frameUsage_; }
         set
         {
            frameUsage_ = value;
            RaisePropertyChanged("FrameUsage");
         }
      }

      private Double timeLapsed_;
      public Double TimeLapsed
      {
         get { return timeLapsed_; }
         set
         {
            timeLapsed_ = value;
            RaisePropertyChanged("TimeLapsed");
         }
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

      private Double velX_;
      public Double VelX
      {
         get { return velX_; }
         set
         {
            velX_ = value;
            RaisePropertyChanged("VelX");
         }
      }

      private Double velY_;
      public Double VelY
      {
         get { return velY_; }
         set
         {
            velY_ = value;
            RaisePropertyChanged("VelY");
         }
      }

      private Double velZ_;
      public Double VelZ
      {
         get { return velZ_; }
         set
         {
            velZ_ = value;
            RaisePropertyChanged("VelZ");
         }
      }

      private Double posX_;
      public Double PosX
      {
         get { return posX_; }
         set
         {
            posX_ = value;
            RaisePropertyChanged("PosX");
         }
      }

      private Double posY_;
      public Double PosY
      {
         get { return posY_; }
         set
         {
            posY_ = value;
            RaisePropertyChanged("PosY");
         }
      }

      private Double posZ_;
      public Double PosZ
      {
         get { return posZ_; }
         set
         {
            posZ_ = value;
            RaisePropertyChanged("PosZ");
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

      private Double orientX_;
      public Double OrientX
      {
         get { return orientX_; }
         set
         {
            orientX_ = value;
            RaisePropertyChanged("OrientX");
         }
      }

      private Double orientY_;
      public Double OrientY
      {
         get { return orientY_; }
         set
         {
            orientY_ = value;
            RaisePropertyChanged("OrientY");
         }
      }

      private Double orientZ_;
      public Double OrientZ
      {
         get { return orientZ_; }
         set
         {
            orientZ_ = value;
            RaisePropertyChanged("OrientZ");
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
         if (null != streamSubscription)
            streamSubscription.Dispose();
      }
   }
}


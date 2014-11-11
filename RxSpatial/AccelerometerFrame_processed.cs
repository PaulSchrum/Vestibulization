using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace RxSpatial
{
   public class AccelerometerFrame_processed : AccelerometerFrame_raw
   {
      public AccelerometerFrame_processed() : base(0,0,0,0,0,0)
      { }

      public AccelerometerFrame_processed(
            AccelerometerFrame_raw currentRawFrame,
            AccelerometerFrame_processed previousFrame
            ) : base(currentRawFrame)
      {
         if (null == currentRawFrame) 
            throw new NullReferenceException("currentRawFrame");

         this.SecondsSinceLast = 
            currentRawFrame.TimeStampSeconds - 
            previousFrame.TimeStampSeconds;

         if (false == mayProceed(currentRawFrame)) return;
         this.g = averageG;
         this.Acceleration = currentRawFrame.Acceleration;
         setTrueAcceleration(currentRawFrame);
         if(null == previousFrame)
         {
            this.Velocity = new Vector3D(
               x: TrueAcceleration.X, 
               y: TrueAcceleration.Y, 
               z: TrueAcceleration.Z);
            this.Position = new Vector3D(0.0, 0.0, 0.0);
            this.Orientation = new Vector3D(0.0, 0.0, 0.0);
         }
         else
         {
            this.Velocity = new Vector3D(
               x: previousFrame.Velocity.X + TrueAcceleration.X * SecondsSinceLast,
               y: previousFrame.Velocity.Y + TrueAcceleration.Y * SecondsSinceLast,
               z: previousFrame.Velocity.Z + TrueAcceleration.Z * SecondsSinceLast);
            this.Position = new Vector3D(
               x: previousFrame.Position.X + this.Velocity.X * SecondsSinceLast,
               y: previousFrame.Position.Y + this.Velocity.Y * SecondsSinceLast,
               z: previousFrame.Position.Z + this.Velocity.Z * SecondsSinceLast);
            this.Orientation = new Vector3D(
               x: previousFrame.Orientation.X + (currentRawFrame.RotationRate.X - averageRotationRate.X) * SecondsSinceLast,
               y: previousFrame.Orientation.Y + (currentRawFrame.RotationRate.Y - averageRotationRate.Y) * SecondsSinceLast,
               z: previousFrame.Orientation.Z + (currentRawFrame.RotationRate.Y - averageRotationRate.Z) * SecondsSinceLast
               );
         }
         var frameProcessingTime =
            (AccelerometerFrame_raw.stopwatch.ElapsedTicks - currentRawFrame.TimeStampTicks);
         frameProcessingTimePercent = (100.0 * frameProcessingTime) /
            (SecondsSinceLast * ticksPerSecond);
         AverageFrameProcessingTimePercent.Add(frameProcessingTimePercent);
      }

      private void setTrueAcceleration(AccelerometerFrame_raw currentRawFrame)
      {
         AdjustGdirectionForRotation();
         this.TrueAcceleration = currentRawFrame.Acceleration - this.g;
      }

      private void AdjustGdirectionForRotation()
      {
         if(false == InvariableGvector)
         {
            // todo: add a bunch of code here
         }
      }

      /// <summary>
      /// Force of gravity.
      /// </summary>
      public Vector3D g { get; internal set; }

      /// <summary>
      /// Acceleration sensed by the accelerometer with g taken out.
      /// </summary>
      public Vector3D TrueAcceleration { get; internal set; }
      public Vector3D Velocity { get; internal set; }
      public Vector3D Position { get; internal set; }
      public Double SecondsSinceLast { get; internal set; }
      public Vector3D Orientation { get; internal set; }
      public Double frameProcessingTimePercent { get; internal set; }

      public void SetGravityVectorWhileStill(Vector3D newG)
      {
         g = newG;
      }

      public static bool InvariableGvector { get; set; }

      private enum InitState
      {
         SettingUp = 0,
         GettingAverageG = 1,
         CleaningUp = 2,
         Done = 3,
      }
      private static InitState initState_ = InitState.SettingUp;

      public static RunningStats AverageFrameProcessingTimePercent { get; internal set; }
      private static RunningStats runningAverageAccelX;
      private static RunningStats runningAverageAccelY;
      private static RunningStats runningAverageAccelZ;
      private static Vector3D averageG;
      private static RunningStats rotRateAboutX;
      private static RunningStats rotRateAboutY;
      private static RunningStats rotRateAboutZ;
      private static Vector3D averageRotationRate;
      private static Double startingTimeStampSeconds_;
      private static bool mayProceed(AccelerometerFrame_raw aRawFrame)
      {
         switch(initState_)
         {
            case InitState.SettingUp:
            {
               startingTimeStampSeconds_ = aRawFrame.TimeStampSeconds;
               runningAverageAccelX = new RunningStats(Int32.MaxValue, false);
               runningAverageAccelY = new RunningStats(Int32.MaxValue, false);
               runningAverageAccelZ = new RunningStats(Int32.MaxValue, false);
               rotRateAboutX = new RunningStats(Int32.MaxValue, false);
               rotRateAboutY = new RunningStats(Int32.MaxValue, false);
               rotRateAboutZ = new RunningStats(Int32.MaxValue, false);
               AverageFrameProcessingTimePercent = new RunningStats(120, false);
               initState_ = InitState.GettingAverageG;
               return false;
            }
            case InitState.GettingAverageG:
            {
               runningAverageAccelX.Add(aRawFrame.Acceleration.X);
               runningAverageAccelY.Add(aRawFrame.Acceleration.Y);
               runningAverageAccelZ.Add(aRawFrame.Acceleration.Z);
               rotRateAboutX.Add(aRawFrame.RotationRate.X);
               rotRateAboutY.Add(aRawFrame.RotationRate.Y);
               rotRateAboutZ.Add(aRawFrame.RotationRate.Z);
               if ((aRawFrame.TimeStampSeconds - startingTimeStampSeconds_) > 0.3)
               {
                  averageG = new Vector3D(
                     runningAverageAccelX.RunningAverage,
                     runningAverageAccelY.RunningAverage,
                     runningAverageAccelZ.RunningAverage
                     );

                  averageRotationRate = new Vector3D(
                     rotRateAboutX.RunningAverage,
                     rotRateAboutY.RunningAverage,
                     rotRateAboutZ.RunningAverage
                     );

                  initState_ = InitState.CleaningUp;
                  return true;
               }
               return false;
            }
            case InitState.CleaningUp:
            {
               runningAverageAccelX = null;
               runningAverageAccelY = null;
               runningAverageAccelZ = null;
               rotRateAboutX = null;
               rotRateAboutY = null;
               rotRateAboutZ = null;
               initState_ = InitState.Done;
               return true;
            }
            case InitState.Done:
            {
               break;
            }
         }
         return true;
      }
   }
}

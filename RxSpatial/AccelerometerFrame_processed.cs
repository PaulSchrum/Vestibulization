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
      internal AccelerometerFrame_processed(
            AccelerometerFrame_raw currentRawFrame,
            AccelerometerFrame_processed previousFrame
            ) : base(currentRawFrame)
      {
         if (null == currentRawFrame) 
            throw new NullReferenceException("currentRawFrame");

         this.Acceleration = currentRawFrame.Acceleration;
         setTrueAcceleration(currentRawFrame);
         if(null == previousFrame)
         {
            this.Velocity = new Vector3D(
               x: TrueAcceleration.X, 
               y: TrueAcceleration.Y, 
               z: TrueAcceleration.Z);
            this.Position = new Vector3D(0.0, 0.0, 0.0);
         }
         else
         {
            this.Velocity = new Vector3D(
               x: previousFrame.Velocity.X + TrueAcceleration.X,
               y: previousFrame.Velocity.Y + TrueAcceleration.Y,
               z: previousFrame.Velocity.Z + TrueAcceleration.Z);
            this.Position = new Vector3D(
               x: previousFrame.Position.X + this.Velocity.X,
               y: previousFrame.Position.Y + this.Velocity.Y,
               z: previousFrame.Position.Z + this.Velocity.Z);
         }
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
      public Vector3D TrueAcceleration { get; set; }
      public Vector3D Velocity { get; internal set; }
      public Vector3D Position { get; internal set; }

      public void SetGravityVectorWhileStill(Vector3D newG)
      {
         g = newG;
      }

      public static bool InvariableGvector { get; set; }

   }
}

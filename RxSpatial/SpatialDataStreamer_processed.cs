using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phidgets.Events;

namespace RxSpatial
{
   public class SpatialDataStreamer_processed : 
      SpatialDataStreamer_raw, 
      IObservable<AccelerometerFrame_processed>
   {
      public SpatialDataStreamer_processed() : base()
      {
         spatial.SpatialData -= base.accelEventHandler;
         accelEventHandler = new SpatialDataEventHandler(spatial_SpatialData_processed);
         spatial.SpatialData += accelEventHandler;
      }

      protected AccelerometerFrame_processed previousProcessedFrame = null;
      protected AccelerometerFrame_processed currentProcessedFrame;

      public IDisposable Subscribe(IObserver<AccelerometerFrame_processed> observer)
      {
         if (!accelObservers.Contains(observer))
            accelObservers.Add(observer);
         return new Unsubscriber<AccelerometerFrame_processed>(accelObservers, observer);
      }
      private List<IObserver<AccelerometerFrame_processed>> accelObservers =
         new List<IObserver<AccelerometerFrame_processed>>();

      private void spatial_SpatialData_processed(object sender, SpatialDataEventArgs e)
      {
         inTakeData(sender, e);
         currentProcessedFrame =
            new AccelerometerFrame_processed(
               base.accelFrame_raw,
               this.previousProcessedFrame);

         foreach (var observer in this.accelObservers)
            observer.OnNext(currentProcessedFrame);

         previousProcessedFrame = currentProcessedFrame;
      }
   }
}

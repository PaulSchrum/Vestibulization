using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace RxSpatial
{
   public static class IEnumerableExtensionMethods
   {
      public static IObservable<T> AsObservable<T>(
         this IEnumerable<T> source,
         Func<T, TimeSpan> scheduleNextFrame,
         Action notifyWhenReady,
         bool useAbsoluteScheduleTime = true
         ) where T : class 
      {
         var sourceStack = source.ToStack();
         return ObservableFromIEnumerable<T>.Create(
            source.ToStack(),
            scheduleNextFrame,
            notifyWhenReady,
            useAbsoluteScheduleTime
            );
      }

      public static Stack<T> ToStack<T>(this IEnumerable<T> source)
      {
         return new Stack<T>(source.Reverse());
      }
   }

   public class ObservableFromIEnumerable<T> : IObservable<T>, IDisposable
      where T : class
   {
      private ObservableFromIEnumerable() { }
      internal static ObservableFromIEnumerable<T>Create(Stack<T> sourceStack,
            Func<T, TimeSpan> scheduleNextFrame,
            Action notifyWhenReady,
            bool useAbsoluteScheduleTime = true
         )
      {
         if (null == notifyWhenReady) throw new ArgumentNullException("notifyWhenReady");
         ObservableFromIEnumerable<T> retValue = new ObservableFromIEnumerable<T>();

         retValue.dataToSend = sourceStack;
         retValue.scheduleNextAt = scheduleNextFrame;
         retValue.absoluteTime = useAbsoluteScheduleTime;
         retValue.nextFrame = retValue.dataToSend.Pop();
         retValue.notifyReady = notifyWhenReady;

         retValue.nextFrameTimer = new Timer();
         retValue.nextFrameTimer.Interval = 0;
         retValue.nextFrameTimer.Elapsed += GoRecurse;

         retValue.readyTimer = new Timer();
         retValue.readyTimer.Interval = 5;
         retValue.readyTimer.Elapsed += new ElapsedEventHandler(retValue.notifyThem);

         return retValue;
      }

      protected Stack<T> dataToSend { get; set; }
      protected Func<T, TimeSpan> scheduleNextAt { get; set; }
      protected bool absoluteTime { get; set; }
      protected Action notifyReady { get; set; }
      protected Timer readyTimer { get; set; }
      protected Timer nextFrameTimer { get; set; }
      protected T nextFrame { get; set; }
      protected IObserver<T> onlySubscriber { get; set; }

      public IDisposable Subscribe(IObserver<T> subscriber)
      {
         onlySubscriber = subscriber;
         return this;
      }

      protected void notifyThem(object sender, ElapsedEventArgs e)
      {
         readyTimer.Enabled = false;
         notifyReady();
      }

      private static void GoRecurse(object sender, ElapsedEventArgs e)
      {
         var this_ = sender as ObservableFromIEnumerable<T>;
         this_.Go();
      }

      public void Go()
      {
         if (this.nextFrame == null)
         {
            nextFrameTimer.Enabled = false;
            nextFrameTimer.Elapsed -= GoRecurse;
            nextFrameTimer = null;
            this.onlySubscriber.OnCompleted();
            return;
         }

         //nextDelay is delay time to the next frame issuance
         var nextDelay = scheduleNextAt(this.nextFrame);
         this.onlySubscriber.OnNext(this.nextFrame);
         this.nextFrame = dataToSend.Pop();
         nextFrameTimer.Elapsed -= GoRecurse;
         nextFrameTimer = new Timer();
         nextFrameTimer.Interval = nextDelay.Milliseconds;
         nextFrameTimer.Elapsed += GoRecurse;
      }

      public void Dispose()
      {
         if(null != nextFrameTimer)
         {
            nextFrameTimer.Elapsed -= GoRecurse;
            nextFrameTimer = null;
         }
         if(null != this.dataToSend)
         {
            dataToSend.Clear();
            dataToSend = null;
         }
      }
   }

}

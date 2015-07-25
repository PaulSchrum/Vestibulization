using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Hardware;
using Android.Content;
using System.Collections.Generic;

using MotionSense;
using AccelViewer;

namespace AccelViewer.Droid
{
   [Activity(Label = "AccelViewer", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
   public class MainActivity : 
      global::Xamarin.Forms.Platform.Android.FormsApplicationActivity,
      ISensorEventListener,
      ISelfMotionSensor
   {
      
      public event VectorEventFire OnAcclerometerFire;
      public event VectorEventFire OnGyroscopeFire;
      public event VectorEventFire OnMagnetometerFire;
      private SensorManager snsMgr { get; set; }
      private static readonly object _syncLock = new object();
      private AccelViewer.App theApp { get; set; }

      protected override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);

         global::Xamarin.Forms.Forms.Init(this, bundle);
         theApp = new App();
         LoadApplication(theApp);
         theApp.RegisterSensors(this);
         snsMgr = (SensorManager)GetSystemService(Context.SensorService);

         //List<String> sensorsBaby = new List<String>();
         //var v = snsMgr.GetSensorList(SensorType.All);
         //foreach (var ssr in v)
         //{
         //   sensorsBaby.Add(ssr.Name);
         //}
      }

      //protected override void OnCreate(Bundle bundle)
      //{
      //   base.OnCreate(bundle);
      //   SetContentView(Resource.Layout.Main);
      //   _sensorManager = (SensorManager)GetSystemService(Context.SensorService);
      //   _sensorTextView = FindViewById<TextView>(Resource.Id.accelerometer_text);
      //}

      protected override void OnResume()
      {
         base.OnResume();
         snsMgr.RegisterListener(this, snsMgr.GetDefaultSensor(SensorType.Accelerometer), SensorDelay.Normal);
         snsMgr.RegisterListener(this, snsMgr.GetDefaultSensor(SensorType.Gyroscope), SensorDelay.Normal);
         snsMgr.RegisterListener(this, snsMgr.GetDefaultSensor(SensorType.MagneticField), SensorDelay.Normal);
      }

      protected override void OnPause()
      {
         base.OnPause();
         snsMgr.UnregisterListener(this);
      }

      public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum]SensorStatus accuracy)
      {
         // nothing needed here
      }

      private int i = 0;
      public void OnSensorChanged(SensorEvent e)
      {
         lock (_syncLock)
         {
            i++;
            if (e.Sensor.Type == SensorType.Accelerometer)
            {
               if (null != OnAcclerometerFire)
                  OnAcclerometerFire(this,
                     new VectorFrameEventArgs(
                        e.Values[0], e.Values[1], e.Values[2]));
            }
            else if (e.Sensor.Type == SensorType.Gyroscope)
            {
               if (null != this.OnGyroscopeFire)
                  OnGyroscopeFire(this,
                     new VectorFrameEventArgs(
                        e.Values[0], e.Values[1], e.Values[2]));
            }
            else if (e.Sensor.Type == SensorType.MagneticField)
            {
               if (null != this.OnMagnetometerFire)
                  OnMagnetometerFire(this,
                     new VectorFrameEventArgs(
                        e.Values[0], e.Values[1], e.Values[2]));
            }
            //var text = new StringBuilder("x = ")
            //    .Append(e.Values[0])
            //    .Append(", y=")
            //    .Append(e.Values[1])
            //    .Append(", z=")
            //    .Append(e.Values[2]);
            //_sensorTextView.Text = text.ToString();
         }
      }
   }

}


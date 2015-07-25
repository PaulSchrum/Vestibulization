using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MotionSense;
using Xamarin.Forms;

using MotionSense;
using GeometryCore.Lines;

namespace AccelViewer
{
   public class App : Application
   {
      internal ISelfMotionSensor sensorSource { get; set; }
      protected Label AccelX { get; set; }
      protected Label AccelY { get; set; }
      protected Label AccelZ { get; set; }
      protected Label GyroX { get; set; }
      protected Label GyroY { get; set; }
      protected Label GyroZ { get; set; }
      protected Label MagX { get; set; }
      protected Label MagY { get; set; }
      protected Label MagZ { get; set; }

      public App()
      {
         // The root page of your application
         MainPage = initMainPage();
      }

      protected Xamarin.Forms.ContentPage initMainPage()
      {
         AccelX = new Label
         {
            XAlign = TextAlignment.Start,
            Text = "Welcome to AccelViewer",
            FontSize = 20
         };

         AccelY = new Label
         {
            XAlign = TextAlignment.Start,
            Text = "Welcome to AccelViewer",
            FontSize = 20
         };

         AccelZ = new Label
         {
            XAlign = TextAlignment.Start,
            Text = "Welcome to AccelViewer",
            FontSize = 20
         };

         GyroX = new Label
         {
            XAlign = TextAlignment.Start,
            Text = "Welcome to AccelViewer",
            FontSize = 20
         };

         GyroY = new Label
         {
            XAlign = TextAlignment.Start,
            Text = "Welcome to AccelViewer",
            FontSize = 20
         };

         GyroZ = new Label
         {
            XAlign = TextAlignment.Start,
            Text = "Welcome to AccelViewer",
            FontSize = 20
         };

         MagX = new Label
         {
            XAlign = TextAlignment.Start,
            Text = "Welcome to AccelViewer",
            FontSize = 20
         };

         MagY = new Label
         {
            XAlign = TextAlignment.Start,
            Text = "Welcome to AccelViewer",
            FontSize = 20
         };

         MagZ = new Label
         {
            XAlign = TextAlignment.Start,
            Text = "Welcome to AccelViewer",
            FontSize = 20
         };

         return new ContentPage
         {
            Content = new StackLayout
            {
               VerticalOptions = LayoutOptions.Center,
               Children = {AccelX, AccelY, AccelZ,
                  GyroX, GyroY, GyroZ,
                  MagX, MagY, MagZ
               }
            }

         };

      }

      public void PutTextAccelX(String newText)
      {
         AccelX.Text = newText;
      }

      protected override void OnStart()
      {
         // Handle when your app starts
      }

      protected override void OnSleep()
      {
         // Handle when your app sleeps
      }

      protected override void OnResume()
      {
         // Handle when your app resumes
      }

      public void RegisterSensors(ISelfMotionSensor s)
      {
         this.sensorSource = s;
         sensorSource.OnAcclerometerFire += HandleAccelerometerFrame;
         sensorSource.OnGyroscopeFire += HandleGyroscopeFrame;
         sensorSource.OnMagnetometerFire += HandleMegnetometerFrame;
      }

      private void HandleAccelerometerFrame(object source, VectorFrameEventArgs e)
      {
         AccelX.Text = "Accel x = " + e.Frame.x.ToString();
         AccelY.Text = "Accel y = " + e.Frame.y.ToString();
         AccelZ.Text = "Accel z = " + e.Frame.z.ToString();
      }

      private void HandleGyroscopeFrame(object source, VectorFrameEventArgs e)
      {
         GyroX.Text = "Gyro x = " + e.Frame.x.ToString();
         GyroY.Text = "Gyro y = " + e.Frame.y.ToString();
         GyroZ.Text = "Gyro z = " + e.Frame.z.ToString();
      }

      private void HandleMegnetometerFrame(object source, VectorFrameEventArgs e)
      {
         MagX.Text = "Magnet x = " + e.Frame.x.ToString();
         MagY.Text = "Magnet y = " + e.Frame.y.ToString();
         MagZ.Text = "Magnet z = " + e.Frame.z.ToString();
      }
   }
}

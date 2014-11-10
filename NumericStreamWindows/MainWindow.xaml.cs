using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Vestibulization
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      private ProcessedData_Window processedDataWindow = null;
      public MainWindow()
      {
         InitializeComponent();
         this.WindowState = WindowState.Minimized;

         processedDataWindow = new ProcessedData_Window();
         processedDataWindow.Show();
      }

      private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
      {
         if (null != processedDataWindow)
            processedDataWindow.Close();

         var DataStream = (IDisposable)this.DataContext;
         if (null == DataStream) return;
         DataStream.Dispose();
         DataStream = null;
      }

      private void mnu_OpenProcessedDataStream_Click(object sender, RoutedEventArgs e)
      {
         processedDataWindow = new ProcessedData_Window();
         processedDataWindow.Show();
      }


   }
}
         //var dc = this.TopGrid.DataContext as RawAccelStream_ViewModel;
         //if (DesignerProperties.GetIsInDesignMode(this) == false)
         //   base.Background = Brushes.Bisque;
         //else
         //{
         //   base.Background = Brushes.Blue;
         //   dc.Dispose();
         //}
         //dc.SetPersistanceSettings(
         //   @"C:\SourceModules\ComputerVision\Vestibulization\misc\MotionLess_YisDown.csv",
         //   TimeSpan.FromSeconds(1.2), TimeSpan.FromSeconds(10), true);
      //}


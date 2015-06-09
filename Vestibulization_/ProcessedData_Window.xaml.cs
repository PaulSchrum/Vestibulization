using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Vestibulization
{
   /// <summary>
   /// Interaction logic for ProcessedData.xaml
   /// </summary>
   public partial class ProcessedData_Window : Window
   {
      public ProcessedData_Window()
      {
         InitializeComponent();
      }

      private void Window_Closed(object sender, EventArgs e)
      {
         Application.Current.Shutdown();
      }
   }
}

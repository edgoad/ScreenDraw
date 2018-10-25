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

namespace ScreenDraw
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }
        private void btnUndo(object sender, RoutedEventArgs e)
        {
            int numStrokes = this.inkCanvas1.Strokes.Count();
            if (numStrokes >= 1)
            {
                this.inkCanvas1.Strokes.RemoveAt(numStrokes - 1);
            }
        }
        void App_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //this.Close();
        }

    }
}

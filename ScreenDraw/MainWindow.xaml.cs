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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Drawing.Imaging;

namespace ScreenDraw
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Window1 window1 = new Window1();
        System.Windows.Forms.Screen[] screens = System.Windows.Forms.Screen.AllScreens;
        System.Windows.Forms.Screen selectedScreen;


        public MainWindow()
        {
            InitializeComponent();
            foreach (var screen in screens)
            {
                MenuItem myMenuItem = new MenuItem();
                myMenuItem.Header = screen.DeviceName;
                myMenuItem.IsCheckable = true;
                if (screen.DeviceName == Properties.Settings.Default.monitorName)
                    myMenuItem.IsChecked = true;

                 mnuMonitors.Items.Add(myMenuItem);
           }
            MouseDown += Window_MouseDown;

            SelectScreen();
        }
        // Allow borderless movement
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        // select screens to use
        private void MonitorSelected(object sender, RoutedEventArgs e) {
            // code to select monitor here
            //MenuItem menuItem = (MenuItem)e.OriginalSource;
            //menuItem.IsChecked = true;

            //Properties.Settings.Default.monitorName = (MenuItem)e.OriginalSource.

            foreach (MenuItem menuItem in mnuMonitors.Items)
            {
                if (menuItem == e.OriginalSource)
                {
                    menuItem.IsChecked = true;
                    string itemName = menuItem.Header.ToString();
                    Properties.Settings.Default.monitorName = itemName;
                    Properties.Settings.Default.Save();
                    //itemName = itemName.Replace("\\\\.\\DISPLAY", "");
                    //int itemNum = Convert.ToInt32(itemName) - 1;
                    //selectedScreen = screens[itemNum];
                    SelectScreen();
                }
                else
                    menuItem.IsChecked = false;
            }


        }
        private void SelectScreen(string displayName)
        {
            string itemName = displayName;
            itemName = itemName.Replace("\\\\.\\DISPLAY", "");
            int itemNum = Convert.ToInt32(itemName) - 1;
            selectedScreen = screens[itemNum];
        }
        private void SelectScreen()
        {
            string itemName = Properties.Settings.Default.monitorName;
            itemName = System.Text.RegularExpressions.Regex.Match(itemName, @"\d+").Value;
//            itemName = itemName.Replace("\\\\.\\DISPLAY", "");    // replaced with regex above
            int itemNum = Convert.ToInt32(itemName) - 1;
            selectedScreen = screens[itemNum];
        }

        // take screenshot and open canvas
        private void openCanvas(object sender, RoutedEventArgs e)
        {
            // setup dimensions for target screen
            //System.Windows.Forms.Screen targetScreen = screens[cmbScreens.SelectedIndex];
            window1.WindowState = WindowState.Normal;
            window1.Left = selectedScreen.Bounds.Left;
            window1.Top = selectedScreen.Bounds.Top;
            window1.Width = selectedScreen.Bounds.Width;
            window1.Height = selectedScreen.Bounds.Height;
            //window1.Left = targetScreen.WorkingArea.Left;
            //window1.Top = targetScreen.WorkingArea.Top;
            //window1.Width = targetScreen.WorkingArea.Width;
            //window1.Height = targetScreen.WorkingArea.Height;

            //capture screen and put as background
            System.IO.Stream myImg = takeScreenshot();
            BitmapImage myBitMap = new BitmapImage();
            myBitMap.BeginInit();
            myBitMap.StreamSource = myImg;
            myBitMap.EndInit();


            // display to canvas
            //window1.inkCanvas1.Background = new BitmapImage(new Uri(@"c:\temp\foo.png",));
            //window1.inkCanvas1.Background = new ImageBrush(new BitmapImage(new Uri(@"c:\temp\snap.png")));
            window1.inkCanvas1.Background = new ImageBrush(myBitMap);

            //open ink on target scree and maximize
            window1.SourceInitialized += (snd, arg) => window1.WindowState = WindowState.Maximized;
            window1.Show();
            //window1.Loaded += MaximizeWindow;
        }
        private System.IO.Stream takeScreenshot()
        {
            System.IO.MemoryStream myImg = new System.IO.MemoryStream();

            //System.Windows.Forms.Screen targetScreen = screens[cmbScreens.SelectedIndex];
            int screenLeft = selectedScreen.Bounds.Left;
            int screenTop = selectedScreen.Bounds.Top;
            int screenWidth = selectedScreen.Bounds.Width;
            int screenHeight = selectedScreen.Bounds.Height;
            //int screenLeft = targetScreen.WorkingArea.Left;
            //int screenTop = targetScreen.WorkingArea.Top;
            //int screenWidth = targetScreen.WorkingArea.Width;
            //int screenHeight = targetScreen.WorkingArea.Height;

            using (Bitmap bmp = new Bitmap(screenWidth, screenHeight))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(screenLeft, screenTop, 0, 0, bmp.Size);
                }
                //bmp.Save(@"c:\temp\snap.png", ImageFormat.Png);
                bmp.Save(myImg, ImageFormat.Png);
            }
            return myImg;
        }

        // Setup buttons
        private void Ink(object sender, RoutedEventArgs e)
        {
            window1.inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;

            // Set the DefaultDrawingAttributes for a red pen.
            window1.inkCanvas1.DefaultDrawingAttributes.Color = Colors.Red;
            window1.inkCanvas1.DefaultDrawingAttributes.IsHighlighter = false;
            window1.inkCanvas1.DefaultDrawingAttributes.Height = 2;
            window1.inkCanvas1.DefaultDrawingAttributes.Width = 2;
        }
        // Set the EditingMode to highlighter input.
        private void Highlight(object sender, RoutedEventArgs e)
        {
            window1.inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;

            // Set the DefaultDrawingAttributes for a highlighter pen.
            window1.inkCanvas1.DefaultDrawingAttributes.Color = Colors.Yellow;
            window1.inkCanvas1.DefaultDrawingAttributes.IsHighlighter = true;
            window1.inkCanvas1.DefaultDrawingAttributes.Height = 25;
        }
        // Set the EditingMode to erase by stroke.
        private void EraseStroke(object sender, RoutedEventArgs e)
        {
            window1.inkCanvas1.EditingMode = InkCanvasEditingMode.EraseByStroke;
        }
        // Set the EditingMode to selection.
        private void Select(object sender, RoutedEventArgs e)
        {
            window1.inkCanvas1.EditingMode = InkCanvasEditingMode.Select;
        }
        private void btnUndo(object sender, RoutedEventArgs e)
        {
            int numStrokes = window1.inkCanvas1.Strokes.Count();
            if (numStrokes >= 1)
            {
                window1.inkCanvas1.Strokes.RemoveAt(numStrokes - 1);
            }
        }
        private void btnClose(object sender, RoutedEventArgs e)
        {
            window1.Close();
            window1 = null;
            window1 = new Window1();
            //window1.Hide();
            //window1.inkCanvas1.Background = System.Windows.Media.Brushes.Ivory;
            //window1.inkCanvas1.Background = null;
            //if (System.IO.File.Exists(@"c:\temp\snap.png"))
            //    System.IO.File.Delete(@"c:\temp\snap.png");
        }

        // choose colors
        //private void clrRed(object sender, RoutedEventArgs e)
        //{
        //    window1.inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;

        //    // Set the DefaultDrawingAttributes for a red pen.
        //    window1.inkCanvas1.DefaultDrawingAttributes.Color = Colors.Red;
        //    //window1.inkCanvas1.DefaultDrawingAttributes.IsHighlighter = false;
        //    //window1.inkCanvas1.DefaultDrawingAttributes.Height = 2;
        //    //window1.inkCanvas1.DefaultDrawingAttributes.Width = 2;
        //}
        //private void clrBlue(object sender, RoutedEventArgs e)
        //{
        //    window1.inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;

        //    // Set the DefaultDrawingAttributes for a red pen.
        //    window1.inkCanvas1.DefaultDrawingAttributes.Color = Colors.Blue;
        //    //window1.inkCanvas1.DefaultDrawingAttributes.IsHighlighter = false;
        //    //window1.inkCanvas1.DefaultDrawingAttributes.Height = 2;
        //    //window1.inkCanvas1.DefaultDrawingAttributes.Width = 2;
        //}
        //private void clrGreen(object sender, RoutedEventArgs e)
        //{
        //    window1.inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;

        //    // Set the DefaultDrawingAttributes for a red pen.
        //    window1.inkCanvas1.DefaultDrawingAttributes.Color = Colors.Green;
        //    //window1.inkCanvas1.DefaultDrawingAttributes.IsHighlighter = false;
        //    //window1.inkCanvas1.DefaultDrawingAttributes.Height = 2;
        //    //window1.inkCanvas1.DefaultDrawingAttributes.Width = 2;
        //}
        //private void clrYellow(object sender, RoutedEventArgs e)
        //{
        //    window1.inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;

        //    // Set the DefaultDrawingAttributes for a red pen.
        //    window1.inkCanvas1.DefaultDrawingAttributes.Color = Colors.Yellow;
        //    //window1.inkCanvas1.DefaultDrawingAttributes.IsHighlighter = false;
        //    //window1.inkCanvas1.DefaultDrawingAttributes.Height = 2;
        //    //window1.inkCanvas1.DefaultDrawingAttributes.Width = 2;
        //}
        //private void clrBlack(object sender, RoutedEventArgs e)
        //{
        //    window1.inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;

        //    // Set the DefaultDrawingAttributes for a red pen.
        //    window1.inkCanvas1.DefaultDrawingAttributes.Color = Colors.Black;
        //    //window1.inkCanvas1.DefaultDrawingAttributes.IsHighlighter = false;
        //    //window1.inkCanvas1.DefaultDrawingAttributes.Height = 2;
        //    //window1.inkCanvas1.DefaultDrawingAttributes.Width = 2;
        //}
        //private void clrWhite(object sender, RoutedEventArgs e)
        //{
        //    window1.inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;

        //    // Set the DefaultDrawingAttributes for a red pen.
        //    window1.inkCanvas1.DefaultDrawingAttributes.Color = Colors.White;
        //    //window1.inkCanvas1.DefaultDrawingAttributes.IsHighlighter = false;
        //    //window1.inkCanvas1.DefaultDrawingAttributes.Height = 2;
        //    //window1.inkCanvas1.DefaultDrawingAttributes.Width = 2;
        //}

        private void ColorChoose(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton.Name == "clrRed")
                window1.inkCanvas1.DefaultDrawingAttributes.Color = Colors.Red;
            if (clickedButton.Name == "clrBlue")
                window1.inkCanvas1.DefaultDrawingAttributes.Color = Colors.Blue;
            if (clickedButton.Name == "clrGreen")
                window1.inkCanvas1.DefaultDrawingAttributes.Color = Colors.Green;
            if (clickedButton.Name == "clrYellow")
                window1.inkCanvas1.DefaultDrawingAttributes.Color = Colors.Yellow;
            if (clickedButton.Name == "clrWhite")
                window1.inkCanvas1.DefaultDrawingAttributes.Color = Colors.White;
            if (clickedButton.Name == "clrBlack")
                window1.inkCanvas1.DefaultDrawingAttributes.Color = Colors.Black;

        }

        // close out all windows when exiting
        void App_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }


    }
}

﻿using System;
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

        public MainWindow()
        {
            InitializeComponent();
            foreach (var screen in screens)
            {
                cmbScreens.Items.Add(screen.DeviceName);
            }
            cmbScreens.SelectedIndex = 0;
        }
        void App_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void openCanvas(object sender, RoutedEventArgs e)
        {
            // setup dimensions for target screen
            System.Windows.Forms.Screen targetScreen = screens[cmbScreens.SelectedIndex];
            window1.WindowState = WindowState.Normal;
            //window1.Left = targetScreen.Bounds.Left;
            window1.Left = targetScreen.WorkingArea.Left;
            window1.Top = targetScreen.WorkingArea.Top;
            window1.Width = targetScreen.WorkingArea.Width;
            window1.Height = targetScreen.WorkingArea.Height;

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
        private void openCanvas_old(object sender, RoutedEventArgs e)
        {
            // setup dimensions for target screen
            System.Windows.Forms.Screen targetScreen = screens[cmbScreens.SelectedIndex];
            window1.WindowState = WindowState.Normal;
            //window1.Left = targetScreen.Bounds.Left;
            window1.Left = targetScreen.WorkingArea.Left;
            window1.Top = targetScreen.WorkingArea.Top;
            window1.Width = targetScreen.WorkingArea.Width;
            window1.Height = targetScreen.WorkingArea.Height;

            //capture screen and put as background
            takeScreenshot();
            //window1.inkCanvas1.Background = new BitmapImage(new Uri(@"c:\temp\foo.png",));
            window1.inkCanvas1.Background = new ImageBrush(new BitmapImage(new Uri(@"c:\temp\snap.png")));

            //open ink on target scree and maximize
            window1.SourceInitialized += (snd, arg) => window1.WindowState = WindowState.Maximized;
            window1.Show();
            //window1.Loaded += MaximizeWindow;
        }

        private System.IO.Stream takeScreenshot()
        {
            System.IO.MemoryStream myImg = new System.IO.MemoryStream();

            System.Windows.Forms.Screen targetScreen = screens[cmbScreens.SelectedIndex];
            int screenLeft = targetScreen.WorkingArea.Left;
            int screenTop = targetScreen.WorkingArea.Top;
            int screenWidth = targetScreen.WorkingArea.Width;
            int screenHeight = targetScreen.WorkingArea.Height;

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
        private void takeScreenshot_org()
        {
            System.Windows.Forms.Screen targetScreen = screens[cmbScreens.SelectedIndex];
            int screenLeft = targetScreen.WorkingArea.Left;
            int screenTop = targetScreen.WorkingArea.Top;
            int screenWidth = targetScreen.WorkingArea.Width;
            int screenHeight = targetScreen.WorkingArea.Height;

            using (Bitmap bmp = new Bitmap(screenWidth, screenHeight))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(screenLeft, screenTop, 0, 0, bmp.Size);
                }
                bmp.Save(@"c:\temp\snap.png", ImageFormat.Png);
            }
        }

        // Set the EditingMode to ink input.
        private void Ink(object sender, RoutedEventArgs e)
        {
            window1.inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;

            // Set the DefaultDrawingAttributes for a red pen.
            window1.inkCanvas1.DefaultDrawingAttributes.Color = Colors.Red;
            window1.inkCanvas1.DefaultDrawingAttributes.IsHighlighter = false;
            window1.inkCanvas1.DefaultDrawingAttributes.Height = 2;
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
        private void clrRed(object sender, RoutedEventArgs e)
        {
            window1.inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;

            // Set the DefaultDrawingAttributes for a red pen.
            window1.inkCanvas1.DefaultDrawingAttributes.Color = Colors.Red;
            window1.inkCanvas1.DefaultDrawingAttributes.IsHighlighter = false;
            window1.inkCanvas1.DefaultDrawingAttributes.Height = 1;
            window1.inkCanvas1.DefaultDrawingAttributes.Width = 1;
        }
        private void clrBlue(object sender, RoutedEventArgs e)
        {
            window1.inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;

            // Set the DefaultDrawingAttributes for a red pen.
            window1.inkCanvas1.DefaultDrawingAttributes.Color = Colors.Blue;
            window1.inkCanvas1.DefaultDrawingAttributes.IsHighlighter = false;
            window1.inkCanvas1.DefaultDrawingAttributes.Height = 1;
            window1.inkCanvas1.DefaultDrawingAttributes.Width = 1;
        }
        private void clrGreen(object sender, RoutedEventArgs e)
        {
            window1.inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;

            // Set the DefaultDrawingAttributes for a red pen.
            window1.inkCanvas1.DefaultDrawingAttributes.Color = Colors.Green;
            window1.inkCanvas1.DefaultDrawingAttributes.IsHighlighter = false;
            window1.inkCanvas1.DefaultDrawingAttributes.Height = 1;
            window1.inkCanvas1.DefaultDrawingAttributes.Width = 1;
        }
        private void clrBlack(object sender, RoutedEventArgs e)
        {
            window1.inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;

            // Set the DefaultDrawingAttributes for a red pen.
            window1.inkCanvas1.DefaultDrawingAttributes.Color = Colors.Black;
            window1.inkCanvas1.DefaultDrawingAttributes.IsHighlighter = false;
            window1.inkCanvas1.DefaultDrawingAttributes.Height = 1;
            window1.inkCanvas1.DefaultDrawingAttributes.Width = 1;
        }


        private void clrRedH(object sender, RoutedEventArgs e)
        {
            window1.inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;

            // Set the DefaultDrawingAttributes for a highlighter pen.
            window1.inkCanvas1.DefaultDrawingAttributes.Color = Colors.Red;
            window1.inkCanvas1.DefaultDrawingAttributes.IsHighlighter = true;
            window1.inkCanvas1.DefaultDrawingAttributes.Height = 25;
        }
        private void clrBlueH(object sender, RoutedEventArgs e)
        {
            window1.inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;

            // Set the DefaultDrawingAttributes for a highlighter pen.
            window1.inkCanvas1.DefaultDrawingAttributes.Color = Colors.Blue;
            window1.inkCanvas1.DefaultDrawingAttributes.IsHighlighter = true;
            window1.inkCanvas1.DefaultDrawingAttributes.Height = 25;
        }
        private void clrGreenH(object sender, RoutedEventArgs e)
        {
            window1.inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;

            // Set the DefaultDrawingAttributes for a highlighter pen.
            window1.inkCanvas1.DefaultDrawingAttributes.Color = Colors.Green;
            window1.inkCanvas1.DefaultDrawingAttributes.IsHighlighter = true;
            window1.inkCanvas1.DefaultDrawingAttributes.Height = 25;
        }
        private void clrYellowH(object sender, RoutedEventArgs e)
        {
            window1.inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;

            // Set the DefaultDrawingAttributes for a highlighter pen.
            window1.inkCanvas1.DefaultDrawingAttributes.Color = Colors.Yellow;
            window1.inkCanvas1.DefaultDrawingAttributes.IsHighlighter = true;
            window1.inkCanvas1.DefaultDrawingAttributes.Height = 25;
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
        private void myCommand()
        {
            int numStrokes = window1.inkCanvas1.Strokes.Count();
            if (numStrokes >= 1)
            {
                window1.inkCanvas1.Strokes.RemoveAt(numStrokes - 1);
            }

        }

    }
}
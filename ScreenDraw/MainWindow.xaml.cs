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
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace ScreenDraw
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        #region cursors
        //private Cursor Eraser = CursorHelper.FromByteArray(Properties.Resources.eraser);

        #endregion
        #region hotkey setup
        // register DLLs for global hotkeys
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const int HOTKEY_ID = 9000;

        //Modifiers:
        private const uint MOD_NONE = 0x0000; //(none)
        private const uint MOD_ALT = 0x0001; //ALT
        private const uint MOD_CONTROL = 0x0002; //CTRL
        private const uint MOD_SHIFT = 0x0004; //SHIFT
        private const uint MOD_WIN = 0x0008; //WINDOWS
        //CAPS LOCK:
        private const uint VK_CAPITAL = 0x14;
        // Function keys
        //        private const uint VK_F10 = 0x79;
        private const uint VK_F1 = 0x70;
        private const uint VK_F2 = 0x71;
        private const uint VK_F3 = 0x72;
        private const uint VK_F4 = 0x73;
        private const uint VK_F5 = 0x74;
        private const uint VK_F6 = 0x75;
        private const uint VK_F7 = 0x76;
        private const uint VK_F8 = 0x77;
        private const uint VK_F9 = 0x78;
        private const uint VK_F10 = 0x79;
        private const uint VK_F11 = 0x7A;
        private const uint VK_F12 = 0x7B;
        private const uint VK_Escape = 0x1B;
        private const uint VK_Z = 0x5A;
        private const uint VK_0 = 0x30;
        private const uint VK_1 = 0x31;
        private const uint VK_2 = 0x32;
        private const uint VK_3 = 0x33;
        private const uint VK_4 = 0x34;
        private const uint VK_5 = 0x35;
        private const uint VK_6 = 0x36;
        private const uint VK_7 = 0x37;
        private const uint VK_8 = 0x38;
        private const uint VK_9 = 0x39;

        private IntPtr _windowHandle;
        private HwndSource _source;
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            _windowHandle = new WindowInteropHelper(this).Handle;
            _source = HwndSource.FromHwnd(_windowHandle);
            _source.AddHook(HwndHook);

            RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_CONTROL, VK_CAPITAL); //CTRL + CAPS_LOCK
            RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_CONTROL, VK_F1); // CTRL + F1
            RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_CONTROL, VK_F2); // CTRL + F2
            RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_CONTROL, VK_F3); // CTRL + F3
            RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_CONTROL, VK_F4); // CTRL + F4
            RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_CONTROL, VK_F12); // CTRL + ESC
            RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_CONTROL, VK_Z); // CTRL + Z

            // colors - ALT/SHIFT 6-0
            RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_ALT + MOD_SHIFT, VK_6); // ALT + Shift + 6
            RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_ALT + MOD_SHIFT, VK_7); // ALT + Shift + 7
            RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_ALT + MOD_SHIFT, VK_8); // ALT + Shift + 8
            RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_ALT + MOD_SHIFT, VK_9); // ALT + Shift + 9
            RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_ALT + MOD_SHIFT, VK_0); // ALT + Shift + 0
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            switch (msg)
            {
                case WM_HOTKEY:
                    switch (wParam.ToInt32())
                    {
                        case HOTKEY_ID:
                            int vkey = (((int)lParam >> 16) & 0xFFFF);
                            if (vkey == VK_CAPITAL)
                            {
                                //tblock.Text += "CapsLock was pressed" + Environment.NewLine;
                                StartInk();
                            }
                            else if (vkey == VK_F1)
                            {
                                StartInk();
                            }
                            else if (vkey == VK_F2)
                            {
                                StartHighlight();
                            }
                            else if (vkey == VK_F3)
                            {
                                StartErase();
                            }
                            else if (vkey == VK_F4)
                            {
                                StartSelect();
                            }
                            else if (vkey == VK_F12)
                            {
                                StartClose();
                            }
                            else if (vkey == VK_Z)
                            {
                                StartUndo();
                            }
                            else if (vkey == VK_6)
                            {
                                TakeScreenshot();
                                window1.inkCanvas1.DefaultDrawingAttributes.Color = Colors.Blue;
                            }
                            else if (vkey == VK_7)
                            {
                                TakeScreenshot();
                                window1.inkCanvas1.DefaultDrawingAttributes.Color = Colors.Yellow;
                            }
                            else if (vkey == VK_8)
                            {
                                TakeScreenshot();
                                window1.inkCanvas1.DefaultDrawingAttributes.Color = Colors.Black;
                            }
                            else if (vkey == VK_9)
                            {
                                TakeScreenshot();
                                window1.inkCanvas1.DefaultDrawingAttributes.Color = Colors.Red;
                            }
                            else if (vkey == VK_0)
                            {
                                StartClose();
                            }
                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        protected override void OnClosed(EventArgs e)
        {
            _source.RemoveHook(HwndHook);
            UnregisterHotKey(_windowHandle, HOTKEY_ID);
            base.OnClosed(e);
        }
        #endregion

        Window1 window1;
        System.Windows.Forms.Screen[] screens = System.Windows.Forms.Screen.AllScreens;
        System.Windows.Forms.Screen selectedScreen;
        public static bool hasScreenshot = false;
        // todo: add global variable for pen size. hotkey to change it

        public MainWindow()
        {
            InitializeComponent();

            // setup notify icon
            this.nIcon.Icon = ScreenDraw.Properties.Resources.logo_icon;  // set notify icon image
            //this.nIcon.Icon = new Icon(@"../../Icons/logo_icon.ico");   // set notify icon image
            this.nIcon.Text = "ScreenDraw";
            this.nIcon.Visible = true;
            // setup righ-click
            this.nIcon.ContextMenuStrip = this.nStrip;
            this.nStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.nItem });
            this.nItem.Text = "Close";
            this.nItem.Click += ExitApp;
            //this.nIcon.ShowBalloonTip(5000, "Hi", "This is a baloon tooltip", System.Windows.Forms.ToolTipIcon.Info);
            // hide&unhide window
            //this.nIcon.Click += new System.EventHandler(this.nIcon_Click);
            this.nIcon.Click += this.nIcon_Click;



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

        #region Select Screens to Use
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
        #endregion

        #region Take screenshot and open canvas
        private void openCanvas(object sender, RoutedEventArgs e)
        {
            TakeScreenshot();
        }
        private void TakeScreenshot() {

            // only if no screenshot exists
            if (!hasScreenshot)
            {
                window1 = new Window1();
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
                //window1.SourceInitialized += (snd, arg) => window1.WindowState = WindowState.Maximized;
                window1.Show();
                //this.Topmost = true;
                //this.Activate();
                window1.Topmost = true;
                window1.Topmost = false;
                //window1.Loaded += MaximizeWindow;

                
                // set screenshot variable
                hasScreenshot = true;
            }
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
        #endregion

        #region Setup buttons
        private void StartInk()
        {
            // if screenshot doesnt already exist, take it
            if (!hasScreenshot)
                TakeScreenshot();

            // setup pen
            window1.inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;

            // Set the DefaultDrawingAttributes for a red pen.
            window1.inkCanvas1.DefaultDrawingAttributes.Color = Colors.Red;
            window1.inkCanvas1.DefaultDrawingAttributes.IsHighlighter = false;
            window1.inkCanvas1.DefaultDrawingAttributes.Height = 2;
            window1.inkCanvas1.DefaultDrawingAttributes.Width = 2;

            // set cursor to pen
            System.IO.MemoryStream cursorStream = new System.IO.MemoryStream(ScreenDraw.Properties.Resources.pencil);
            Cursor cur = new Cursor(cursorStream);
            window1.inkCanvas1.Cursor = cur;

        }
        private void StartHighlight()
        {         
            // if screenshot doesnt already exist, take it
            if (!hasScreenshot)
                TakeScreenshot();

            // setup highlighter
            window1.inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;

            // Set the DefaultDrawingAttributes for a highlighter pen.
            window1.inkCanvas1.DefaultDrawingAttributes.Color = Colors.Yellow;
            window1.inkCanvas1.DefaultDrawingAttributes.IsHighlighter = true;
            window1.inkCanvas1.DefaultDrawingAttributes.Height = 25;

            // set cursor to pen
            //window1.inkCanvas1.Cursor = Cursors.Pen;
            System.IO.MemoryStream cursorStream = new System.IO.MemoryStream(ScreenDraw.Properties.Resources.marker);
            Cursor cur = new Cursor(cursorStream);
            window1.inkCanvas1.Cursor = cur;

        }
        private void StartErase()
        {
            window1.inkCanvas1.EditingMode = InkCanvasEditingMode.EraseByStroke;

            // set cursor to eraser
            System.IO.MemoryStream cursorStream = new System.IO.MemoryStream(ScreenDraw.Properties.Resources.eraser);
            Cursor cur = new Cursor(cursorStream);
            window1.inkCanvas1.Cursor = cur;

        }
        private void StartSelect()
        {
            window1.inkCanvas1.EditingMode = InkCanvasEditingMode.Select;

            // set cursor to pen
            window1.inkCanvas1.Cursor = Cursors.Cross;
        }
        private void ExitApp(Object sender, EventArgs e)
        {
            try
            {
                StartClose();
            }
            catch { }
            this.nIcon.Visible = false;
            //this.Exit();
            //Close();
            System.Windows.Application.Current.Shutdown();
        }
        private void StartClose()
        {           
            // reset screenshot variable
            hasScreenshot = false;

            // close windows
            window1.Close();
            window1 = null;
            window1 = new Window1();
        }
        private void StartUndo()
        {
            int numStrokes = window1.inkCanvas1.Strokes.Count();
            if (numStrokes >= 1)
            {
                window1.inkCanvas1.Strokes.RemoveAt(numStrokes - 1);
            }
        }

        private void Ink(object sender, RoutedEventArgs e)
        {
            StartInk();
        }
        // Set the EditingMode to highlighter input.
        private void Highlight(object sender, RoutedEventArgs e)
        {
            StartHighlight();
        }
        // Set the EditingMode to erase by stroke.
        private void EraseStroke(object sender, RoutedEventArgs e)
        {
            StartErase();
        }
        // Set the EditingMode to selection.
        private void Select(object sender, RoutedEventArgs e)
        {
            StartSelect();
        }
        private void btnUndo(object sender, RoutedEventArgs e)
        {
            StartUndo();
        }
        private void btnClose(object sender, RoutedEventArgs e)
        {
            StartClose();
            //// reset screenshot variable
            //hasScreenshot = false;

            //// close windows
            //window1.Close();
            //window1 = null;
            //window1 = new Window1();
            ////window1.Hide();
            ////window1.inkCanvas1.Background = System.Windows.Media.Brushes.Ivory;
            ////window1.inkCanvas1.Background = null;
            ////if (System.IO.File.Exists(@"c:\temp\snap.png"))
            ////    System.IO.File.Delete(@"c:\temp\snap.png");
        }
#endregion

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
            this.nIcon.Visible = false;
            System.Windows.Application.Current.Shutdown();
        }

        #region notification icon
        System.Windows.Forms.NotifyIcon nIcon = new System.Windows.Forms.NotifyIcon();  // notify icon test
        System.Windows.Forms.ContextMenuStrip nStrip = new System.Windows.Forms.ContextMenuStrip();
        System.Windows.Forms.ToolStripMenuItem nItem = new System.Windows.Forms.ToolStripMenuItem();
        private void nIcon_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MouseEventArgs me = (System.Windows.Forms.MouseEventArgs)e;
//            MouseEventArgs me = (MouseEventArgs)e;
            //if (e.Equals(MouseButton.Left))
            if(me.Button== System.Windows.Forms.MouseButtons.Left)
            {
                //Show();
                //WindowState = FormWindowState.Normal;
                if (this.Visibility == Visibility.Collapsed)
                    this.Visibility = Visibility.Visible;
                else
                    this.Visibility = Visibility.Collapsed;
            }
        }
        #endregion
    }

    public sealed class CursorHelper
    {
        private CursorHelper() { } //Private constructor as we do not need any instances of this class.

        public static Cursor FromByteArray(byte[] array)
        {
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(array))
            {
                return new Cursor(memoryStream);
            }
        }
    }
}

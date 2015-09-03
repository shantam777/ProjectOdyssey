using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Awesomium.Core;
using Awesomium.Windows.Controls;


namespace ProjectOdyssey
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            webControl.ShowCreatedWebView += webControl_ShowCreatedWebView;
            //webControl.LoadingFrameComplete += OnLoadingComplete;
            this.Source = WebCore.Configuration.HomeURL;
        }
        public MainWindow(IntPtr nativeView)
        {
            InitializeComponent();
            webControl.ShowCreatedWebView += webControl_ShowCreatedWebView;
            webControl.WindowClose += webControl_WindowClose;
            this.NativeView = nativeView;
            this.IsRegularWindow = false;
        }
        public MainWindow(Uri url)
        {
            InitializeComponent();
            webControl.ShowCreatedWebView += webControl_ShowCreatedWebView;
            webControl.WindowClose += webControl_WindowClose;
            this.Source = url;
        }
        public Uri Source
        {
            get { return (Uri)GetValue(SourceP); }
            set { SetValue(SourceP, value); }
        }
        public static readonly DependencyProperty SourceP =
            DependencyProperty.Register("Source", typeof(Uri),
            typeof(MainWindow), new FrameworkPropertyMetadata(null));
        public IntPtr NativeView
        {
            get { return (IntPtr)GetValue(NativeViewP); }
            private set { this.SetValue(MainWindow.NativeViewPKey, value); }
        }
        private static readonly DependencyPropertyKey NativeViewPKey =
            DependencyProperty.RegisterReadOnly("NativeView", typeof(IntPtr),
            typeof(MainWindow), new FrameworkPropertyMetadata(IntPtr.Zero));
        public static readonly DependencyProperty NativeViewP =
            NativeViewPKey.DependencyProperty;
        public bool IsRegularWindow
        {
            get { return (bool)GetValue(IsRegularWindowP); }
            private set { this.SetValue(MainWindow.IsRegularWindowPKey, value); }
        }
        private static readonly DependencyPropertyKey IsRegularWindowPKey =
            DependencyProperty.RegisterReadOnly("IsRegularWindow", typeof(bool),
            typeof(MainWindow), new FrameworkPropertyMetadata(true));
        public static readonly DependencyProperty IsRegularWindowP =
            IsRegularWindowPKey.DependencyProperty;
        private void webControl_ShowCreatedWebView(object sender,
    ShowCreatedWebViewEventArgs e)
        {
            if (webControl == null)
                return;

            if (!webControl.IsLive)
                return;

            MainWindow newWindow;

            if (e.IsPopup && !e.IsUserSpecsOnly)
            {
                Int32Rect screenRect = e.Specs.InitialPosition.GetInt32Rect();
                newWindow = new MainWindow(e.NewViewInstance);
                newWindow.ShowInTaskbar = false;
                newWindow.WindowStyle = WindowStyle.ToolWindow;
                newWindow.ResizeMode = e.Specs.Resizable ?
                    ResizeMode.CanResizeWithGrip :
                    ResizeMode.NoResize;
                if ((screenRect.Width > 0) && (screenRect.Height > 0))
                {
                    double horizontalBorderHeight =
                        SystemParameters.ResizeFrameHorizontalBorderHeight;
                    double verticalBorderWidth =
                        SystemParameters.ResizeFrameVerticalBorderWidth;
                    double captionHeight =
                        SystemParameters.CaptionHeight;
                    newWindow.Width = screenRect.Width +
                        (verticalBorderWidth * 2);
                    newWindow.Height = screenRect.Height +
                        captionHeight +
                        (horizontalBorderHeight * 2);
                }
                newWindow.Show();
                if ((screenRect.Y > 0) && (screenRect.X > 0))
                {
                    newWindow.Top = screenRect.Y;
                    newWindow.Left = screenRect.X;
                }
            }
            else if (e.IsWindowOpen || e.IsPost)
            {
                newWindow = new MainWindow(e.NewViewInstance);
                newWindow.Show();
            }
            else
            {
                e.Cancel = true;
                newWindow = new MainWindow(e.TargetURL);
                newWindow.Show();
            }
        }

        private void webControl_WindowClose(object sender, WindowCloseEventArgs e)
        {
            if (!e.IsCalledFromFrame)
                this.Close();
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            webControl.Dispose();
        }

    }



}

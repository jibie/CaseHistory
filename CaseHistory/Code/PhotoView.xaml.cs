using System.Windows;
using OfIllness.ModeView;
using OfIllness.Model;
using System.Windows.Media.Imaging;
using System;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;

namespace OfIllness
{
    /// <summary>
    /// PhotoView.xaml 的交互逻辑
    /// </summary>
    public partial class PhotoView : Window
    {
        public PhotoView()
        {
            InitializeComponent();
        }

        public PhotoView(ShowPhotoInfo spi)
            : this()
        {
            PhotoInfo = spi;
            Owner = PhotoInfo.Owner;
            ShowPic(spi.CurrentSclerte);
        }

        private bool m_IsMouseLeftButtonDown;


        public ShowPhotoInfo PhotoInfo
        {
            get;
            set;
        }


        private void btnLeft90_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            imgRtf.Angle -= 90;
        }

        private void btnRight90_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            imgRtf.Angle += 90;
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            ReSet();

        }

        private void ReSet()
        {
            imgRtf.Angle = 0;

            imgTlt.X = 0;
            imgTlt.Y = 0;

            imgScale.ScaleX = 1;
            imgScale.ScaleY = 1;
        }


        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            ReSet();
            if (PhotoInfo != null)
            {
                if (PhotoInfo.CurrentPhotoIndex != 0)
                {
                    --PhotoInfo.CurrentPhotoIndex;
                }
                ShowPic(PhotoInfo.CurrentSclerte);
            }

        }

        private void ShowPic(SclerteInfo si)
        {
            if (si != null)
            {
                imgContent.Source = new BitmapImage(new Uri(si.SleritePicUrl));
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            ReSet();
            if (PhotoInfo != null)
            {
                if (PhotoInfo.CurrentPhotoIndex != PhotoInfo.OcSclerteInfo.Count - 1)
                {
                    PhotoInfo.CurrentPhotoIndex++;
                    ShowPic(PhotoInfo.CurrentSclerte);
                }
            }
        }

        private void btnMagnify_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            ZoomImage(0.05);
        }

        private void btnLessen_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            ZoomImage(-0.05);
        }

        private void MasterImage_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            var scale = e.Delta * 0.001;
            ZoomImage(scale);
        }
        /// <summary>
        /// 放大
        /// </summary>
        private void ZoomImage(double scale)
        {
            if (imgScale.ScaleX + scale < 1)
                return;
            imgScale.ScaleX += scale;
            imgScale.ScaleY += scale;
        }

        private void MasterImage_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;
            if (m_IsMouseLeftButtonDown)
                DoImageMove(content, e);
        }
        Point m_PreviousMousePoint = new Point();
        private void DoImageMove(FrameworkElement rectangle, MouseEventArgs e)
        {
            e.Handled = true;
            if (e.LeftButton != MouseButtonState.Pressed)
                return;
            var group = imgTFGroup;
            var transform = imgTlt;
            var position = e.GetPosition(rectangle);
            transform.X += position.X - m_PreviousMousePoint.X;
            transform.Y += position.Y - m_PreviousMousePoint.Y;
            m_PreviousMousePoint = position;
        }


        private void MasterImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            m_IsMouseLeftButtonDown = true;
            m_PreviousMousePoint = e.GetPosition(content);
            //content.CaptureMouse();

        }
        private void MasterImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            //imgContent.ReleaseMouseCapture();
            m_IsMouseLeftButtonDown = false;
        }

    }
}

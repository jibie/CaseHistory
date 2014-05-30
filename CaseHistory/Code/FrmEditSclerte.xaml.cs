using System.Windows;
using OfIllness.Model;
using System.IO;
using System.Windows.Media.Imaging;
using System;

namespace OfIllness
{
    public partial class FrmEditSclerte : Window
    {
        public FrmEditSclerte()
        {
            InitializeComponent();
            SclerteValue = new SclerteInfo();
        }

        public FrmEditSclerte(PatientInfo pi)
            : this()
        {
            Title = string.Format("新增[{0}]的骨片信息", pi.Name);
            SclerteValue.PatientId = pi.Id;
        }

        public SclerteInfo SclerteValue
        {
            get
            {
                return DataContext as SclerteInfo;
            }
            set
            {
                DataContext = value;
            }
        }

        private void btnBrowserFile_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "图片文件(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|所有文件(*.*)|*.*";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SclerteValue.SleritePicUrl = ofd.FileName;
                txtPicUrl.Text = SclerteValue.SleritePicUrl;
                imgPic.Source = (BitmapSource)AppGlobal.ImgThumbnail.Convert(SclerteValue.SleritePicUrl, null, null, null);
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(txtPicUrl.Text))
            {
                UMessageBox.Show("错误!", "文件不存在", MessageBoxButton.OK);
                return;
            }
            DialogResult = true;
            e.Handled = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            e.Handled = true;
            SclerteValue.SleritePicUrl = null;
        }
    }
}

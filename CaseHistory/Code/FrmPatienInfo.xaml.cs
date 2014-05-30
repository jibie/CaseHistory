using System.Windows;
using OfIllness.Model;
using System;
using System.Windows.Media.Imaging;
using OfIllness.ModeView;

namespace OfIllness
{
    /// <summary>
    /// FrmPatienInfo.xaml 的交互逻辑
    /// </summary>
    public partial class FrmPatienInfo : Window
    {

        public FrmPatienInfo()
        {
            InitializeComponent();
            PatientValue = new PatientInfo
            {
                BeHospitalizedDate = DateTime.Now,
                CourtyardState = "在院",
                Sex = "未知"
            };
        }

        public FrmPatienInfo(PatientInfo p)
        {
            InitializeComponent();
            PatientValue = p;
        }

        public PatientInfo PatientValue
        {
            get
            {
                return DataContext as PatientInfo;
            }
            set
            {
                DataContext = value;
            }
        }


        private void linkDelOpr_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            FrameworkContentElement hl = e.OriginalSource as FrameworkContentElement;
            OperationInfo oi = hl.Tag as OperationInfo;
            if (oi.OperationId > 0)
            {
                bool result = UMessageBox.Show("删除?", string.Format("您确定要删除:\n{0}{1:yyyy-MM-dd}", oi.Surgeon, oi.OperationDate), MessageBoxButton.OKCancel).Value;
                if (result)
                {
                    if (!AppGlobal.DataQuery.DeleteOperationInfo(oi.OperationId))
                        return;
                    PatientValue.OcOperation.Remove(oi);
                }

            }

        }

        private void linkAddOperation_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            FrmEditOperation fao = new FrmEditOperation(PatientValue);
            if (fao.ShowDialog() == true)
            {
                OperationInfo oi = fao.OperationValue;
                if (PatientValue.Id > 0)
                {
                    oi.OperationId = AppGlobal.DataQuery.AddOperationInfo(oi);
                   
                }
                PatientValue.OcOperation.Add(oi);
            }
        }



        private void linkDelPic_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            FrameworkContentElement hl = e.OriginalSource as FrameworkContentElement;
            SclerteInfo si = hl.DataContext as SclerteInfo;
            bool result = UMessageBox.Show("删除骨片?", "您确定要删除这张骨片\n" + si.Title, MessageBoxButton.OKCancel).Value;
            if (result)
            {
                if (si.ScleriteId > 0)
                {
                    if (!AppGlobal.DataQuery.DeleteSclerteInfo(si))
                        return;
                    PatientValue.OcSclerteInfo.Remove(si);
                }

            }
        }

        private void linkShowPic_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            ShowPhotoInfo spi = new ShowPhotoInfo { OcSclerteInfo = PatientValue.OcSclerteInfo, CurrentPhotoIndex = (ushort)PhotosListBox.SelectedIndex, Owner = this };
            AppGlobal.ShowPho.Execute(spi);
        }

        private void linkOperationEdit_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            FrameworkContentElement fce = sender as FrameworkContentElement;
            OperationInfo oi = fce.DataContext as OperationInfo;
            FrmEditOperation feo = new FrmEditOperation { Owner = this };
            feo.OperationValue = oi;
            if (feo.ShowDialog() == true)
            {
                AppGlobal.DataQuery.EditOperationInfo(feo.OperationValue);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            DialogResult = false;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                UMessageBox.Show("错误", "请输入患者姓名", MessageBoxButton.OK);
                return;
            }
            if (!dpBeHospitalized.Value.HasValue)
            {
                UMessageBox.Show("错误", "请输入入院日期!", MessageBoxButton.OK);
                return;
            }

            DialogResult = true;
        }

        private void linkAddSclerte_Click(object sender, RoutedEventArgs e)
        {
            FrmEditSclerte fec = new FrmEditSclerte(PatientValue);
            fec.Owner = this;
            if (fec.ShowDialog() == true)
            {
                SclerteInfo si = fec.SclerteValue;
                if (si.PatientId > 0)
                {
                    AppGlobal.DataQuery.AddSclerteInfo(si);
                }
                PatientValue.OcSclerteInfo.Add(si);
            }
        }

    }
}

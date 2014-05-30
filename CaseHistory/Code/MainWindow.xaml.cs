using System;
using System.Windows;
using System.Windows.Controls;
using OfIllness.Model;
using System.Text;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using OfIllness.ModeView;
using System.Diagnostics;
namespace OfIllness
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DeleDataRefresh = (source) =>
            {
                DGView.ItemsSource = OcP;
            };
        }

        private ObservableCollection<PatientInfo> OcP;


        Action<ObservableCollection<PatientInfo>> DeleDataRefresh;

        private void DGView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DGView.SelectedItem != null)
            {
                PatientInfo p = DGView.SelectedItem as PatientInfo;
                lvOperation.ItemsSource = p.OcOperation;
                lvPic.ItemsSource = p.OcSclerteInfo;
            }
            e.Handled = true;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            DGView.ItemsSource = null;
            StringBuilder sb = new StringBuilder();
            //手术日期查询
            if (opsStart.Value.HasValue || opsEnd.Value.HasValue)
            {
                string partId = string.Join(",", AppGlobal.DataQuery.QueryOperationPatient(opsStart.Value, opsEnd.Value));
                if (string.IsNullOrEmpty(partId))
                {
                    return;
                }
                else
                    sb.AppendFormat(" and PatientId in ({0})", partId);
            }

            string PatientName = txtName.Text.Trim();
            if (PatientName.Length > 0)
            {
                sb.AppendFormat(" and PatientName like '%{0}%'", PatientName);
            }
            if (dpStart.Value.HasValue)
            {
                sb.AppendFormat(" and BeHospitalizedDate>='{0:yyyy-MM-dd}'", dpStart.Value);
            }
            if (dpEnd.Value.HasValue)
            {
                sb.AppendFormat(" and BeHospitalizedDate<='{0:yyyy-MM-dd 23:59:59.999}'", dpEnd.Value);
            }

            if (cmbLeaveHospital.Text != "全部" && !string.IsNullOrEmpty(cmbLeaveHospital.Text))
            {
                sb.AppendFormat(" and CourtyardState ='{0}'", cmbLeaveHospital.Text);
            }

            string tech = txtTeach.Text.Trim();
            if (tech.Length > 0)
            {
                sb.AppendFormat(" and like '%{0}%'", tech);
            }
            OcP = new ObservableCollection<PatientInfo>(AppGlobal.DataQuery.QueryPatientBaseInfo(sb.ToString()));
            DGView.ItemsSource = OcP;
        }

        private void btnInHospital_Click(object sender, RoutedEventArgs e)
        {
            FrmPatienInfo fpi = new FrmPatienInfo();
            fpi.Title = "新增患者信息";
            if (fpi.ShowDialog() == true)
            {
                if (AppGlobal.DataQuery.AddPatientInfo(fpi.PatientValue) > 0)
                {
                    PatientInfo value = fpi.PatientValue;
                    value.Seq =Convert.ToUInt32(OcP.Count+1);
                    OcP.Add(value);
                }
            }

            e.Handled = true;
        }

        private void linkDel_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            FrameworkContentElement hl = e.OriginalSource as FrameworkContentElement;
            PatientInfo pi = hl.DataContext as PatientInfo;
            bool result = UMessageBox.Show("删除", string.Format("删除患者 {0} 的所有信息", pi.Name), MessageBoxButton.OKCancel).Value;
            if (result)
            {
                if (AppGlobal.DataQuery.DeletePatientInfo(pi))
                {
                    if (object.ReferenceEquals(pi, DGView.SelectedItem))
                    {
                        lvOperation.ItemsSource = null;
                        lvPic.ItemsSource = null;
                    }
                    OcP.Remove(pi);

                }
            }

        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtName.Clear();
            dpStart.Value = null;
            dpEnd.Value = null;
            cmbLeaveHospital.Text = "全部";
            txtTeach.Clear();
            opsStart.Value = null;
            opsEnd.Value = null;
            e.Handled = true;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                OcP = new ObservableCollection<PatientInfo>(AppGlobal.DataQuery.QueryPatientBaseInfo(null));
                DGView.Dispatcher.BeginInvoke(DeleDataRefresh, OcP);
            });

        }

        private void DGView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (DGView.SelectedItem != null)
            {
                AppGlobal.ShowPatientInfo.Execute(DGView.SelectedItem);
            }
        }

        private void lvOperation_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (lvOperation.SelectedItem != null)
            {
                PatientInfo fi = DGView.SelectedItem as PatientInfo;
                OperationInfo oti = lvOperation.SelectedItem as OperationInfo;
                FrmEditOperation feo = new FrmEditOperation(fi) { Owner = this, OperationValue = oti };
                if (feo.ShowDialog() == true)
                {
                    AppGlobal.DataQuery.EditOperationInfo(feo.OperationValue);
                }
            }
        }

        private void lvPic_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PatientInfo fi = DGView.SelectedItem as PatientInfo;
            if (fi != null)
            {
                ShowPhotoInfo spi = new ShowPhotoInfo { OcSclerteInfo = fi.OcSclerteInfo, CurrentPhotoIndex = (ushort)lvPic.SelectedIndex, Owner = this };
                AppGlobal.ShowPho.Execute(spi);
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            Process.Start("http://www.cnblogs.com/jsyb/");
        }

    }
}

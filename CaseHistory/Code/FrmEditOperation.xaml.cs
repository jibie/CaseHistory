using System.Windows;
using OfIllness.Model;
using System;

namespace OfIllness
{
    /// <summary>
    /// FrmAddOperation.xaml 的交互逻辑
    /// </summary>
    public partial class FrmEditOperation : Window
    {
        public FrmEditOperation()
        {
            InitializeComponent();
            OperationValue = new OperationInfo
            {
                OperationDate = DateTime.Now
            };
        }

        public FrmEditOperation(PatientInfo pi)
            : this()
        {
            OperationValue.PatientId = pi.Id;
            Title = string.Format("{0}[{1}]的手术信息", OperationValue.PatientId == 0 ? "新增" : "修改/查看", pi.Name);
        }

        public OperationInfo OperationValue
        {
            get
            {
                return DataContext as OperationInfo;
            }
            set
            {
                DataContext = value;
            }
        }


        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSurgeon.Text))
            {
                UMessageBox.Show("错误", "手术医师不能为空", MessageBoxButton.OK);
                return;
            }
            this.DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

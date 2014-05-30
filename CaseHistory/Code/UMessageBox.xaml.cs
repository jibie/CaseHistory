using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;


namespace OfIllness
{
    /// <summary>
    /// UMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class UMessageBox : Window
    {
        /// <summary>
        /// 禁止在外部实例化
        /// </summary>
        private UMessageBox()
        {
            InitializeComponent();
        }

        public new string Title
        {
            get { return this.lblTitle.Text; }
            set { this.lblTitle.Text = value; }
        }

        public string Message
        {
            get { return this.lblMsg.Text; }
            set { this.lblMsg.Text = value; }
        }

        /// <summary>
        /// 静态方法 模拟MESSAGEBOX.Show方法
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="msg">消息</param>
        public static bool? Show(string title, string msg, MessageBoxButton mbb)
        {
            var msgBox = new UMessageBox()
            {
                Title = title,
                Message = msg
            };
            switch (mbb)
            {
                case MessageBoxButton.OK:
                    msgBox.btnOk.Visibility = Visibility.Visible;
                    msgBox.btnCanel.Visibility = Visibility.Hidden;
                    Canvas.SetRight(msgBox.btnOk, 10);
                    Canvas.SetBottom(msgBox.btnOk, 10);
                    break;

                case MessageBoxButton.OKCancel:
                case MessageBoxButton.YesNo:
                    msgBox.btnOk.Visibility = Visibility.Visible;
                    msgBox.btnCanel.Visibility = Visibility.Visible;
                    break;

            }
            return msgBox.ShowDialog();
        }



        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            DragMove();
            base.OnMouseLeftButtonDown(e);
        }

        private void btnCalel_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            this.DialogResult = false;
            this.Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            this.DialogResult = true;
            this.Close();
        }
    }
}
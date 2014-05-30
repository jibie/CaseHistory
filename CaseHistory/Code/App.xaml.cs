using System;
using System.Windows;
using System.Windows.Threading;
using System.IO;
using System.Reflection;
using System.Resources;
namespace OfIllness
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            
        }
        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            ShowException(e.Exception);

        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString(), "未捕获异常", MessageBoxButton.OK, MessageBoxImage.Exclamation);


        }

        public void ShowException(Exception e)
        {
            MessageBox.Show(e.ToString(), "未捕获异常", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }
}

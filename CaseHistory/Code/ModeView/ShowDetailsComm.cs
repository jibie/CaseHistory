using System;
using System.Windows.Input;
using OfIllness.Model;
using System.Windows;
namespace OfIllness.ModeView
{
    public class ShowDetailsComm : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;


        public void Execute(object parameter)
        {
            PatientInfo p = parameter as PatientInfo;
            FrmPatienInfo fp = new FrmPatienInfo(p);
            fp.Owner = Application.Current.MainWindow;
            if (fp.ShowDialog() == true)
            {
                AppGlobal.DataQuery.EditPatientBaseInfo(p);
            }
        }
    }
}

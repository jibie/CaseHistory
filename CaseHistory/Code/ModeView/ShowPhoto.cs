using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;
using OfIllness.Model;
using System.Windows;

namespace OfIllness.ModeView
{
    public class ShowPhoto : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            ShowPhotoInfo spi = parameter as ShowPhotoInfo;
            PhotoView pw = new PhotoView(spi); ;
            pw.ShowDialog();
        }
    }
    public class ShowPhotoInfo
    {

        public ObservableCollection<SclerteInfo> OcSclerteInfo
        {
            get;
            set;
        }

        public SclerteInfo CurrentSclerte
        {
            get
            {
                SclerteInfo si = null;
                if (CurrentPhotoIndex >= 0 && OcSclerteInfo != null && OcSclerteInfo.Count > CurrentPhotoIndex)
                {
                    si = OcSclerteInfo[(int)CurrentPhotoIndex];
                }
                return si;
            }
        }
        public UInt16 CurrentPhotoIndex
        {
            get;
            set;
        }

        public Window Owner
        {
            get;
            set;
        }
    }
}

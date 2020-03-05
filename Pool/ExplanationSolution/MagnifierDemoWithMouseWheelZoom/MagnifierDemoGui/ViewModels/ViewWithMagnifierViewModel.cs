using System;
using System.Windows.Media.Imaging;
using Caliburn.Micro;

namespace Gui.ViewModels
{
    public class ViewWithMagnifierViewModel : Screen
    {
        private BitmapImage m_image;

        public ViewWithMagnifierViewModel(BitmapImage image)
        {
            Image = image;
            Guid = Guid.NewGuid();
        }

        public  Guid Guid { get; }

        public BitmapImage Image
        {
            get => m_image;
            set
            {
                m_image = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
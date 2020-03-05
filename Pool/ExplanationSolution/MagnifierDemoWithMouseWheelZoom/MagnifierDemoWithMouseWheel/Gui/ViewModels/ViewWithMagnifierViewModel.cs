using System;
using System.Windows.Media.Imaging;
using Caliburn.Micro;

namespace Gui.ViewModels
{
    public class ViewWithMagnifierViewModel : Screen, IDisposable
    {
        public ViewWithMagnifierViewModel(BitmapImage image)
        {
            Image = image;
        }

        public BitmapImage Image { get; set; }

        private bool m_disposed;

        ~ViewWithMagnifierViewModel()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                Image = null;

                if (disposing)
                {
                }
            }
            m_disposed = true;
        }

    }
}
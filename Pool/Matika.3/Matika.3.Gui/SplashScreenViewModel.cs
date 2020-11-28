using System;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Action = System.Action;

namespace Matika._3.Gui
{
    public class SplashScreenViewModel : Screen
    {
        private readonly Action m_initAction;
        private string m_info;
        private string m_text;

        public SplashScreenViewModel(Action initAction, string version, string path, string info)
        {
            m_initAction = initAction;
            Text = version;
            Image = new BitmapImage(new Uri(path, UriKind.Relative));
            Info = info;
        }

        public string Text
        {
            get => m_text;
            set
            {
                m_text = value;
                NotifyOfPropertyChange();
            }
        }

        public string Info
        {
            get => m_info;
            set
            {
                m_info = value;
                NotifyOfPropertyChange();
            }
        }

        public BitmapImage Image { get; set; }

        protected override async void OnActivate()
        {
            base.OnActivate();

            await Task.Run(m_initAction);

            TryClose();
        }
    }
}
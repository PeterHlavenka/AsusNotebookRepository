using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Mediaresearch.Framework.Gui;
using Screen = Caliburn.Micro.Screen;

namespace Gui.ViewModels
{
    public class MainViewModel : Screen
    {
        private ViewWithMagnifierViewModel m_model;

        public MainViewModel()
        {
            var image  = new BitmapImage(new Uri("..\\Images\\image.jpg", UriKind.Relative));

            Model = new ViewWithMagnifierViewModel(image);

            ChangeCommand = new RelayCommand(DoChange);
        }

        private List<string> Names { get; set; }


        public ICommand ChangeCommand { get; }

        public ViewWithMagnifierViewModel Model
        {
            get => m_model;
            set
            {
                m_model = value;
                NotifyOfPropertyChange();
            }
        }

        private void DoChange()
        {
            Names = new List<string>();

            var dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var folder = new DirectoryInfo(dialog.SelectedPath);
                var images = folder.GetFiles();

                foreach (var fileInfo in images)
                {
                    Names.Add($@"{dialog.SelectedPath}/{fileInfo.Name}");
                }

                Thread thread = new Thread(ChangeImages);
                thread.Start();
            }
        }

        private void ChangeImages()
        {
            foreach (var name in Names)
            {
                var image = new BitmapImage(new Uri(name));
                image.Freeze();

                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    Model.Dispose();
                    Model = new ViewWithMagnifierViewModel(image);
                });

                Thread.Sleep(1000);
            }
        }
    }
}
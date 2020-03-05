using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Gui.Enums;
using Mediaresearch.Framework.Gui;
using Screen = Caliburn.Micro.Screen;

namespace Gui.ViewModels
{
    public class MainViewModel : Screen
    {
        private double m_memoryUsed;
        private ViewWithMagnifierViewModel m_model;

        public MainViewModel()
        {
            Image = new BitmapImage(new Uri("..\\Images\\image.jpg", UriKind.Relative));

            MagnifierViewModel = new ViewWithMagnifierViewModel(Image);

            ChangeModelCommand = new RelayCommand(DoChange);
            ChangeImageCommand = new RelayCommand(DoChange);

            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimerTick;
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(500);
            dispatcherTimer.Start();
        }

       
        private Guid m_modelGuid;

        public Guid ModelGuid
        {
            get { return m_modelGuid; }
            set
            {
                m_modelGuid = value; 
                NotifyOfPropertyChange();
            }
        }



        public double MemoryUsed
        {
            get => m_memoryUsed;
            set
            {
                m_memoryUsed = value;
                NotifyOfPropertyChange();
            }
        }

        private List<string> Names { get; set; }
        private List<BitmapImage> Images { get; set; }

        private BitmapImage Image { get; }

        public ICommand ChangeModelCommand { get; }
        public ICommand ChangeImageCommand { get; }

        public ViewWithMagnifierViewModel MagnifierViewModel
        {
            get => m_model;
            set
            {
                m_model = value;
                NotifyOfPropertyChange();
            }
        }

        private void DispatcherTimerTick(object sender, EventArgs e)
        {
            MemoryUsed = Process.GetCurrentProcess().PrivateMemorySize64 / 1000000;

            ModelGuid = MagnifierViewModel.Guid;
        }

        private void DoChange(object obj)
        {
            var type = (ChangeType) obj;

            Names = new List<string>();
            Images = new List<BitmapImage>();

            var dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var folder = new DirectoryInfo(dialog.SelectedPath);
                var images = folder.GetFiles();

                foreach (var fileInfo in images)
                {
                    Names.Add($@"{dialog.SelectedPath}/{fileInfo.Name}");
                }

                foreach (var name in Names)
                {
                    // var image = new BitmapImage(new Uri(name));   // memory leak

                    var bitmap = CreateBitmap(name);

                    Images.Add(bitmap);
                }

                if (type == ChangeType.ViewModelChange)
                {
                    var thread = new Thread(ChangeViewModels);
                    thread.Start();
                }
                else
                {
                    var thread = new Thread(ChangeImage);
                    thread.Start();
                }
            }
        }

        private void ChangeImage()
        {
            foreach (var bitmapImage in Images)
            {
                MagnifierViewModel.Image = bitmapImage;
                Thread.Sleep(200);
            }

            Images.Clear();      // dame moznost k uklidu (pamet je porad alokovana)

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private BitmapImage CreateBitmap(string name)
        {
            var bitmap = new BitmapImage();
            var stream = File.OpenRead(name);
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = stream;
            bitmap.EndInit();
            stream.Close();
            stream.Dispose();

            return bitmap;
        }

        private void ChangeViewModels()
        {
            foreach (var bitmapImage in Images)
            {
                MagnifierViewModel = new ViewWithMagnifierViewModel(bitmapImage);
                Thread.Sleep(200);
            }

            Images.Clear();      // dame moznost k uklidu (pamet je porad alokovana)

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }

   
}
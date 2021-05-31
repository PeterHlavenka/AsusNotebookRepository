using System;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.CompilerServices;
using System.Windows;
using Xceed.Wpf.AvalonDock.Layout.Serialization;

namespace AvalonDockingManagerWithLayoutSerializer
{
    
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private XmlLayoutSerializer m_layoutSerializer;
        private readonly string m_fileName;

        public MainWindow()
        {
            InitializeComponent();
            m_layoutSerializer = new XmlLayoutSerializer(AvalonDockingManagerWithLayoutSerializerDockingManager);
            m_fileName = AvalonDockingManagerWithLayoutSerializerDockingManager.Name + ".xml";
        }
        
        private void LoadDocking()
        {
            try
            {
                using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForDomain())
                {
                    if (file.FileExists(m_fileName))
                    {
                        using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(m_fileName, FileMode.Open, file))
                        {
                            if (stream.Length > 0)
                            {
                                m_layoutSerializer.Deserialize(stream);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Can not load DockingManager: {ex.Message}");
            }
        }

        private void SaveDocking()
        {
            try
            {
                using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForDomain())
                {
                    using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(m_fileName, FileMode.Create, file))
                    {
                        m_layoutSerializer.Serialize(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Can not save DockingManager: {ex.Message}");
            }
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            LoadDocking();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            SaveDocking();
        }
    }
}
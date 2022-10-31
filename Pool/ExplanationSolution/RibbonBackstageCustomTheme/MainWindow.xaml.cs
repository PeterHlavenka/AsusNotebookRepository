using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using Microsoft.Win32;
using Syncfusion.SfSkinManager;
using Syncfusion.Windows.Shared;
using Syncfusion.Windows.Tools.Controls;

namespace RibbonBackstageCustomTheme
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public MainWindow()
        {
            InitializeComponent();   
        }

        private void CmbTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTheme.SelectedIndex == 0)
            {
                SfSkinManager.SetVisualStyle(this, VisualStyles.Office2019Colorful);
            }
            else if(cmbTheme.SelectedIndex == 1)
            {
                SfSkinManager.SetVisualStyle(this, VisualStyles.Office2016Colorful);
            }
            else if (cmbTheme.SelectedIndex == 2)
            {
                SfSkinManager.SetVisualStyle(this, VisualStyles.Office2013DarkGray);
            }
        }
    }
    public class Model : NotificationObject
    {
        private string themeName;

        public string ThemeName
        {
            get { return themeName; }
            set { themeName = value; }
        }


    }


    public class ViewModel
    {
        /// <summary>
        /// Maintains the ribbon properties.
        /// </summary>
        private static Ribbon ribbon = null;

        /// <summary>
        /// Maintains the richtextbox properties.
        /// </summary>
        private static RichTextBox rich = null;

        /// <summary>
        /// Maintains the file path to load file.
        /// </summary>
        string filePath = @"/Data/temp.rtf";


        /// <summary>
        /// Maintains the command for save as backstage item.
        /// </summary>
        private ICommand saveAsCommand;

        /// <summary>
        /// Maintains the command for open backstage item.
        /// </summary>
        private ICommand openCommand;

        /// <summary>
        /// Maintains the command for close backstage item.
        /// </summary>
        private ICommand closeCommand;

        /// <summary>
        /// Maintains the command for print backstage item.
        /// </summary>
        private ICommand printCommand;

        /// <summary>
        /// Gets or sets the command for document content <see cref="BackstageViewModel"/> class.
        /// </summary>
        public string FileContent { get; set; }

        /// <summary>
        /// Gets or sets the command for saveas backstage command button <see cref="BackstageViewModel"/> class.
        /// </summary>
        public ICommand SaveAsCommand
        {
            get
            {
                return saveAsCommand;
            }
        }

        /// <summary>
        /// Gets or sets the command for open backstage command button <see cref="BackstageViewModel"/> class.
        /// </summary>
        public ICommand OpenCommand
        {
            get
            {
                return openCommand;
            }
        }

        /// <summary>
        /// Gets or sets the command for close backstage command button <see cref="BackstageViewModel"/> class.
        /// </summary>
        public ICommand CloseCommand
        {
            get
            {
                return closeCommand;
            }
        }

        /// <summary>
        /// Gets or sets the command for print backstage command button <see cref="BackstageViewModel"/> class.
        /// </summary>
        public ICommand PrintCommand
        {
            get
            {
                return printCommand;
            }
        }

        private bool booleanToVisibility;
        public bool BooleanToVisibility
        {
            get { return booleanToVisibility; }
            set { booleanToVisibility = value; }
        }

        public ViewModel()
        {

            booleanToVisibility = true;
            saveAsCommand = new DelegateCommand<object>(SaveAsExecute);
            openCommand = new DelegateCommand<object>(OpenExecute);
            closeCommand = new DelegateCommand<object>(CloseExecute);
            printCommand = new DelegateCommand<object>(PrintExecute);
            System.Windows.Resources.StreamResourceInfo streamResourceInfo = Application.GetResourceStream(new Uri(filePath, UriKind.RelativeOrAbsolute));
            StreamReader streamReader = new StreamReader(streamResourceInfo.Stream);
            string readText = streamReader.ReadToEnd();
            FileContent = readText;

            Model m = new Model();
            m.ThemeName = "Material";
            themeList = new ObservableCollection<Model>();
            themeList.Add(new Model { ThemeName = "Office2019Colorful" });
            themeList.Add(new Model { ThemeName = "Office2016Colorful" });
            themeList.Add(new Model { ThemeName = "Office2013DarkGray" });      

        }

        /// <summary>
        /// Gets the ribbon property
        /// </summary>
        /// <param name="obj">Specifies the dependency object.</param>
        /// <returns></returns>
        public static Ribbon GetRibbon(DependencyObject obj)
        {
            return (Ribbon)obj.GetValue(RibbonProperty);
        }

        /// <summary>
        /// Sets the ribbon property
        /// </summary>
        /// <param name="obj">>Specifies the dependency object.</param>
        /// <param name="value">Specifies the ribbon value.</param>
        public static void SetRibbon(DependencyObject obj, Ribbon value)
        {
            obj.SetValue(RibbonProperty, value);
        }

        /// <summary>
        /// Dependency property.
        /// </summary>
        public static readonly DependencyProperty RibbonProperty =
            DependencyProperty.RegisterAttached("Ribbon", typeof(Ribbon), typeof(ViewModel), new FrameworkPropertyMetadata(OnRibbonChanged));

        /// <summary>
        /// Method used to access the ribbon control.
        /// </summary>
        /// <param name="obj">Specifies the dependency object.</param>
        /// <param name="args">Specifies the dependency property changes event args.</param>
        public static void OnRibbonChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ribbon = obj as Ribbon;
        }
   
        /// <summary>
        /// Method which is used to execute the saveas command.
        /// </summary>
        /// <param name="parameter">Specifies the parameter of saveas command.</param>
        private void SaveAsExecute(object parameter)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            FileStream rtfFile = null;
            saveFile.Filter = "FlowDocument Files (*.rtf)|*.rtf|All Files (*.*)|*.*";
            if (saveFile.ShowDialog().Value)
            {
                rtfFile = saveFile.OpenFile() as FileStream;
                XamlWriter.Save(rich, rtfFile);
                TextRange t = new TextRange(rich.Document.ContentStart, rich.Document.ContentEnd);
                t.Save(rtfFile, System.Windows.DataFormats.Rtf);
                rtfFile.Close();
            }
        }

        /// <summary>
        /// Method which is used to execute the print command.
        /// </summary>
        /// <param name="parameter">Specifies the parameter of print command.</para
        private void PrintExecute(object parameter)
        {
            PrintDialog printFile = new PrintDialog();
            printFile.ShowDialog();
            
        }

        /// <summary>
        /// Method which is used to execute the open command.
        /// </summary>
        /// <param name="parameter">Specifies the object type parameter.</param>
        private void OpenExecute(object parameter)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "FlowDocument Files (*.rtf)|*.rtf|All Files (*.*)|*.*";
            if (openFile.ShowDialog().Value)
            {
                string path = openFile.FileName;
                filePath = path;
                FileContent = File.ReadAllText(filePath);
                ribbon.HideBackStage();
            }
        }

        /// <summary>
        /// Method to hide the backstage for close backstage button.
        /// </summary>
        /// <param name="parameter">Specifies the object type parameter.</param>
        private void CloseExecute(object parameter)
        {
            ribbon.HideBackStage();
        }



        private ObservableCollection<Model> themeList;

        public ObservableCollection<Model> ThemeList
        {
            get { return themeList; }
            set { themeList = value; }
        }
    }

   public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Boolean && (bool)value)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Visibility && (Visibility)value == Visibility.Visible)
            {
                return true;
            }
            return false;
        }
    }


}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfUniverse.Entities;
using WpfUniverse.ViewModels;

namespace WpfUniverse.Views
{
    /// <summary>
    /// Interaction logic for PlanetView.xaml
    /// </summary>
    public partial class PlanetView : UserControl
    {
        public PlanetView()
        {
            InitializeComponent();

            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = MainWindow.PlanetViewModel;
            }
          
        }

        
    }
}

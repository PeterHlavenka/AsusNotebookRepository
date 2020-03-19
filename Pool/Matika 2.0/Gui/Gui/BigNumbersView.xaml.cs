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
using WpfAnimatedGif;

namespace Matika.Gui
{
    /// <summary>
    /// Interaction logic for BigNumbersView.xaml
    /// </summary>
    public partial class BigNumbersView : UserControl
    {
        public BigNumbersView()
        {
            InitializeComponent();
        }

        private void ShowingMonkey_OnAnimationLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is BigNumbersViewModel dc)
            {
                dc.MonkeyController = ImageBehavior.GetAnimationController(ShowingMonkey);
                ResultTextBox.Focus();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace DroidMatika
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            Equals.Text = " = ";
            //WhiteImg.Source = "white.jpg";

            OperationMenu = new List<MenuItem>
            {
                new MenuItem(Strings.Addition,  (d)=> new SumExample(d)),
                new MenuItem(Strings.Subtraction, (d)=> new DiffExample(d)),
                new MenuItem(Strings.Multiplication, (d)=> new ProductExample(d)),
                new MenuItem(Strings.Division, (d)=> new DivideExample(d))
            };
            
            AllowingMenu = new List<MenuItem>
            {
                new MenuItem(Strings.DecimalNumbers),
                new MenuItem(Strings.NegativeNumbers),
            };
            
            OnPropertyChanged(nameof(OperationMenu));
            OnPropertyChanged(nameof(AllowingMenu));
            
            Generate();
            UserResultLabel.Focus();
        }

        public List<MenuItem> OperationMenu { get; set; } 
        public List<MenuItem> AllowingMenu { get; set; } 
        public double UserResult { get; set; }
        private ExampleBase CurrentExample { get; set; }
        private List<MenuItem> AllowedOperations { get; set; } = new List<MenuItem>();
        public int SuccessCount { get; set; }
        public int FailedCount { get; set; }


        private void Generate()
        {
            UserResultLabel.TextColor = Color.Black;
            
            // Beru jen checknute operace
            AllowedOperations = OperationMenu.Any(d => d.IsChecked) ? OperationMenu.Where(d => d.IsChecked).ToList() : OperationMenu.Select(d => d).ToList();

            var randomInt = new Random().Next(0, AllowedOperations.Count);
            CurrentExample = AllowedOperations.ElementAt(randomInt).GetExample();

            Operator.Text = CurrentExample.Operator;

            FirstNumber.Text = CurrentExample.FirstNumber.ToString();
            SecondNumber.Text = CurrentExample.SecondNumber.ToString();

            UserResultLabel.Text = string.Empty;
        }

        // Potvrzeni vysledku uzivatelem, validace
        private void UserResultLabel_OnCompleted(object sender, EventArgs e)
        {
            if (decimal.TryParse(UserResultLabel.Text, out var userResult))
            {
                if (userResult == CurrentExample.Result)
                {
                    SuccessCount++;
                    CountLabel.TextColor = Color.Green;
                    CountLabel.Text = SuccessCount.ToString();
                    Generate();
                }
                else
                {
                    FailedCount++;
                    CountLabel.TextColor = Color.Red;
                    CountLabel.Text = FailedCount.ToString();
                    UserResultLabel.TextColor = Color.Red;
                }
            }

            UserResultLabel.Focus();
        }

        // Setnuti barvy pisma na cernou po vymazani textu
        private void UserResultLabel_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserResultLabel.Text)) UserResultLabel.TextColor = Color.Black;
        }

        // Tapnuti na MenuItem
        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            if (sender is StackLayout stackLayout)
            {
                var checkBox = (CheckBox) stackLayout.Children.First(d => d.GetType().IsAssignableFrom(typeof(CheckBox)));

                checkBox.IsChecked = !checkBox.IsChecked;
            }
        }

        // Po checknuti moznosti ve swipeView pregeneruj priklad
        private void CheckBox_OnCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            Generate();
        }

        // Po zavreni swipeView focusni at mame keyboard
        private void TapGestureRecognizer_OnGridTapped(object sender, EventArgs e)
        {
            UserResultLabel.Focus();
        }

        // Pri otevirani swipeView chceme sundat klavesnici
        private void SwipeView_OnSwipeStarted(object sender, SwipeStartedEventArgs e)
        {
            UserResultLabel.Unfocus();
        }

        // Zmena obtiznosti
        private void CustomSlider_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is CustomSlider slider)
            {
                OperationMenu?.ForEach(d => d.SetDifficulty((int)slider.Value));
            }
        }
    }
}
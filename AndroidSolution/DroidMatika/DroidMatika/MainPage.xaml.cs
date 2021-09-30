using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace DroidMatika
{
    public sealed partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Equals.Text = " = ";

            Menu = new List<MenuItem>
            {
                new MenuItem(Strings.Addition, ()=> new SumExample()),
                new MenuItem(Strings.Subtraction, ()=> new DiffExample()),
                new MenuItem(Strings.Multiplication, ()=> new ProductExample()),
                new MenuItem(Strings.Division, ()=> new DivideExample())
            };
            OnPropertyChanged(nameof(Menu));
            
            Generate();
            UserResultLabel.Focus();
        }

        public List<MenuItem> Menu { get; set; } 
        public double UserResult { get; set; }
        private ExampleBase CurrentExample { get; set; }
        private List<MenuItem> AllowedOperations { get; set; } = new List<MenuItem>();
        public int SuccessCount { get; set; }
        public int FailedCount { get; set; }

        private void Generate()
        {
            UserResultLabel.TextColor = Color.Black;
            
            // Beru jen checknute operace
            AllowedOperations = Menu.Any(d => d.IsChecked) ? Menu.Where(d => d.IsChecked).ToList() : Menu.Select(d => d).ToList();
            
            var randomInt = new Random().Next(0, AllowedOperations.Count);
            CurrentExample = AllowedOperations.ElementAt(randomInt).GetExample();

            Operator.Text = CurrentExample.Operator;

            FirstNumber.Text = CurrentExample.FirstNumber.ToString();
            SecondNumber.Text = CurrentExample.SecondNumber.ToString();

            UserResultLabel.Text = string.Empty;
            UserResultLabel.Focus();
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
            UserResultLabel.Unfocus();
            UserResultLabel.Focus();
        }
    }
}
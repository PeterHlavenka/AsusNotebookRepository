using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace DroidMatika
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            
            Equals.Text = " = ";
            Generate();
            UserResultLabel.Focus();
        }
        
        public double UserResult { get; set; }

        private ExampleBase CurrentExample { get; set; }

        private void Generate()
        {
            UserResultLabel.TextColor = Color.Black;

            var randomInt = new Random().Next(2, 4);
            switch (randomInt)
            {
                case 1: CurrentExample = new DiffExample();
                    break;
                case 2: CurrentExample = new DivideExample();
                    break;
                case 3: CurrentExample = new ProductExample();
                    break;
                case 4: CurrentExample = new SumExample();
                    break;
            }
           
            Operator.Text = CurrentExample.Operator;

            FirstNumber.Text = CurrentExample.FirstNumber.ToString();
            SecondNumber.Text = CurrentExample.SecondNumber.ToString();

            UserResultLabel.Text = string.Empty;
            UserResultLabel.Focus();
        }

        private void UserResultLabel_OnCompleted(object sender, EventArgs e)
        {
            if (decimal.TryParse(UserResultLabel.Text, out var userResult))
            {
                if (userResult == CurrentExample.Result)
                    Generate();
                else
                    UserResultLabel.TextColor = Color.Red;
            }

            UserResultLabel.Focus();
        }

        private void UserResultLabel_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserResultLabel.Text))
            {
                UserResultLabel.TextColor = Color.Black;
            }
        }
    }

    public class ExampleBase
    {
        public int FirstNumber { get; set; }
        public int SecondNumber { get; set; }
        public decimal Result { get; set; }
        public string Operator { get; set; }

        protected int CreateNumber(int minValue, int maxValue)
        {
            return new Random().Next(minValue, maxValue);
        }
    }

    public class DivideExample : ExampleBase
    {
        public DivideExample()
        {
            var first = CreateNumber(1, 10);
            var second = CreateNumber(5, 10);
            var result = first * second;

            // otoceni at tam nejsou desetinna mista
            FirstNumber = result;
            SecondNumber = first;
            Result = second;
            Operator = " : ";
        }
    }

    public class SumExample : ExampleBase
    {
        public SumExample()
        {
            FirstNumber = CreateNumber(5, 10);
            SecondNumber = CreateNumber(5, 10);
            Result = FirstNumber + SecondNumber;
            Operator = " + ";
        }
    }

    public class DiffExample : ExampleBase
    {
        public DiffExample()
        {
            var first = CreateNumber(5, 10);
            var second = CreateNumber(5, 10);

            FirstNumber = Math.Max(first, second);
            SecondNumber = Math.Min(first, second);
            Result = FirstNumber - SecondNumber;
            Operator = " - ";
        }
    }

    public class ProductExample : ExampleBase
    {
        public ProductExample()
        {
            FirstNumber = CreateNumber(5, 10);
            SecondNumber = CreateNumber(5, 10);
            Result = FirstNumber * SecondNumber;
            Operator = " . ";
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DroidMatika
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomSlider : ContentView
    {
        private double m_value;
        private List<decimal> Steps { get; set; }

        public double Value
        {
            get => m_value;
            set
            {
                m_value = value;
                OnPropertyChanged();
            }
        }

        public CustomSlider()
        {
            InitializeComponent();
            
            Slider.Maximum = 10;
            Slider.Minimum = 0;
            
            CreateSteps(0, 10, 1);
        }
        
        private void OnDragCompleted()
        {
            if (! Steps.Any())
            {
                return;
            }
            
            var value = (decimal)  Slider.Value;

            if (Steps.Any(d => d == value))
            {
                return;
            }

            var isLarger = Steps.Any(d => d > value);
            var isLower = Steps.Any(d => d < value);

            decimal current = 0;

            if (isLarger && isLower)
            {
                var next = Steps.First(d => d > value);
                var previous = Steps.Last(d => d < value);

                current = Math.Abs(value - previous) < Math.Abs(value - next) ? previous : next;
            }

            if (isLarger && !isLower) current = Steps.First(d => d > value);

            if (isLower && !isLarger) current = Steps.Last(d => d < value);

            Slider.Value = (double) current;
            Value = Slider.Value;
        }

        private void CreateSteps(decimal point, decimal maximum, decimal tick)
        {
            Steps = new List<decimal>();

            if (tick <= 0) {return;}
            
            while (point <= maximum)
            {
                Steps.Add(point);
                point += tick;
            }
        }

        private void Slider_OnDragCompleted(object sender, EventArgs e)
        {
            OnDragCompleted();
        }
    }
}
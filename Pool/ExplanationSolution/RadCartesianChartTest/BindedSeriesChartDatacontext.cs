using System;
using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;
using Telerik.Windows.Controls;

namespace RadCartesianChartTest
{
    public class BindedSeriesChartDatacontext : Screen
    {
        private readonly Random rnd = new Random();

        public BindedSeriesChartDatacontext(RadCartesianChart chart)
        {
            Chart = chart;
            Data = new List<Product>();
            GetData();

            AddEmptyValuesToData();

            chart.Width = Math.Min(Data.Count * 30, 500);

            chart.Zoom = new Size(Zoom, 1);
        }

        public RadCartesianChart Chart { get; set; }

        private int PocetMistNaOseX => 20;
        public int PocetNacitanychDatDoGrafu { get; set; } = 6;

        private int Zoom => PocetNacitanychDatDoGrafu / PocetMistNaOseX;

        public List<Product> Data { get; set; }

        private void AddEmptyValuesToData()
        {
            for (var i = Data.Count; i < PocetMistNaOseX; i++)
            {
                var product = new Product {Name = "" + i, QuantitySold = 0};
                Data.Add(product);
            }

            NotifyOfPropertyChange(nameof(Data));
        }

        private void GetData()
        {
            for (var i = 0; i < PocetNacitanychDatDoGrafu; i++)
            {
                var product = new Product {Name = "Product " + i, QuantitySold = rnd.Next(10, 99)};
                Data.Add(product);
            }
        }

        public void Reload()
        {
            Data = new List<Product>();
            GetData();
            AddEmptyValuesToData();
            Chart.Width = Math.Min(Data.Count * 30, 500);
            Chart.Zoom = new Size(Zoom, 1);
            NotifyOfPropertyChange(nameof(Data));
        }
    }


    public class Product
    {
        public string Name { get; set; }

        public double QuantitySold { get; set; }
    }
}
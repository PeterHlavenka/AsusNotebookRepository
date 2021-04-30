using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Caliburn.Micro;
using Telerik.Windows.Controls;

namespace RadCartesianChartTest
{
    public class BindedSeriesChartDatacontext : Screen
    {
        public BindedSeriesChartDatacontext(RadCartesianChart chart)
        {

            Chart = chart;
            Data = new List<Product>();
            GetData();
            
            // chart.MaxWidth = 1000;
            // var neco = chart.MaxWidth / Data.Count;
            //  chart.Zoom = new Size(neco, 1);

            AddEmptyValuesToData();
            
             chart.Width = Math.Min( Data.Count * 30, 500);
            
             chart.Zoom = new Size(Zoom, 1);
        }
        
        public RadCartesianChart Chart { get; set; }

        private int PocetMistNaOseX => 20;
        public int PocetNacitanychDatDoGrafu { get; set; } = 6;

        private int Zoom => PocetNacitanychDatDoGrafu / PocetMistNaOseX;

        private void AddEmptyValuesToData()
        {
            for (int i = Data.Count; i < PocetMistNaOseX; i++)
            {
                Product product = new Product {Name = ""+i, QuantitySold = 0};
                Data.Add(product);
            }
            NotifyOfPropertyChange(nameof(Data));
        }

        public List<Product> Data { get; set; }
        Random rnd = new Random();

        private void GetData()
        {
            for (int i = 0; i < PocetNacitanychDatDoGrafu; i++)
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
            Chart.Width = Math.Min( Data.Count * 30, 500);
            Chart.Zoom = new Size(Zoom, 1);
            NotifyOfPropertyChange(nameof(Data));
        }
    }



    public class Product
    {
        public string Name
        {
            get;
            set;
        }
        public double QuantitySold
        {
            get;
            set;
        }
    }
}
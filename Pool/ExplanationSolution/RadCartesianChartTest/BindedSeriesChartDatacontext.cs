using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Telerik.Windows.Controls;

namespace RadCartesianChartTest
{
    public class BindedSeriesChartDatacontext
    {
        public BindedSeriesChartDatacontext()
        {
            Data = new ObservableCollection<Product>();
            GetData();
        }

        public ObservableCollection<Product> Data { get; set; }
        Random rnd = new Random();

        private void GetData()
        {
            for (int i = 0; i < 10; i++)
            {
                Product product = new Product();
                product.Name = "Product " + i;
                product.QuantitySold = rnd.Next(10, 99);
                Data.Add(product);
            }
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
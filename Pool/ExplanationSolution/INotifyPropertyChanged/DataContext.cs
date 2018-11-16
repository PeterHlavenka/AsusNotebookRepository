using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using INotifyPropertyChanged.Annotations;
using Telerik.Windows.Controls;
using Telerik.Windows.Diagrams.Core;

namespace INotifyPropertyChanged
{
    public class DataContext: System.ComponentModel.INotifyPropertyChanged
    {
        private int m_count = 758;
        private ObservableCollection<Item> m_observableItems = new ObservableCollection<Item>();

        

        public DataContext(RadGridView radGridView)
        {
            radGridView.ItemsSource = ObservableItems;

            var worker = new BackgroundWorker();
            worker.DoWork += DoWork;
            worker.RunWorkerAsync();
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            using (var file = new StreamReader(@"C:\Pool\Admosphere\temp\Pricing RadGridView export do excelu\TelerikExportDemoFromTextFile\TelerikExportDemoFromTextFile\text.txt"))
            {
                var line = file.ReadLine();

                while ((line = file.ReadLine()) != null)
                {
                    var result = line.Split('\t');

                    var item = new Item
                    {
                        ComputationResultState = result[0],
                        // AdvertisedFromDate = Convert.ToDateTime(GetSubstring(result[1])),
                        DayOfWeek = result[2],
                        MediumName = result[3],
                        // Start = Convert.ToDateTime(GetSubstring(result[4])),
                        // End = Convert.ToDateTime(GetSubstring(result[5])),
                        Footage = GetSubstring(result[6]) == string.Empty ? 0 : double.Parse(GetSubstring(result[6])),
                        AdvertisementType = GetSubstring(result[7]),
                        Placement = GetSubstring(result[8]),
                        PriceValue = GetSubstring(result[9]) == string.Empty ? (decimal?)null : Convert.ToDecimal(GetSubstring(result[9])),
                        ComputationResultValue = GetSubstring(result[10]) == string.Empty ? 0 : double.Parse(GetSubstring(result[10])),
                        ComputationResultFailureType = GetSubstring(result[11]),
                        SponsoringsPerAdvertiserPerSponsoredProgramme = GetSubstring(result[12]) == string.Empty ? (int?)null : int.Parse(result[12].Substring(1, result[12].Length - 2))
                    };

                    List.Add(item);
                }

               // MessageBox.Show("Loaded");
                             
                Application.Current.Dispatcher.BeginInvoke(new Action(delegate
                {
                   // ObservableItems = new ObservableCollection<Item>(List);     
                    ObservableItems.AddRange(List);
                }));
            }

            string GetSubstring(string result)
            {
                return result.Substring(1, result.Length - 2);
            }
        }


        
        public List<Item> List { get; set; } = new List<Item>();

        public int Count
        {
            get => m_count;
            set
            {
                m_count = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Item> ObservableItems
        {
            get => m_observableItems;
            set
            {
                m_observableItems = value; 
               // OnPropertyChanged(nameof(ObservableItems));
            }
        }


        #region INotifyRegion
        


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        } 
        #endregion

        public class Item
        {
            public string ComputationResultState { get; set; } //
            public DateTime AdvertisedFromDate { get; set; }
            public string DayOfWeek { get; set; } //
            public string MediumName { get; set; }
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
            public double? Footage { get; set; }
            public string AdvertisementType { get; set; }
            public string Placement { get; set; }
            public double ComputationResultValue { get; set; }
            public string ComputationResultFailureType { get; set; }
            public decimal? BlockRating { get; set; }
            public decimal? Rating { get; set; }
            public string BlockCode { get; set; }
            public decimal? PriceValue { get; set; }
            public decimal? DeclaredPriceDiff { get; set; }
            public decimal? TvLogPriceValue { get; set; }
            public string BlockIdent { get; set; }
            public string SponsoredProgrammeName { get; set; }
            public bool? IsSponsoredProgrammeUserDefined { get; set; }
            public string ProgrammeBeforeName { get; set; }
            public string ProgrammeAfterName { get; set; }
            public int? SponsoredProgrammeLength { get; set; }
            public int? SponsoringsPerAdvertiserPerSponsoredProgramme { get; set; }
            public string OwnerName { get; set; }
            public string ProductBrandName { get; set; }
        }


      
    }
}
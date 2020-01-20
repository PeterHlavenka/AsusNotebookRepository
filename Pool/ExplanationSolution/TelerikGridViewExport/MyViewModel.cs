using System;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;
using Telerik.Windows.Diagrams.Core;
using ContextMenu = System.Windows.Controls.ContextMenu;
using MenuItem = System.Windows.Controls.MenuItem;

namespace TelerikGridViewExport
{
    public class MyViewModel : ViewModelBase
    {
        private ObservableCollection<Club> clubs;

        public ObservableCollection<Club> Clubs
        {
            get
            {
                if (clubs == null)
                {
                    clubs = CreateClubs();
                }

                return clubs;
            }
        }

        private CompositeFilterDescriptorCollection Descriptors { get; set; }

        private ObservableCollection<Club> CreateClubs()
        {
            var clubs = new ObservableCollection<Club>();
            Club club;

            club = new Club("Liverpool", new DateTime(1892, 1, 1), 45362);
            clubs.Add(club);

            club = new Club("Manchester Utd.", new DateTime(1878, 1, 1), 76212);
            clubs.Add(club);

            club = new Club("Chelsea", new DateTime(1905, 1, 1), 42055);
            clubs.Add(club);

            return clubs;
        }


        public void Filtered(object sender)
        {
            var radGrid = sender as RadGridView;

            if (radGrid?.FilterDescriptors.Count == 0)
            {
                return;
            }

            Descriptors = new CompositeFilterDescriptorCollection();

            radGrid?.FilterDescriptors.ForEach(d => Descriptors.Add(d));
        }

        public void SetFilters(object source)
        {
            if (source is MenuItem menuItem)
            {
                if (menuItem.Parent is ContextMenu parent)
                {
                    var radGrid = parent.PlacementTarget as RadGridView;


                    radGrid?.FilterDescriptors.SuspendNotifications();

                    radGrid?.FilterDescriptors.AddRange(Descriptors);

                    radGrid?.FilterDescriptors.ResumeNotifications();
                }
            }
        }


        public void ExportToExcel(object source)
        {
            if (source is MenuItem menuItem)
            {
                if (menuItem.Parent is ContextMenu parent)
                {
                    var radGrid = parent.PlacementTarget as RadGridView;

                    const string extension = "xls";

                    var dialog = new SaveFileDialog
                    {
                        DefaultExt = extension,
                        Filter = string.Format("{1} files (.{0})|.{0}|All files (.)|.", extension, "Excel"),
                        FilterIndex = 1
                    };

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            using (var stream = dialog.OpenFile())
                            {
                                radGrid?.Export(stream,
                                    new GridViewExportOptions
                                    {
                                        Format = ExportFormat.ExcelML,
                                        ShowColumnHeaders = true,
                                        ShowColumnFooters = true,
                                        ShowGroupFooters = false
                                    });
                            }
                        }

                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message);
                        }
                    }
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;

namespace ExtendedRadGridViewPlusBehaviors
{
    public class MyViewModel : ViewModelBase
    {
        private ObservableCollection<Club> clubs;

        public ObservableCollection<Club> Clubs
        {
            get
            {
                if (this.clubs == null)
                {
                    this.clubs = this.CreateClubs();
                }

                return this.clubs;
            }
        }

        private ObservableCollection<Club> CreateClubs()
        {
            ObservableCollection<Club> clubs = new ObservableCollection<Club>();
            Club club;

            club = new Club("Liverpool", new DateTime(1892, 1, 1), 45362);
            clubs.Add(club);

            club = new Club("Manchester Utd.", new DateTime(1878, 1, 1), 76212);
            clubs.Add(club);

            club = new Club("Chelsea", new DateTime(1905, 1, 1), 42055);
            clubs.Add(club);

            return clubs;
        }
    }
}

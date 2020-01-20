using System;
using System.Collections.ObjectModel;
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
                if (clubs == null)
                {
                    clubs = CreateClubs();
                }

                return clubs;
            }
        }

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
    }
}
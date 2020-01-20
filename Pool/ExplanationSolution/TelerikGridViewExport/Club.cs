using System;
using System.ComponentModel;

namespace TelerikGridViewExport
{
    public class Club : INotifyPropertyChanged
    {
        private DateTime established;

        private string name;
        private int stadiumCapacity;

        public Club(string name, DateTime established, int stadiumCapacity)
        {
            this.name = name;
            this.established = established;
            this.stadiumCapacity = stadiumCapacity;
        }

        public string Name
        {
            get => name;
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public DateTime Established
        {
            get => established;
            set
            {
                if (value != established)
                {
                    established = value;
                    OnPropertyChanged("Established");
                }
            }
        }

        public int StadiumCapacity
        {
            get => stadiumCapacity;
            set
            {
                if (value != stadiumCapacity)
                {
                    stadiumCapacity = value;
                    OnPropertyChanged("StadiumCapacity");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }
}
using Caliburn.Micro;

namespace WpfUniverse.Gui.ViewModels
{
    public abstract class ViewModelBase : PropertyChangedBase
    {
        //protected void OnPropertyChanged(string property)
        //{
        //    OnPropertyChangedEx(property);
        //    OnPropertyChangeInternal(property);
        //}

        //protected virtual void OnPropertyChangeInternal(string property){}

        //protected void OnPropertyChangedEx(string property)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        //}
    }
}
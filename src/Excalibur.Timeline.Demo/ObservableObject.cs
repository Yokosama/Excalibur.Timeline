using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Excalibur.Timeline.Demo
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyOfPropertyChange(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool SetProperty<T>(ref T reference, T value, [CallerMemberName] in string propertyName = "")
        {
            if (!Equals(reference, value))
            {
                reference = value;
                NotifyOfPropertyChange(propertyName);
                return true;
            }

            return false;
        }
    }
}

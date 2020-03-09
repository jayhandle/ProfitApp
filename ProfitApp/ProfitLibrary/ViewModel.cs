using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProfitLibrary
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        //public void Log(string msg, LogState stage = LogState.Info)
        //{
        //    Logger.Log(this.GetType().Name, msg, stage);
        //}

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion INotifyPropertyChanged Members
    }
}
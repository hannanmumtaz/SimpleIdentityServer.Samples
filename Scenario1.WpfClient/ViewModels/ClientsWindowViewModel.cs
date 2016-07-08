using System.ComponentModel;

namespace Scenario1.WpfClient.ViewModels
{
    internal class ClientsWindowViewModel : INotifyPropertyChanged
    {
        #region Fields

        private string _subject;

        private string _name;

        #endregion

        #region Properties

        public string Subject
        {
            get
            {
                return _subject;
            } 
            set
            {
                _subject = value;
                NotifyPropertyChanged("Subject");
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private methods

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion
    }
}

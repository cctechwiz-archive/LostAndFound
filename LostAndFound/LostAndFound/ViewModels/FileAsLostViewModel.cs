using LostAndFound.Services.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LostAndFound.ViewModels
{
    class FileAsLostViewModel : INotifyPropertyChanged
    {
        private ICommand _fileAsLostCommand;

        private string _name = "";
        private string _description = "";
        private string _email = "";
        private string _phoneNumber = "";
        private DateTime _dateLost;

        public event PropertyChangedEventHandler PropertyChanged;

        public FileAsLostViewModel()
        {
            _fileAsLostCommand = new RelayCommand(obj => {
                //if (_canExecute)
                //{
                //    // create new lost item
                //}
            });
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
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
                RaisePropertyChanged("Name");
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                RaisePropertyChanged("Description");
            }
        }

        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                RaisePropertyChanged("Email");
            }
        }

        public string PhoneNumber
        {
            get
            {
                return _phoneNumber;
            }
            set
            {
                _phoneNumber = value;
                RaisePropertyChanged("PhoneNumber");
            }
        }

    }
}

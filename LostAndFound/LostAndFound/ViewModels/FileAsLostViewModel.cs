using LostAndFound.Services.Commands;
using LostAndFound.Services.Providers;
using LostAndFound.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LostAndFound.ViewModels
{
    class FileAsLostViewModel
    {
        private ICommand _fileAsLostCommand;
        private LostItemProvider _lostItemProvider;

        private string _name = "";
        private string _description = "";
        private string _email = "";
        private string _phoneNumber = "";
        private DateTime _dateLost;


        public FileAsLostViewModel()
        {
            _fileAsLostCommand = new RelayCommand(SubmitCreateRequest);
            _lostItemProvider = new LostItemProvider();
        }


        private void SubmitCreateRequest(object obj)
        {
            if (String.IsNullOrWhiteSpace(Name)) {
                this.ShowWarning("A name is required!");
            } else if (String.IsNullOrWhiteSpace(Email) && String.IsNullOrWhiteSpace(PhoneNumber)) {
                this.ShowWarning("Please enter an email address OR a phone number by which to contact the owner!");
            } else {
                _lostItemProvider.CreateLostItem(DateTime.Now, Description, "", "", PhoneNumber, Email, "");
                Window window = (Window)obj;
                window.Close();
            }
        }

        private void ShowWarning(string message)
        {
            GenericDialog dialog = new GenericDialog(message);
            dialog.Owner = Application.Current.MainWindow;
            dialog.ShowDialog();
        }


        public ICommand FileAsLostCommand
        {
            get { return _fileAsLostCommand; }
            set { _fileAsLostCommand = value; }
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
            }
        }


        public DateTime DateLost
        {
            get 
            { 
                return _dateLost;
            }
            set
            {
                _dateLost = value;
            }
        }


    }
}

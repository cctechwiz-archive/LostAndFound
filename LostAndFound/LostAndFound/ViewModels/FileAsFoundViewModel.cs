using LostAndFound.Services.Commands;
using LostAndFound.Services.Providers;
using LostAndFound.Views.Dialogs;
using LostAndFound.Models;
using LostAndFound.Views.Windows;
using LostAndFound.Views.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LostAndFound.Views;

namespace LostAndFound.ViewModels
{
    class FileAsFoundViewModel
    {
        private ICommand _fileAsFoundCommand;
        private FoundItemProvider _foundItemProvider;

        private string _name = "";
        private string _description = "";
        private string _foundBy = "";
        private string _employeeName = "";
        private string _location = "";
        private DateTime _dateFound;


        public FileAsFoundViewModel()
        {
            _fileAsFoundCommand = new RelayCommand(SubmitCreateRequest);
            _foundItemProvider = new FoundItemProvider();
        }


        private void SubmitCreateRequest(object obj)
        {
            if (String.IsNullOrWhiteSpace(Name)) {
                this.ShowWarning("A name is required!");
            } else {
                _foundItemProvider.CreateFoundItem(DateTime.Now, Description, Location, FoundBy, EmployeeName);
                ReportFoundView.reloadList = true;
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


        public ICommand FileAsFoundCommand
        {
            get { return _fileAsFoundCommand; }
            set { _fileAsFoundCommand = value; }
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

        public string Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }

        public string FoundBy
        {
            get
            {
                return _foundBy;
            }
            set
            {
                _foundBy = value;
            }
        }


        public string EmployeeName
        {
            get
            {
                return _employeeName;
            }
            set
            {
                _employeeName = value;
            }
        }

        public DateTime DateFound
        {
            get 
            { 
                return _dateFound;
            }
            set
            {
                _dateFound = value;
            }
        }


    }
}

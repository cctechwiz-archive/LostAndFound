using LostAndFound.Services.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LostAndFound.ViewModels
{
    class FileAsLostViewModel
    {
        private ICommand _fileAsLostCommand;
        private bool _canExecute = false;

        public FileAsLostViewModel()
        {
            _fileAsLostCommand = new RelayCommand(obj => {
                if (_canExecute)
                {
                    // create new lost item
                }
            });
        }
    }
}

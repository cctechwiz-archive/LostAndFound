using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LostAndFound.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for WarningDialog.xaml
    /// </summary>
    public partial class GenericDialog : MetroWindow
    {
        private string _message;

        public GenericDialog(string message)
        {
            if (String.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Dialog message should not be null or empty.");
            }
            _message = message;

            InitializeComponent();
        }

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }
    }
}

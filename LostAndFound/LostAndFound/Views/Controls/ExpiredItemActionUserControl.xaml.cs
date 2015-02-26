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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LostAndFound.Views.Controls
{
    /// <summary>
    /// Interaction logic for ExpiredItemActionUserControl.xaml
    /// </summary>
    public partial class ExpiredItemActionUserControl : UserControl
    {
        public ExpiredItemActionUserControl()
        {
            InitializeComponent();
        }

        private void disposed_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

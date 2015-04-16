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
using MahApps.Metro.Controls;

namespace LostAndFound
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        void clickHelp(Object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"help.pdf");
        }

        public void getFiles()
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.
        }
    }
}

using LostAndFound.Models;
using LostAndFound.Services.Providers;
using LostAndFound.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace LostAndFound.Views
{
    public partial class SearchForLostItemView : UserControl
    {
        private ObservableCollection<LostItem> _lostItems;

        public SearchForLostItemView()
        {
            LostItemProvider provider = new LostItemProvider();
            _lostItems = new ObservableCollection<LostItem>(provider.GetLostItems());
            InitializeComponent();
            LostItemListView.ItemsSource = _lostItems;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FileAsLostWindow fileAsLostWindow = new FileAsLostWindow();
            fileAsLostWindow.Owner = Application.Current.MainWindow;
            fileAsLostWindow.Show();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Search
        }
    }
}

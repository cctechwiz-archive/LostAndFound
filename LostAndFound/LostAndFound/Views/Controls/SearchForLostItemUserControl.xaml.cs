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
        private LostItemProvider _lostItemProvider;
        private List<LostItem> _cachedLostItems;
        private List<LostItem> _lostItems;

        public SearchForLostItemView()
        {
            _lostItemProvider = new LostItemProvider();
            _cachedLostItems = _lostItemProvider.GetLostItems();
            _lostItems = new List<LostItem>(_lostItemProvider.GetLostItems());
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
            TextBox textBox = (TextBox)sender;
            if (null != textBox) {
                if (String.IsNullOrEmpty(textBox.Text))
                {
                    _lostItems = _cachedLostItems;
                }
                else
                {
                    IEnumerable<LostItem> query = _cachedLostItems.Where(i => i.Name.Contains(textBox.Text));
                    List<LostItem> asList = query.ToList();
                    if (asList.Count == 0)
                    {
                        query = _lostItemProvider.GetLostItems().Where(i => i.Name.Contains(textBox.Text));
                        asList = query.ToList();
                    }
                    _lostItems = new List<LostItem>(asList);
                }
            }
            if (null != LostItemListView)
            {
                LostItemListView.ItemsSource = _lostItems;
            }
        }
    }
}

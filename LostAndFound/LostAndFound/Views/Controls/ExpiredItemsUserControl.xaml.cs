using LostAndFound.Models;
using LostAndFound.Services.Providers;
using LostAndFound.Views.Windows;
using LostAndFound.Views.Controls;
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

namespace LostAndFound.Views
{
    /// <summary>
    /// Interaction logic for ExpiredItemsView.xaml
    /// </summary>
    public partial class ExpiredItemsView : UserControl
    {
        private int farBack;
        private static List<FoundItem> _cachedFoundItems;
        public static List<FoundItem> _foundItems;
        public static bool reloadList;
        public static List<MyItem> _selectedItemsList;


        public ExpiredItemsView()
        {
            farBack = -1;
            reloadList = false;
            FoundItemProvider _foundItemProvider = new FoundItemProvider();
            _cachedFoundItems = _foundItemProvider.GetFoundItems();
            _foundItems = new List<FoundItem>(_foundItemProvider.GetFoundItems());
            InitializeComponent();
        }

        public void selectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (reloadList)
            {
                reloadListView();
            }
            else
            {
                reloadExpiredList();
            }
        }

        private void reloadExpiredList()
        {
            string text1 = dateDDL.SelectedValue.ToString();
            if (text1.Equals("7 days ago")) farBack = 7;
            if (text1.Equals("30 days ago")) farBack = 30;
            if (text1.Equals("60 days ago")) farBack = 60;
            if (text1.Equals("90 days ago")) farBack = 90;
            if (text1.Equals("The beginning of time")) farBack = 0;
            ExpiredItemListView.Items.Clear();
            DateTime dateTime = DateTime.Now;
            foreach (var item in _foundItems)
            {
                TimeSpan ts = dateTime - item.DateReported;
                int td = ts.Days;
                if (td >= farBack)
                {
                    ExpiredItemListView.Items.Add(createMyItem(item));
                }
            }
        }

        private void reloadListView()
        {
            reloadList = false;
            var foundItemProvider = new FoundItemProvider();
            _foundItems = null;
            _foundItems = foundItemProvider.GetFoundItems();
            reloadExpiredList();
        }

        private MyItem createMyItem(FoundItem item)
        {
            String tmp1 = "";
            foreach (var desc in item.DescriptionTags)
            {
                tmp1 += desc.Name + " ";
            }
            String tmp2 = "";
            foreach (var desc in item.LocationTags)
            {
                tmp2 += desc.Name + " ";
            }
            MyItem t;
            t = new MyItem { desc = tmp1, loc = tmp2, date = item.DateReported.ToString("MM/dd/yyyy"), name = item.Name, isVisible = true, isSelected = false };
            return t;
        }

        private void itemClicked(object sender, SelectionChangedEventArgs e)
        {
            if (reloadList)
            {
                reloadListView();
            }
            else
            {
                _selectedItemsList = new List<MyItem>();
                foreach (MyItem i in ExpiredItemListView.Items)
                {
                    bool test = i.isSelected;
                    if (i.isSelected)
                    {
                        _selectedItemsList.Add(i);
                    }
                }
            }
        }
    }
}

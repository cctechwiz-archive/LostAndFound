using LostAndFound.Models;
using LostAndFound.Services.Providers;
using LostAndFound.Views.Windows;
using LostAndFound.Views.Controls;
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
using System.Diagnostics;

namespace LostAndFound.Views
{
    public partial class SearchForLostItemView : UserControl
    {
        private static List<LostItem> _cachedLostItems;
        public static List<LostItem> _lostItems;
        public static bool reloadList;

        public SearchForLostItemView()
        {
            reloadList = false;
            LostItemProvider _lostItemProvider = new LostItemProvider();
            _cachedLostItems = _lostItemProvider.GetLostItems();
            _lostItems = new List<LostItem>(_lostItemProvider.GetLostItems());
            InitializeComponent();
            foreach (var item in _lostItems)
            {
                LostItemListView.Items.Add(createMyItem(item));
            }
        }

        private MyItem createMyItem(LostItem item)
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
            t = new MyItem { desc = tmp1, loc = tmp2, date = item.DateReported.ToString("MM/dd/yyyy"), name = item.Name,isVisible = true, isSelected = false };
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
                if (e.AddedItems.Count > 0)
                {
                    var item = (MyItem)e.AddedItems[0];
                    LostItemDetailsUserControl.ItemName.Text = item.name;
                    LostItemDetailsUserControl.itemBasic = item;
                    LostItemDetailsUserControl.lostItems = _lostItems;
                }
            }
        }

        private void reloadListView()
        {
            reloadList = false;
            var lostItemProvider = new LostItemProvider();
            _lostItems = null;
            _lostItems = lostItemProvider.GetLostItems();
            LostItemListView.Items.Clear();
            foreach (var item in _lostItems)
            {
                LostItemListView.Items.Add(createMyItem(item));
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (reloadList)
            {
                reloadListView();
            }
            else
            {
                TextBox textBox = (TextBox)sender;
                if (null != textBox)
                {
                    foreach (var item in LostItemListView.Items)
                    {
                        MyItem i = (MyItem)item;
                        i.isSelected = false;
                    }
                    LostItemListView.Items.Clear();
                    foreach (var item in _lostItems)
                    {
                        MyItem mItem = createMyItem(item);
                        if (mItem.name.ToLower().Contains(textBox.Text.ToLower()))
                        {
                            LostItemListView.Items.Add(mItem);
                        }
                    }
                }
            }
        }
    }
}

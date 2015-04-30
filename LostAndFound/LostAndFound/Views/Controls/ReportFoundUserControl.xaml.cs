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
    /// Interaction logic for ReportFoundView.xaml
    /// </summary>
    public partial class ReportFoundView : UserControl
    {
        private static List<FoundItem> _cachedFoundItems;
        public static List<FoundItem> _foundItems;
        public static bool reloadList;
        private MyItem itemBasic;

        public ReportFoundView()
        {
            reloadList = false;
            FoundItemProvider _foundItemProvider = new FoundItemProvider();
            _cachedFoundItems = _foundItemProvider.GetFoundItems();
            _foundItems = new List<FoundItem>(_foundItemProvider.GetFoundItems());
            InitializeComponent();
            foreach (var item in _foundItems)
            {
                FoundItemListView.Items.Add(createMyItem(item));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FileAsLostWindow fileAsLostWindow = new FileAsLostWindow();
            fileAsLostWindow.Owner = Application.Current.MainWindow;
            fileAsLostWindow.Show();
            SearchForLostItemView.reloadList = true;
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            if (itemBasic != null)
            {
                foreach (var item in _foundItems)
                {
                    DescriptionTag[] newDescs = item.DescriptionTags.ToArray();
                    string newDesc = "";
                    for (int i = 0; i < newDescs.Length; i++)
                    {
                        newDesc += newDescs[i].Name;
                        if (i < newDescs.Length - 1) newDesc += " ";
                    }
                    LocationTag[] newLocs = item.LocationTags.ToArray();
                    string newLoc = "";
                    for (int i = 0; i < newLocs.Length; i++)
                    {
                        newLoc += newLocs[i].Name;
                        if (i < newLocs.Length - 1) newLoc += " ";
                    }
                    if (itemBasic.date == item.DateReported.ToString("MM/dd/yyyy") && itemBasic.desc.Contains(newDesc) && itemBasic.loc.Contains(newLoc))
                    {
                        var dateTime = new DateTime(1, 1, 1);
                        FoundItem fi = new FoundItem(dateTime, item.DescriptionTags, item.LocationTags, item.Name, item.Employee);
                        FoundItemProvider fip = new FoundItemProvider();
                        fip.UpdateFoundItem(item, fi);
                        dateTime = DateTime.Now;
                        reloadList = true;
                        ExpiredItemsView.reloadList = true;
                        var disposedItemProvider = new DisposedItemProvider();
                        disposedItemProvider.CreateDisposedItem(item.DateReported, dateTime, newDesc, newLoc, "", "", "", "", "");
                    }
                }
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
                    foreach (var item in FoundItemListView.Items)
                    {
                        MyItem i = (MyItem)item;
                        i.isSelected = false;
                    }
                    FoundItemListView.Items.Clear();
                    foreach (var item in _foundItems)
                    {
                        MyItem mItem = createMyItem(item);
                        if (mItem.desc.Contains(textBox.Text))
                        {
                            FoundItemListView.Items.Add(mItem);
                        }
                    }
                }
            }
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
            t = new MyItem { desc = tmp1, loc = tmp2, date = item.DateReported.ToString("MM/dd/yyyy"), isVisible = true, isSelected = false };
            return t;
        }

        private void reloadListView()
        {
            reloadList = false;
            _foundItems = null;
            FoundItemProvider fip = new FoundItemProvider();
            _foundItems = fip.GetFoundItems();
            FoundItemListView.Items.Clear();
            foreach (var item in _foundItems)
            {
                FoundItemListView.Items.Add(createMyItem(item));
            }
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
                    itemBasic = item;
                }
            }
        }


    }
}

using LostAndFound.Models;
using LostAndFound.Services.Providers;
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

        private void Close(object sender, RoutedEventArgs e)
        {

        }

        private void itemClicked(object sender, RoutedEventArgs e)
        {
            FoundItemProvider _foundItemProvider = new FoundItemProvider();
            List<FoundItem> _foundItems = new List<FoundItem>(_foundItemProvider.GetFoundItems());
            foreach (MyItem itemBasic in ExpiredItemsView._selectedItemsList)
            {
                ReportFoundView.reloadList = true;
                ExpiredItemsView.reloadList = true;
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
                            var disposedItemProvider = new DisposedItemProvider();
                            disposedItemProvider.CreateDisposedItem(item.DateReported, dateTime, newDesc, newLoc, "", "", "", "", disposed.Text);
                        }
                    }
                }
            }
        }
    }
}

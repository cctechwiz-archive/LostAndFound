using System;
using LostAndFound.Models;
using LostAndFound.Services.Providers;
using LostAndFound.Views.Windows;
using LostAndFound.Views.Controls;
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
    /// Interaction logic for LostItemDetailsUserControl.xaml
    /// </summary>
    public partial class LostItemDetailsUserControl : UserControl
    {
        public static TextBlock ItemName;
        public static MyItem itemBasic;
        public static List<LostItem> lostItems;

        public LostItemDetailsUserControl()
        {
            InitializeComponent();
            ItemName = itemName;
            itemBasic = new MyItem();
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
            t = new MyItem { desc = tmp1, loc = tmp2, date = item.DateReported.ToString("MM/dd/yyyy"), isVisible = true, isSelected = false };
            return t;
        }

        public static class itemtext
        {
            public static String iText;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FileAsFoundWindow fileAsFoundWindow = new FileAsFoundWindow();
            fileAsFoundWindow.Owner = Application.Current.MainWindow;
            fileAsFoundWindow.Show();
            ReportFoundView.reloadList = true;
            ExpiredItemsView.reloadList = true;
        }
        private void claimClick(object sender, RoutedEventArgs e)
        {
            foreach (var item in lostItems)
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
                    var lostItemProvider = new LostItemProvider();
                    var dateTime = new DateTime(1, 1, 1);
                    LostItem li = new LostItem(dateTime, item.DescriptionTags, item.LocationTags, item.Name, item.Employee);
                    lostItemProvider.UpdateLostItem(item, li);
                    dateTime = DateTime.Now;
                    SearchForLostItemView.reloadList = true;
                }
            }
        }
    }
}

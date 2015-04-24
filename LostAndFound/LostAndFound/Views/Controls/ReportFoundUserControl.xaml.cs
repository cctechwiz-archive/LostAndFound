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

namespace LostAndFound.Views
{
    /// <summary>
    /// Interaction logic for ReportFoundView.xaml
    /// </summary>
    public partial class ReportFoundView : UserControl
    {
        public ReportFoundView()
        {
            InitializeComponent();
            // Testing Providers
            /*
            var dateTime = new DateTime(2012, 5, 12);
            var foundItemProvider = new FoundItemProvider();
            var lostItemProvider = new LostItemProvider();
            var disposedItemProvider = new DisposedItemProvider();
            var FoundItems = foundItemProvider.GetFoundItems();
            var LostItems = lostItemProvider.GetLostItems();
            var DisposedItems = disposedItemProvider.GetDisposedItems();

            DescriptionTag d1 = new DescriptionTag("dtag1");
            DescriptionTag d2 = new DescriptionTag("dtag2");
            String dTagString = "dtag1 dtag2";
            List<DescriptionTag> dTags = new List<DescriptionTag>();
            dTags.Add(d1);
            dTags.Add(d2);
            LocationTag l1 = new LocationTag("ltag1");
            LocationTag l2 = new LocationTag("ltag2");
            String lTagString = "ltag1 ltag2";
            List<LocationTag> lTags = new List<LocationTag>();
            lTags.Add(l1);
            lTags.Add(l2);
            var fi = new FoundItem(dateTime, dTags, lTags, "Joe", "Rob");
            var li = new LostItem(dateTime, dTags, lTags, "Joe", "Rob");
            var di = new DisposedItem(dateTime, dateTime, dTags, lTags, "Joe", "na", "na", "Rob", "Incinerator");
            DescriptionTag d1a = new DescriptionTag("dtag1a");
            DescriptionTag d2a = new DescriptionTag("dtag2a");
            List<DescriptionTag> dTagsa = new List<DescriptionTag>();
            dTagsa.Add(d1a);
            dTagsa.Add(d2a);
            LocationTag l1a = new LocationTag("ltag1a");
            LocationTag l2a = new LocationTag("ltag2a");
            List<LocationTag> lTagsa = new List<LocationTag>();
            lTagsa.Add(l1a);
            lTagsa.Add(l2a);
            var dateTime2 = new DateTime(2014, 8, 9);
            var fi2 = new FoundItem(dateTime2, dTagsa, lTagsa, "Joey", "Robby");
            var li2 = new LostItem(dateTime2, dTagsa, lTagsa, "Joey", "Robby");
            var di2 = new DisposedItem(dateTime2, dateTime2, dTagsa, lTagsa, "Joey", "na", "na", "Robby", "Trash");

            var dateTime3 = new DateTime(1, 1, 1);
            var fi3 = new FoundItem(dateTime3, dTagsa, lTagsa, "Joey", "Robby");
            var li3 = new LostItem(dateTime3, dTagsa, lTagsa, "Joey", "Robby");
            var di3 = new DisposedItem(dateTime3, dateTime3, dTagsa, lTagsa, "Joey", "na", "na", "Robby", "Trash");
            */
            //FoundItems.Add(foundItemProvider.CreateFoundItem(dateTime, dTagString, lTagString, "Joe", "Rob"));
            //foundItemProvider.UpdateFoundItem(fi, fi2);
            //No Delete, must use update with unusual value????
            //foundItemProvider.UpdateFoundItem(fi2, fi3);
            //LostItems.Add(lostItemProvider.CreateLostItem(dateTime, dTagString, lTagString, "Joe", "na", "na", "Rob"));
            //lostItemProvider.UpdateLostItem(li, li2);
            //No Delete, must use update with unusual value????
            //lostItemProvider.UpdateLostItem(li2, li3);
            //DisposedItems.Add(disposedItemProvider.CreateDisposedItem(dateTime, dateTime, dTagString, lTagString, "Joe", "na", "na", "Rob", "Incinerator"));
            //disposedItemProvider.UpdateDisposedItem(di, di2);
            //No Delete, must use update with unusual value????
            //disposedItemProvider.UpdateDisposedItem(di2, di3);
        }
    }
}

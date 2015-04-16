using System;
using System.Security.Policy;

namespace LostAndFound.Models
{
    public class ClaimedItem : Item
    {
        public ClaimedItem(DateTime date, string tnumber, string name)
        {
            this.DateReported = date;
            this.TNumber = tnumber;
            this.Name = name;
        }

        public string TNumber { get; set; }
    }
}

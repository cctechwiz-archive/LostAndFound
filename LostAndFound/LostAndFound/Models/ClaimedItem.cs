using System;
using System.Security.Policy;

namespace LostAndFound.Models
{
    public class ClaimedItem : Item
    {
        public ClaimedItem(DateTime date)
        {
            this.Date = date;
        }

        private DateTime Date { get; set; }

        private User User { get; set; }
    }
}

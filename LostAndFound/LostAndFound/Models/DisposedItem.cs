using System;

namespace LostAndFound.Models
{
    public class DisposedItem : Item
    {
        private DateTime Date { get; set; }

        private User User { get; set; }
    }
}

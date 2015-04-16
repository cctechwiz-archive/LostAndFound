using System;
using System.Collections.Generic;

namespace LostAndFound.Models
{
    public class DisposedItem : Item
    {
        private DateTime Date { get; set; }

        private List<DisposedTag> Tags { get; set; }
    }
}

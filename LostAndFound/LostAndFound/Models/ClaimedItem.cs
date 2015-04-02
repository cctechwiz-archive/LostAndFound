using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Models
{
    public class ClaimedItem : Item
    {
        private DateTime Date { get; set; }

        private User User { get; set; }
    }
}

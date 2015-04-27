using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Models
{
    public class MyItem
    {
        public string desc { get; set; }
        public string loc { get; set; }
        public string date { get; set; }
        public bool isVisible { get; set; }
        public bool isSelected { get; set; }
    }
}

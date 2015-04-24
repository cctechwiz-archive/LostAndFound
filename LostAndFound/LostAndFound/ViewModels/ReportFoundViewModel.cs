using System;
using System.Collections.Generic;
using LostAndFound.Models;
using LostAndFound.Services;

namespace LostAndFound.ViewModels
{

    class ReportFoundViewModel
    {
        
        public ReportFoundViewModel()
        {
        }

        public List<Item> Items { get; set; }

        public String Text { get; set; }

    }
}

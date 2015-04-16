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

        public List<User> Users { get; set; }

        public String Text { get; set; }

    }
}

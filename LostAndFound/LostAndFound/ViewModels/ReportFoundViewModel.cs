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
            var excelProvider = new ExcelProvider();
            Users = excelProvider.GetUsers();
            Users.Add(excelProvider.CreateNewUser("JERSH", "MERXWERLS","123-111-9928"));
        }

        public List<User> Users { get; set; }

        public String Text { get; set; }

    }
}

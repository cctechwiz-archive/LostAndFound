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

            User oldJesse = new User("Jesse", "Maxwell", "435-590-5555");
            User newJesse = new User("JESSAy", "maerxhwelrl", "123-456-8678");
            excelProvider.UpdateUser(oldJesse, newJesse);
        }

        public List<User> Users { get; set; }

        public String Text { get; set; }

    }
}

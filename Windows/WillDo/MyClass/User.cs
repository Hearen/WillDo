using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WillDo.MyClass
{
    public class User
    {
        /// <summary>
        /// you cannot serealize a private member!!!!!
        /// make it avalvable outside its class;
        /// </summary>
        Globals Global = (App.Current as App).Global;
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string password { get; set; }
        
        public List<ToDoItem> allItemsList { get; set; }
        public ObservableCollection<string> titleList { get; set; }
        public ObservableCollection<string> categoryList { get; set; }
        public ObservableCollection<string> locationList { get; set; }

        public User()
        {
            user_id = 1;
            user_name = "Hearen";
            password = "Noodle66666";
            allItemsList = Global.allItemList;
            titleList = Global.titles;
            categoryList = Global.categories;
            locationList = Global.locations;
        }
    }
}

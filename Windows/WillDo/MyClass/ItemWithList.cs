using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WillDo.MyClass
{
    public class ItemWithList
    {
        private string name;
        private ObservableCollection<object> list = new ObservableCollection<object>();

        public string Name 
        {
            get { return name; }
            set { name = value; }
        }

        public ObservableCollection<object> List
        {
            get { return list; }
            set { list = value; }
        }

    }

    public class Priority
    {
        public string color { get; set; }
        public string name { get; set; }
        public Priority(string color, string name)
        {
            this.color = color;
            this.name = name;
        }
    }
    
}

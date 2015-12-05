using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WillDo.MyClass
{
    public class Globals
    {
        static DateTime MIDNIGHT = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);//the midnight of today;
        static DateTime NEXT_MIDNIGHT = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1, 23, 59, 59);//the midnight of tomorrow;

        public bool IsInTrash = false;

        public int user_id = 52;

        public string encrypted_pwd = "asfequieqij;kljt";

        public string user_name = "nick";

        /// <summary>
        /// this value is current available; after being used, increment it please;
        /// this value should checked and reset each time synching with the server;
        /// this value will be stored in xml file to enable the offline adding operations;
        /// its initialization process happens when firstly loading data from xml file;
        /// </summary>
        public int current_max_id;

        //global frequently used variables; all these list can be replaced by itemWithListDic;
        //public List<string> titleList = new List<string>();
        //public List<string> categoryList = new List<string>();
        //public List<string> locationList = new List<string>();
        //end of the stored List;

        /// <summary>
        /// all UI elements' corresponding data initializing header and foot;
        /// </summary>
        public Dictionary<string, ItemWithList> itemWithListDic = new Dictionary<string, ItemWithList>();
        
        

        /// <summary>
        /// all the to-do items will be store in the dictionary -- convenient to access and store;
        /// </summary>
        public Dictionary<int, ToDoItem> itemDic = new Dictionary<int, ToDoItem>();
        
        //the current item
        public ToDoItem currentItem = new ToDoItem();//after each selection changing in listview, reset the currentItem;

        //the following variables are the filtering ones used to filter the list excluding the trash list;
        //filtering factors ordered in decreasing order;
        public string showFilter = "All Tasks"; //all, incomplete, complete, recently modified;

        public string titleFilter { get; set; } //stored locally;

        public string dueFilter = "<any date>"; //set by due by combobox or specific time;

        public DateTime specificFilter = MIDNIGHT; //this attribute is used to assist the dueTime to filter;

        public int priorityFilter = 0; // 1 - 10 inclusive respectively representing different prioirty;

        /// <summary>
        /// these two lists are used to filter; not for edition;
        /// </summary>
        /// 
        public List<string> fCategories = new List<string>();
        public List<string> fLocations = new List<string>();
        

        /// <summary>
        /// the following collections are used to edit and filter;
        /// </summary>
        public ObservableCollection<string> categories = new ObservableCollection<string>(); //stored locally;

        public ObservableCollection<string> locations = new ObservableCollection<string>(); //the selected location - from the stored ones;

        public ObservableCollection<string> titles = new ObservableCollection<string>();
        //filtering factors ordered in decreasing order;

        public List<ToDoItem> allItemList = new List<ToDoItem>();//contain all the to-do items including complete and incomplete;

        public ObservableCollection<ToDoItem> shownItemList = new ObservableCollection<ToDoItem>();//contain only the items filtered;

        public ObservableCollection<ToDoItem> deletedItemList = new ObservableCollection<ToDoItem>();//contain all the deleted items;

        public Globals()
        {
            initUIElement();
        }

        public void refreshAllItems(User user)
        {
            allItemList = user.allItemsList;
            titles = user.titleList;
            categories = user.categoryList;
            locations = user.locationList;
            shownItemList.Clear();
            deletedItemList.Clear();
            for(int i = 0; i < allItemList.Count; i++)
            {
                if (allItemList[i].IsDeleted)
                    deletedItemList.Add(allItemList[i]);
                else
                    shownItemList.Add(allItemList[i]);
            }
            initUIElement();
        }

        /// <summary>
        /// given a parent name and the current UIElement, return the certain type and certain name parent;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetParentObject<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);

            while (parent != null)
            {
                Console.WriteLine(parent.ToString());
                if (parent is T && (((T)parent).Name == name | string.IsNullOrEmpty(name)))
                {
                    return (T)parent;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }

        public T GetChildObject<T>(DependencyObject obj, string name) where T: FrameworkElement
        {
            for(int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T && ((T)child).Name == name)
                    return child as T;
                else
                {
                    T childOfChild = GetChildObject<T>(child, name);
                    if (childOfChild != null)
                        return childOfChild as T;
                }
            }
            return null;
        }

        /// <summary>
        /// according to the given unique id to get the Item from the exsiting list
        /// shownItemList, deletedItemList or allItemList;
        /// </summary>
        /// <param name="id">the unique id of the to-do item</param>
        /// <param name="type">default 0 - shownItemList, -1 - deletedItemList</param>
        /// <returns></returns>
        public ToDoItem getItemFromInnerList(int id, int type = 0)
        {
            ToDoItem item = null;
            ObservableCollection<ToDoItem> list = new ObservableCollection<ToDoItem>();
            switch(type)
            {
                case -1: list = deletedItemList; break;
                case 0: list = shownItemList; break;
                default: break;
            }

            for (int i = 0; i < list.Count; i++)
            {
                if (id == list[i].ID)
                    item = list[i];
            }
            return item;
        }

        /// <summary>
        /// at the very beginning the shownItemList and allItemList will be initialized with same value;
        /// </summary>
        public void loadData()
        {

            //load categories;
            //itemWithListDic["Category"].List.Add("Work");
            //itemWithListDic["Category"].List.Add("Study");
            //itemWithListDic["Category"].List.Add("Daily");

            ///these are used to initialize with zombie data; it's time to use the real ones;
            /*
            //
            //listViewItem
            for (int i = 0; i < 10; i++)
            {
                ToDoItem item0 = new ToDoItem();
                item0.Due = DateTime.Now.AddHours(6 * i);
                item0.Created = DateTime.Now.AddHours(10 * i);
                item0.Comment += 800000 * i;
                item0.Priority += i;
                item0.Title += i;
                item0.ID = i;
                item0.Priority = item0.Priority > 10 ? 10 : item0.Priority;
                allItemList.Add(item0);
                shownItemList.Add(item0);
            }
            */
            ///load the date from xml file;
            XmlReaderWriter xmlReader = new XmlReaderWriter();
            xmlReader.readFromXml();
            
        }

        //before this operation, all related values should be set according to the given requirements above;
        //after this operation, the shownItemList will only contain the items meed the filtering condition;
        public void filter()
        {
            List<ToDoItem> tmpList = new List<ToDoItem>();
            filterByShown(tmpList);
            
            filterByTitle(tmpList);
            filterByDue(tmpList);
            filterByPriority(tmpList);
            filterByCategory(tmpList);
            filterByLocation(tmpList);
            
            if(IsInTrash)
            {
                deletedItemList.Clear();
                foreach(ToDoItem item in tmpList)
                {
                    deletedItemList.Add(item);
                }
            }
            else
            {
                shownItemList.Clear();
                foreach(ToDoItem item in tmpList)
                {
                    shownItemList.Add(item);
                }
            }
        }
        
        //the first filter - by show type
        /// <summary>
        /// the first place to filter allItemList to get the first version of shownItemList;
        /// where we should also consider the IsDeleted attribute of the item and then so be it;
        /// </summary>
        private void filterByShown(List<ToDoItem> list)
        {
            ///handle the sepcial occasions;
            foreach (ToDoItem item in allItemList)
            {
                if (showFilter == "All Tasks")
                {
                    if (!IsInTrash && !item.IsDeleted)///to-do list, used to filter the deleted ones;
                        list.Add(item);
                    else if(IsInTrash && item.IsDeleted)
                        list.Add(item);
                }
                //imcomplete itesm including progress less than 100 and isComplete status is true;
                if (showFilter == "Incomplete Tasks" && item.Progress < 100)
                {
                    if (!IsInTrash && !item.IsDeleted)///to-do list, used to filter the deleted ones;
                        list.Add(item);
                    else if (IsInTrash && item.IsDeleted)
                        list.Add(item);
                }
                else if(showFilter == "Complete Tasks" && item.Progress == 100)
                {
                    if (!IsInTrash && !item.IsDeleted)///to-do list, used to filter the deleted ones;
                        list.Add(item);
                    else if (IsInTrash && item.IsDeleted)
                        list.Add(item);
                }
                    //just shown the item modified within a day;
                else if(showFilter == "Recently Modified" && (DateTime.Now.Subtract(item.LastModified) < TimeSpan.FromDays(1.0)))
                {
                    if (!IsInTrash && !item.IsDeleted)///to-do list, used to filter the deleted ones;
                        list.Add(item);
                    else if (IsInTrash && item.IsDeleted)
                        list.Add(item);
                }
            }
         }

        //the second filter using a temporary list to store the filtered items first 
        //and then replace the items in shownItemsList
        private void filterByTitle(List<ToDoItem> list)
        {
            if (titleFilter == null)//just return without filtering operation;
                return;
            for (int i = 0; i < list.Count; i++)
            {
                if (titleFilter != null && !(list[i].Title.Contains(titleFilter)))
                {
                    list.RemoveAt(i);
                    i--;//remove an item from collection, to subscript the next one correctly, substract by one is neccessary;
                }
                    
            }
        }

        //using a temporary list to store the filtered results and then replace the items in shownItenList;
        private void filterByDue(List<ToDoItem> list)
        {
            if (dueFilter == "<any date>")//just return without further process;
                return;
            
            //special occasion;
            //filtering according to a certain date;
            for (int i = 0; i < list.Count; i++)
            {
                if (dueFilter == "<specific date>")
                {
                    //set the time to the midnight of that day;
                    specificFilter = new DateTime(specificFilter.Year, specificFilter.Month, specificFilter.Day,
                                        23, 59, 59);
                    if (specificFilter.CompareTo(list[i].Due) < 0)///within the specific time
                        list.RemoveAt(i--);
                        //remove an item from collection, to subscript the next one correctly, substract by one is neccessary;
                }
                else if (dueFilter == "Now" &&
                            (
                            DateTime.Now.AddHours(1.0).CompareTo(list[i].Due) < 0 ||
                            DateTime.Now.AddHours(-1.0).CompareTo(list[i].Due) > 0
                            )
                         )//via hour instead of minutes or seconds;
                {
                    list.RemoveAt(i);
                    i--;//remove an item from collection, to subscript the next one correctly, substract by one is neccessary;
                }
                else if (dueFilter == "Today" && list[i].Due.CompareTo(MIDNIGHT) > 0)//within a day;
                {
                    list.RemoveAt(i--);
                    //remove an item from collection, to subscript the next one correctly, substract by one is neccessary;
                }
                else if (dueFilter == "Tomorrow" &&
                    list[i].Due.CompareTo(NEXT_MIDNIGHT) > 0)//before tomorrow;
                {
                    list.RemoveAt(i--);
                    //remove an item from collection, to subscript the next one correctly, substract by one is neccessary;
                }
                else if (dueFilter == "The End of This Week" && 
                    (
                    list[i].Due.DayOfWeek < DateTime.Now.DayOfWeek) || list[i].Due > DateTime.Now.AddDays(7)
                    )//within a week - started from sunday;
                {
                    list.RemoveAt(i--);
                    //remove an item from collection, to subscript the next one correctly, substract by one is neccessary;
                }
            }//end of foreach for shownItemList;
        }

        //only the larger or equal ones can be selected;
        private void filterByPriority(List<ToDoItem> list)
        {
            if (priorityFilter == 0)//just return without any filtering;
                return;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Priority < priorityFilter)
                {
                    list.RemoveAt(i--);
                    ////remove an item from collection, to subscript the next one correctly, substract by one is neccessary;
                }
            }
        }

        //selecting certain categories after all those filtering processes;
        private void filterByCategory(List<ToDoItem> list)
        {
            if (fCategories.Count == 0)//without any filtering condition; just return;
                return;
            for (int i = 0; i < list.Count; i++)
            {
                if (!fCategories.Contains(list[i].Category))
                {
                    list.RemoveAt(i--);
                    //remove an item from collection, to subscript the next one correctly, substract by one is neccessary;
                }
            }
        }

        //filtered by locations;
        private void filterByLocation(List<ToDoItem> list)
        {
            if (fLocations.Count == 0)//without any location filtering condition, just return;
                return;

            for (int i = 0; i < list.Count; i++)
            {
                if (!(fLocations.Contains(list[i].Location)))//do't contain, remove it;
                {
                    list.RemoveAt(i--);
                    //remove an item from collection, to subscript the next one correctly, substract by one is neccessary;
                }
            }
        }

        
        /// <summary>
        /// //this method will be invoked only once - when the window first initialized;
        /// all the data here is stable; unless another version is out;
        /// </summary>
        private void initUIElement()
        {
            //adding showItem
            ItemWithList showItem = new ItemWithList();
            showItem.Name = "Show";
            showItem.List.Add("All Tasks");
            showItem.List.Add("Incomplete Tasks");
            showItem.List.Add("Complete Tasks");
            showItem.List.Add("Recently Modified");
            if(!itemWithListDic.ContainsKey("Show"))
                itemWithListDic.Add("Show", showItem);

            //adding titleItem -- this item may be modified at run-time
            ItemWithList titleItem = new ItemWithList();
            titleItem.Name = "Title";
            if (!itemWithListDic.ContainsKey("Title"))
                itemWithListDic.Add(titleItem.Name, titleItem);

            //adding dueItem for header
            ItemWithList dueItem = new ItemWithList();
            dueItem.Name = "Due by";
            dueItem.List.Add("<any date>");
            dueItem.List.Add("<specific date>");
            dueItem.List.Add("Now");
            dueItem.List.Add("Today");
            dueItem.List.Add("Tomorrow");
            dueItem.List.Add("The End of This Week");
            if (!itemWithListDic.ContainsKey("Due by"))
                itemWithListDic.Add(dueItem.Name, dueItem);

            //adding priorityItem for header
            ItemWithList abovePriorityItem = new ItemWithList();
            abovePriorityItem.Name = "Above Priority";


            abovePriorityItem.List.Add(new Priority("", "<any>"));
            abovePriorityItem.List.Add(new Priority("LightBlue", "1 Lowest"));
            abovePriorityItem.List.Add(new Priority("LightSkyBlue", "2 Low"));
            abovePriorityItem.List.Add(new Priority("DeepSkyBlue", "3 Low-Med"));
            abovePriorityItem.List.Add(new Priority("MediumBlue", "4 Medium"));
            abovePriorityItem.List.Add(new Priority("Navy", "5 Medium"));
            abovePriorityItem.List.Add(new Priority("MidnightBlue", "6 Medium"));
            abovePriorityItem.List.Add(new Priority("HotPink", "7 Med-High"));
            abovePriorityItem.List.Add(new Priority("DeepPink", "8 High"));
            abovePriorityItem.List.Add(new Priority("Red", "9 Very High"));
            abovePriorityItem.List.Add(new Priority("DarkRed", "10 Highest"));

            if (!itemWithListDic.ContainsKey("Above Priority"))
                itemWithListDic.Add(abovePriorityItem.Name, abovePriorityItem);

            ///these two attributes are not stable; should be removed;
            /*
            //adding categoryItem for header
            ItemWithList categoryItem = new ItemWithList();
            categoryItem.Name = "Category";
            categoryItem.List.Add("Work");
            categoryItem.List.Add("Study");
            categoryItem.List.Add("Daily");
            itemWithListDic.Add(categoryItem.Name, categoryItem);

            ItemWithList locationItem = new ItemWithList();
            locationItem.Name = "Location";
            locationItem.List.Add("home");
            locationItem.List.Add("classroom");
            locationItem.List.Add("dormitory");
            locationItem.List.Add("school");
            itemWithListDic.Add(locationItem.Name, locationItem);
            */
           

            //adding priority for foot
            ItemWithList priorityItem = new ItemWithList();
            priorityItem.Name = "Priority";
            foreach (Priority priority in abovePriorityItem.List)
            {
                priorityItem.List.Add(priority.name);
            }
            if (!itemWithListDic.ContainsKey("Priority"))
                itemWithListDic.Add(priorityItem.Name, priorityItem);

            //adding %Complete for foot
            ItemWithList completeItem = new ItemWithList();
            completeItem.Name = "%Complete";
            completeItem.List.Add("0");
            completeItem.List.Add("25");
            completeItem.List.Add("50");
            completeItem.List.Add("75");
            completeItem.List.Add("100");
            if (!itemWithListDic.ContainsKey("%Complete"))
                itemWithListDic.Add(completeItem.Name, completeItem);
        }

    }
}

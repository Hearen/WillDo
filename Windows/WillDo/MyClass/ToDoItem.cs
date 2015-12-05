using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WillDo.MyClass
{
    public class ToDoItem:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        int id;//unique id;
        public int ID
        {
            get { return id; }
            set
            {
                id = value;
                LastModified = DateTime.Now;
                if(this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("ID"));
                }
            }
        }



        string title;//subject or title for the to-do item;

        public string Title 
        {
            get { return title; } 
            set
            {
                title = value;
                LastModified = DateTime.Now;
                if(this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Title"));
                }
            }
        }

        

        DateTime created;

        public DateTime Created 
        {
            get { return created; }
            set
            {
                created = value;
                LastModified = DateTime.Now;
                if(this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Created"));
                }
            }
        }//the time when the item created;

       

        DateTime due;//the deadline of the item;
        public DateTime Due 
        {
            get { return due; }
            set
            {
                due = value;
                LastModified = DateTime.Now;
                if(this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Due"));
                }
            }
        }

        string color;//used to decorate the foreground of the listviewitem;
        public string Color
        {
            get { return color; }
            set
            {
                color = value;
                LastModified = DateTime.Now;
                if(this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Color"));
                }
            }
        }

        DateTime lastModified;//the time last modified the to-do item, which can be used to synch with server;
        public DateTime LastModified 
        {
            get { return lastModified; }
            set
            {
                lastModified = value;
                if(this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("LastModified"));
                }
            }
        }

        string category;
        public string Category
        {
            get { return category; }
            set
            {
                category = value;
                LastModified = DateTime.Now;
                if(this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Category"));
                }
            }
        }

        string location;
        public string Location
        {
            get { return location; }
            set
            {
                location = value;
                LastModified = DateTime.Now;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Location"));
                }
            }
        }

        //this part is used to add several locations and categories to to-do item;
        //still some problems need to be solved;
        /*
        List<string> category = new List<string>();//category of this to-do item;
        public string Category 
        {
            get 
            {
                return String.Join(",", category.ToArray());
            }
            set
            {
                if(!category.Contains(value))//in case adding more than once;
                {
                    List<string> sList = new List<string>(category);
                    sList.Add(value);
                    category = sList;
                }
                if(this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Category"));
                }
            }
        }
        public void removeCString(string s)
        {
            if (category.Contains(s))
                category.Remove(s);
        }
        
        List<string> location = new List<string>();//where the to-do task set;
        public string Location 
        {
            get 
            { 
                return String.Join(",", location.ToArray()); 
            }
            set
            {
                if(!location.Contains(value))//in case to add more than once;
                    location.Add(value);
                if(this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Location"));
                }
            }
        }

        */

        int priority;//priority from 1 - 10 inclusive;
        public int Priority 
        {
            get { return priority; }
            set
            {
                priority = value;//once priority changed, change the corresponding priorityColor;
                LastModified = DateTime.Now;
                switch (priority)
                {
                    //using PriorityColor instead of priorityColor;
                    //make sure the propertychanged event will be invoked inside PriorityColor
                    case 1: PriorityColor = "LightBlue"; break;
                    case 2: PriorityColor = "LightSkyBlue"; break;
                    case 3: PriorityColor = "DeepSkyBlue"; break;
                    case 4: PriorityColor = "MediumBlue"; break;
                    case 5: PriorityColor = "Navy"; break;
                    case 6: PriorityColor = "MidnightBlue"; break;
                    case 7: PriorityColor = "HotPink"; break;
                    case 8: PriorityColor = "DeepPink"; break;
                    case 9: PriorityColor = "Red"; break;
                    case 10: PriorityColor = "DarkRed"; break;
                    default: PriorityColor = "Navy"; break;
                }
                if(this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Priority"));
                    //this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("PriorityColor"));//may be something wrong about this usage;
                }
            }
        }

        /// <summary>
        /// used to decorate the priority;
        /// </summary>
        string priorityColor;
        public string PriorityColor
        {
            get 
            { 
                return priorityColor;
            }

            set
            {
                priorityColor = value;
                LastModified = DateTime.Now;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("PriorityColor"));
                }
            }
        }

        int progress;

        public int Progress
        {
            get { return progress; }
            set
            {
                progress = value;
                LastModified = DateTime.Now;
                if(progress == 100)
                {
                    isComplete = true;
                }
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Progress"));
                }
            }
        }//%Complete which should have only several predefined values;

       
        bool isComplete;//the status of the item;
        public bool IsComplete 
        {
            get { return isComplete; }
            set
            {
                isComplete = value;
                LastModified = DateTime.Now;
                if(isComplete)
                {
                    Progress = 100;
                    Color = "Gray";///as long as it's complete, gray it;
                }
                else
                {
                    if (Progress == 100)
                        Progress = 75;
                    if(!IsDeleted && Color == "Gray")///only the current item is not deleted and it's gray; set it back to default;
                        Color = "Black";
                }
                if(this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("IsComplete"));
                }
            }
        }

        bool isDeleted;///the status of the item, in trash or shownList;
        public bool IsDeleted
        {
            get { return isDeleted; }
            set
            {
                isDeleted = value;
                LastModified = DateTime.Now;
                if(isDeleted)///as long as it's deleted, gray it;
                {
                    Color = "Gray";///using Color instead of color, to invoke the propertyChanged event;
                }
                else
                {
                    if(!IsComplete && Color == "Gray")///in case black a complete task; and only the current color gray not personal designed;
                        Color = "Black";
                }
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("IsDeleted"));
                }
            }
        }

        string comment;//the details of the to-do item;
        public string Comment 
        {
            get { return comment; }
            set
            {
                comment = value;
                LastModified = DateTime.Now;
                if(this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Comment"));
                }
            }
        } 

        public ToDoItem()
        {
            ID = 0;
            Title = "to-do";
            Progress = 0;
            Created = DateTime.Now;
            Due = DateTime.Now;
            Color = "Black";
            Category = "Work";
            Priority = 5;
            Location = "";
            IsComplete = false;
            IsDeleted = false;
            Comment = "I really have to finish this project in time";
        }

        /// <summary>
        /// used to return a duplication of the current item; in case of address reference;
        /// </summary>
        /// <returns></returns>
        public ToDoItem getCopy()
        {
            ToDoItem item = new ToDoItem();
            item.ID = this.ID;
            item.title = this.title;
            item.progress = this.progress;
            item.priority = this.priority;
            item.location = this.location;
            item.category = this.category;
            item.isComplete = this.isComplete;
            item.color = this.color;
            item.comment = this.comment;
            item.created = this.created;
            item.due = this.due;
            item.lastModified = this.lastModified;
            
            return item;
        }
    }
}

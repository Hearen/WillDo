using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WillDo.MyClass;
using WillDo.Tools;

namespace WillDo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Globals Global = (App.Current as App).Global;

        public MainWindow()
        {
            InitializeComponent();
            Global.loadData();
            initHeader();
            initBody();
            activateFootPanel(false);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            XmlReaderWriter xmlReader = new XmlReaderWriter();
            xmlReader.writeDownToXml();
            base.OnClosing(e);
        }

        /// <summary>
        /// set the header Panel according to Globals.itemWithListDic;
        /// </summary>
        private void initHeader()
        {
            //Global.initUIElement();
            
            //binding for header
            ItemWithList item = Global.itemWithListDic["Show"] as ItemWithList;
            this.showItem.titleTextBlock.Text = item.Name;
            //this.showItem.itemsComboBox.ItemsSource = typeof(Colors).GetProperties();
            this.showItem.itemsComboBox.ItemsSource = item.List;
            this.showItem.itemsComboBox.IsEditable = false;
            this.showItem.itemsComboBox.SelectedIndex = 0;


            item = Global.itemWithListDic["Due by"] as ItemWithList;
            this.dueItem.titleTextBlock.Text = item.Name;
            this.dueItem.itemsComboBox.ItemsSource = item.List;
            this.dueItem.itemsComboBox.IsEditable = false;
            this.dueItem.itemsComboBox.SelectedIndex = 0;

            item = Global.itemWithListDic["Above Priority"] as ItemWithList;
            this.abovePriorityItem.titleTextBlock.Text = item.Name;
            List<Priority> abovePriorityList = new List<Priority>();
            foreach(object o in item.List)
            {
                abovePriorityList.Add(o as Priority);
            }
            this.abovePriorityItem.itemsComboBox.ItemsSource = abovePriorityList;
            this.abovePriorityItem.itemsComboBox.IsEditable = false;
            this.abovePriorityItem.itemsComboBox.SelectedIndex = 0;

            ///title;
            this.titleItem.titleTextBlock.Text = "Title";
            this.titleItem.itemsComboBox.ItemsSource = Global.titles;

            ///category;
            this.categoryItem.titleTextBlock.Text = "Category";
            this.categoryItem.itemsComboBox.ItemsSource = Global.categories;
            this.categoryItem.itemsComboBox.IsEditable = false;

            ///locations;
            this.hLocationItem.titleTextBlock.Text = "Location";
            this.hLocationItem.itemsComboBox.ItemsSource = Global.locations;
            this.hLocationItem.itemsComboBox.IsEditable = false;

            //adding for foot
            item = Global.itemWithListDic["Priority"] as ItemWithList;
            List<Priority> priorityList = new List<Priority>();
            priorityList.AddRange(abovePriorityList);
            this.priorityItem.titleTextBlock.Text = item.Name;
            priorityList.RemoveAt(0);//delete the add selection;
            this.priorityItem.itemsComboBox.ItemsSource = priorityList;
            this.priorityItem.itemsComboBox.SelectedIndex = 4;

            item = Global.itemWithListDic["%Complete"] as ItemWithList;
            this.progressItem.titleTextBlock.Text = item.Name;
            this.progressItem.itemsComboBox.ItemsSource = item.List;
            this.progressItem.itemsComboBox.IsEditable = false;
            this.progressItem.itemsComboBox.SelectedIndex = 0;

            ///for foot categories;
            this.fCategoryItem.titleTextBlock.Text = "Category";
            this.fCategoryItem.itemsComboBox.ItemsSource = Global.categories;
            this.fCategoryItem.itemsComboBox.SelectedIndex = 0;

            ///for foot locations;
            this.fLocationItem.titleTextBlock.Text = "Location";
            this.fLocationItem.itemsComboBox.ItemsSource = Global.locations;

            this.colorItem.titleTextBlock.Text = "Color";
            this.colorItem.itemsComboBox.ItemsSource = typeof(Colors).GetProperties();
            
            
            ///this.colorItem.itemsComboBox.SelectedIndex = 0;
            ///have to disable this operation; for side effect; 
            ///in the ColorSelectionControl will affect the first foreground of the listview;

            //this.fStartedItem.titleTextBlock.Text = "Started from";

            this.fDueItem.titleTextBlock.Text = "Due Date";
        }

        /// <summary>
        /// bind the allItemList to listview;
        /// </summary>
        private void initBody()
        {
            this.listViewItem.ItemsSource = Global.shownItemList;
        }

        /// <summary>
        /// both the listView and the TextBox inside listView will cause selectionChanged
        /// but only the directly selection happened on the listview will invoke this event;
        /// so we need to handle the textBox gotfocus event at the same time;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Console.WriteLine(listViewItem.SelectedIndex);//used to test the index;
            ToDoItem item = listViewItem.SelectedItem as ToDoItem;
            if(item == null)
            {
                return;
            }
            //set the current item; the selected item in listview;
            Global.currentItem = item;
            ///why must we have to put this assignment before the foot panel resetting?
            ///setFootPanel will invoke all the related controls to reset the value of the currentItem;
            ///and right now the currentItem is still pointting to the previous item!!!!
            if (!Global.IsInTrash)//only when in to-do list the foot panel can be used;
            {
                setFootPanel();
            }
        }

        /// <summary>
        /// this handler is quite the same with the selectionChanged event handler;
        /// used to handle the selection changed to assign the currently selected item to the Global.currentItem;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textB_GotFocus(object sender, RoutedEventArgs e)
        {
            FrameworkElement text;
            text = sender as TextBlock;
            if(text == null)
                text = sender as TextBox;
            ContentPresenter contentPresenter = text.TemplatedParent as ContentPresenter;
            ToDoItem item = contentPresenter.Content as ToDoItem;
            this.listViewItem.SelectedItem = item;//set the selectedItem of listView to the current focused;
            //set the current item; the selected item in listview;
            Global.currentItem = item;
            ///why must we have to put this assignment before the foot panel resetting?
            ///setFootPanel will invoke all the related controls to reset the value of the currentItem;
            ///and right now the currentItem is still pointting to the previous item!!!!
            
            setFootPanel();
        }
        
        
        /// <summary>
        /// //using the selected to-do item to set all the related UIElements in Foot Panel;
        /// besides, set the comment of the side at the same time;
        /// </summary>
        /// <param name="item"></param>
        private void setFootPanel()
        {
            ToDoItem item = Global.currentItem;
            activateFootPanel(true);
            if (item == null)//in case the item is null and access it;
                return;
            this.priorityItem.itemsComboBox.SelectedIndex = item.Priority - 1 ;
            this.progressItem.itemsComboBox.SelectedIndex = item.Progress / 25;
            this.fDueItem.dateSelector.SelectedDate = item.Due;
           //this.fStartedItem.dateSelector.SelectedDate = item.Started;
            this.fCategoryItem.itemsComboBox.SelectedItem = item.Category;
            this.fLocationItem.itemsComboBox.SelectedItem = item.Location;
            
            ///test whether we can get the button object from its itemsComboBox or not;
            ///the result is we cannot access its child of the items;
            //Console.WriteLine("*******************************");
            //Console.WriteLine(Global.GetChildObject<Button>(this.fCategoryItem.itemsComboBox, "categoryB"));
            
            //this.colorItem.itemsComboBox.SelectedIndex = 2;
            ///have to disable this operation; for side effect; 
            ///in the ColorSelectionControl will affect the first foreground of the listview;

            //set the comment of the side; -- just a side function;
            this.commentTextBox.Text = item.Comment;
        }

        /// <summary>
        /// activate or deactivate all the elements in foot panel;
        /// besides, deactivate the elements clear the content of the comment in the Side Panel;
        /// </summary>
        private void activateFootPanel(bool isTrue)
        {
            if(!isTrue)//if deactivate the foot panel, set all the values back to default;
            {
                this.priorityItem.itemsComboBox.SelectedIndex = 4;
                this.progressItem.itemsComboBox.SelectedIndex = 0;
                this.fDueItem.dateSelector.SelectedDate = DateTime.Now;

                //this.fStartedItem.dateSelector.SelectedDate = DateTime.Now;

                ///this.colorItem.itemsComboBox.SelectedIndex = 0;
                ///have to disable this operation; for side effect; 
                ///in the ColorSelectionControl will affect the first foreground of the listview;

                this.fCategoryItem.itemsComboBox.SelectedIndex = 0;
                this.fLocationItem.itemsComboBox.SelectedIndex = 0;

                //side function of this method; clear the comment;
                this.commentTextBox.Text = "";
            }
            this.priorityItem.itemsComboBox.IsEnabled = isTrue;
            this.progressItem.itemsComboBox.IsEnabled = isTrue;
            this.fDueItem.dateSelector.IsEnabled = isTrue;
            //this.fStartedItem.dateSelector.IsEnabled = isTrue;
            this.colorItem.itemsComboBox.IsEnabled = isTrue;
            this.fCategoryItem.itemsComboBox.IsEnabled = isTrue;
            this.fLocationItem.itemsComboBox.IsEnabled = isTrue;
        }

        private void listViewItem_LostFocus(object sender, RoutedEventArgs e)
        {
            //activateFootPanel(false);//need to reedit the selected item, cannot set it disabled;
            //Console.WriteLine(listViewItem.SelectedItem.ToString());
        }

        /// <summary>
        /// add new default to-do item to the shownItemList and allItemList
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            ToDoItem item = new ToDoItem();
            item.ID = Global.current_max_id++;
            Global.shownItemList.Add(item);//ToDo this code will be changed due to the ID unique problem;
            this.listViewItem.SelectedItem = item;
            Global.allItemList.Add(item);
        }

        /// <summary>
        /// delete the currently selected item from the shownItemList and the listview;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (Global.currentItem != null)
            {
                /*//cannot just change the currentItem;
                int index = Global.shownItemList.IndexOf(Global.currentItem);
                if (index + 1 < Global.shownItemList.Count)
                    Global.currentItem = Global.shownItemList[index + 1];
                else if(index - 1 > -1)
                    Global.currentItem = Global.shownItemList[index - 1];
                */
                //foreach(ToDoItem item in Global.shownItemList)//cannot modify a collection inside foreach;
                
                ///currentIndex++;//move to the next place;after deleting the items' amount is decreased by one; 
                ///needless to increment by one;
                Global.currentItem.IsDeleted = true;
                
                if(Global.IsInTrash)//delete permanently;
                {
                    if(MessageBox.Show("Are you sure to delete this task permanently?","Warning!", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        int currentIndex = Global.deletedItemList.IndexOf(Global.currentItem);
                        if(!Global.deletedItemList.Contains(Global.currentItem))///make sure the current list contains the currentItem;
                        {
                            MessageBox.Show("Please select a to-do task first!");
                            return;
                        }
                        Global.deletedItemList.Remove(Global.currentItem);
                        Global.allItemList.Remove(Global.currentItem);
                        if (Global.deletedItemList.Count == 0)///after deleting the deletedItemList is empty now, just return;
                            return;
                        int nextIndex = (currentIndex < Global.deletedItemList.Count) ? currentIndex : 0;
                        this.listViewItem.SelectedIndex = nextIndex;///select the next item to make it more friendly;
                        ///needless to reset the currentItem, no edition will be processed - disabled edit panel;
                    }
                }
                else
                {
                    if(!Global.shownItemList.Contains(Global.currentItem))//make sure the current list contains the currentItem;
                    {
                        MessageBox.Show("Please select a to-do task first!");
                        return;
                    }
                    int currentIndex = Global.shownItemList.IndexOf(Global.currentItem);
                    Global.deletedItemList.Add(Global.currentItem);
                    Global.shownItemList.Remove(Global.currentItem);
                    if (Global.shownItemList.Count == 0)///if after deleting operation, there is no item left; just return;
                        return;
                    int nextIndex = (currentIndex < Global.shownItemList.Count) ? currentIndex : 0;//if out of range, set back to 0;
                    
                    this.listViewItem.SelectedIndex = nextIndex;
                    Global.currentItem = Global.shownItemList[nextIndex];///reset the currentItem, in case of edition;
                }
                
                
                ///trying to delete it directly instead of using the id;
                /*
                for (int i = 0; i < Global.shownItemList.Count; i++ )
                {
                    if (Global.shownItemList[i].ID == Global.currentItem.ID)//delete the item according to the unique id;
                    {
                        Global.shownItemList.RemoveAt(i);
                        break;
                    }
                    
                }
                */
                //Global.shownItemList.Remove(Global.currentItem);//this will remove the item currentItem points to but
                //this will cause the change of the currentItem will directly affect the shownItemList and listView;
                //this.listViewItem.SelectedIndex = 0;//this cannot change the fact that the currentItem is null;
                //when remove from shownItemList, the control power given to the selectionChanged handler;
            }
            else
                MessageBox.Show("Please select an to-do item first.");
        }

        private void setNextIndexByList(ObservableCollection<ToDoItem> list)
        {
            int currentIndex = list.IndexOf(Global.currentItem);
            int nextIndex = (currentIndex < list.Count) ? currentIndex : 0;//if out of range, set back to 0;
            this.listViewItem.SelectedIndex = nextIndex;
        }

        private void trashButton_Click(object sender, RoutedEventArgs e)
        {
            this.listViewItem.ItemsSource = Global.deletedItemList;
            this.addButton.Visibility = System.Windows.Visibility.Collapsed;
            this.restoreButton.Visibility = System.Windows.Visibility.Visible;
            this.addButton.IsEnabled = false;
            Global.IsInTrash = true;
            activateFootPanel(false);
        }

        private void todoButton_Click(object sender, RoutedEventArgs e)
        {
            this.listViewItem.ItemsSource = Global.shownItemList;
            this.addButton.IsEnabled = true;
            this.addButton.Visibility = System.Windows.Visibility.Visible;
            this.restoreButton.Visibility = System.Windows.Visibility.Collapsed;
            Global.IsInTrash = false;
            //activateFootPanel(true);///needless to activate the edit panel now, which will be automatically activated
            ////when selecting an item in the to-do list;
        }

        private void restoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (Global.currentItem != null)
            {
                int currentIndex = Global.deletedItemList.IndexOf(Global.currentItem);
                Global.currentItem.IsDeleted = false;
                Global.shownItemList.Add(Global.currentItem);
                Global.deletedItemList.Remove(Global.currentItem);
                int nextIndex = (currentIndex < Global.deletedItemList.Count) ? currentIndex : 0;//if out of range, set back to 0;
                if (Global.deletedItemList.Count == 0)///if after deleting operation, there is no item left; just return;
                    return;
                this.listViewItem.SelectedIndex = nextIndex;
                Global.currentItem = Global.deletedItemList[nextIndex];
            }
            else
                MessageBox.Show("Please select a deleted to-do item first.");
        }

        /// <summary>
        /// this event handler used to edit the comment of the currently selected item;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void commentTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Global.currentItem.Comment = this.commentTextBox.Text;
        }

        private void synchButton_Click(object sender, RoutedEventArgs e)
        {
            MyConnector connector = new MyConnector();

            User user = new User();
            connector.synch(user);
            List<ToDoItem> itemsList = user.allItemsList;
            if(itemsList.Count > 0)
            {
                Global.current_max_id = itemsList[0].ID;
                for (int i = 0; i < itemsList.Count; i++)
                {
                    if (itemsList[i].ID > Global.current_max_id)
                        Global.current_max_id = itemsList[i].ID;
                }
                Global.current_max_id++;
            }
            else
            {
                Global.current_max_id = 1;
            }
        }
    }
}

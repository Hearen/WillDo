using System;
using System.Collections.Generic;
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

namespace WillDo.MyControls
{
    /// <summary>
    /// Interaction logic for FootCheckControl.xaml
    /// used to edit the attributes of selected to-do item;
    /// </summary>
    public partial class FLocationControl : UserControl
    {
        private Globals Global = (App.Current as App).Global;

        public FLocationControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// when the deleting button clicked, delete the current item from comboBox and local storage;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboB_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            //all these are tried to access comboBox directly and delete the comboBoxItem selected;
            //later on, found out that access the comboBox is not that easy;
            //so try to delete the string from the bond ObservableCollection which will be pretty easy to achieve;
            //ComboBoxItem comboBoxItem = Global.GetParentObject<ComboBoxItem>(button, "");
            //Console.Write(comboBoxItem.Parent.ToString());
            //CheckboxControl checkBoxControl = Global.GetParentObject<CheckboxControl>(button, "categoryItem");
            //ComboBox comboBox = Global.GetParentObject<ComboBox>(button, "itemsComboBox");
            ///
            StackPanel stackPanel = LogicalTreeHelper.GetParent(button) as StackPanel;
            string deletedItem = (stackPanel.FindName("comboTB") as TextBlock).Text;
            Global.locations.Remove(deletedItem);
            ///using another usercontrol to ease the logical burden here;
            /*
            ItemWithList item = Global.itemWithListDic["Category"];
            if (item.List.Contains(deletedItem))
            {
                item.List.Remove(deletedItem);
                
            }
            else if ((item = Global.itemWithListDic["Location"]).List.Contains(deletedItem))
            {
                item.List.Remove(deletedItem);
            }
            else if ((item = Global.itemWithListDic["Title"]).List.Contains(deletedItem))
            {
                item.List.Remove(deletedItem);
            }
            */
        }

        /// <summary>
        /// change the location for the currently selected to-do item;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Global.currentItem.Location = (sender as ComboBox).SelectedValue as string;
            ///using another usercontrol - FCategory to ease the logical burden here;
            /*
            string itemString = (sender as ComboBox).SelectedValue as string;
            ToDoItem item = Global.currentItem;
            ///this is also not a nice idea to determine the source of the comboBox
            ///which should be reconstructed when the technique is quite enough;
            
            ItemWithList itemWithList = Global.itemWithListDic["Category"];
            if (itemWithList.List.Contains(itemString))
            {
                item.Category = itemString;
            }
            else if ((itemWithList = Global.itemWithListDic["Location"]).List.Contains(itemString))
            {
                item.Location = itemString;
            }
            */
        }

        private void itemsComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                ///Console.WriteLine("*************\n " + itemsComboBox.Text);
                ///testing whether the input text can be withdrawn or not;
                if(!Global.locations.Contains(itemsComboBox.Text) && itemsComboBox.Text.Length > 0)///only add bran-new and not null item;
                {
                    Global.locations.Add(itemsComboBox.Text);
                }
                Global.currentItem.Location = itemsComboBox.Text;
            }
        }


        
        ///current version will not handle several locations and categories in a single item;
        ///still some technical problems need to be solved;
        /*
        private void comboCB_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            string itemString = checkBox.Content as string;
            if (Global.itemWithListDic["Category"].List.Contains(itemString))
            {
                if ((bool)(checkBox.IsChecked) && !Global.currentItem.Category.Contains(itemString))
                {//checkBox is checked and the filtering categories doesn't contain this itemString, 
                    //add it to filtering categories;
                    Global.currentItem.Category = itemString;
                }
                else if (!(bool)(checkBox.IsChecked) && Global.currentItem.Category.Contains(itemString))
                {//the checkBox is not checked or unchecked;
                    //and the filtering categories contains the itemString;
                    //remove it from filtering categories;
                    Global.currentItem.Category = itemString;
                }
            }
            else if (Global.itemWithListDic["Location"].List.Contains(itemString))//content belongs to Location type;
            {
                if ((bool)(checkBox.IsChecked) && !Global.locations.Contains(itemString))
                {//checkBox is checked and the filtering categories doesn't contain this itemString, 
                    //add it to filtering categories;
                    Global.locations.Add(itemString);
                }
                else if (!(bool)(checkBox.IsChecked) && Global.locations.Contains(itemString))
                {//the checkBox is not checked or unchecked;
                    //and the filtering categories contains the itemString;
                    //remove it from filtering categories;
                    Global.locations.Remove(itemString);
                }
            }
            
            //Console.WriteLine(checkBox.ToString());//just for test; what content will be output;
            e.Handled = true;
        }
        */
    }
}

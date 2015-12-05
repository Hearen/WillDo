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
    /// Interaction logic for FCategoryControl.xaml
    /// </summary>
    public partial class FCategoryControl : UserControl
    {
        private Globals Global = (App.Current as App).Global;

        public FCategoryControl()
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
            StackPanel stackPanel = LogicalTreeHelper.GetParent(button) as StackPanel;
            string deletedItem = (stackPanel.FindName("comboTB") as TextBlock).Text;
            Global.categories.Remove(deletedItem);
        }

        /// <summary>
        /// change the location for the currently selected to-do item;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Global.currentItem.Category = (sender as ComboBox).SelectedValue as string;
            //Global.currentItem.LastModified = DateTime.Now;
        }

        private void itemsComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                if(!Global.categories.Contains(itemsComboBox.Text) && itemsComboBox.Text.Length > 0)///only add bran-new and not null item;
                {//at the same time, edit the category of the current item;
                    Global.categories.Add(itemsComboBox.Text);
                }
                Global.currentItem.Category = itemsComboBox.Text;
                ///Global.currentItem.LastModified = DateTime.Now;
                ///we can do this in ToDoItem when any modified operation happened, just change the lastModified;
            }
        }
    }
}

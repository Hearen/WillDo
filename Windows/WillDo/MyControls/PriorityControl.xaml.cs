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
    /// Interaction logic for PriorityControl.xaml
    /// </summary>
    public partial class PriorityControl : UserControl
    {
        private Globals Global = (App.Current as App).Global;
        public PriorityControl()
        {
            InitializeComponent();
        }

        private void itemsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            Grid grid = LogicalTreeHelper.GetParent(comboBox) as Grid;
            string title = (grid.FindName("titleTextBlock") as TextBlock).Text;
            switch(title)
            {
                case "Above Priority": filterByPriority(comboBox); break;
                case "Priority": setPriority(comboBox); break;
                default: break;
            }
        }

        //using the new priority selected by comboBox, filter the allItemList;
        private void filterByPriority(ComboBox comboBox)
        {
            Global.priorityFilter = comboBox.SelectedIndex; //for the item <any> the priority starts with 1 to 10;
            Global.filter();
        }

        ///ToDo how to make sure the new value will be written down to the local storage?
        ///and refresh the listview in time;
        //setting priority for the current selected item in listview; --- store in Globasl;
        private void setPriority(ComboBox comboBox)//ToDo!
        {
            if(Global.currentItem != null)
            {
                ///directly using the Global.currentItem to access the currently selected item in listview;
                ///Global.getItemFromInnerList(Global.currentItem.ID).Priority = comboBox.SelectedIndex + 1;//there is no any item, so the index starts with 0 to 9;
                Global.currentItem.Priority = comboBox.SelectedIndex + 1;
                //there is no any item, so the index starts with 0 to 9;
            }
        }
    }
}

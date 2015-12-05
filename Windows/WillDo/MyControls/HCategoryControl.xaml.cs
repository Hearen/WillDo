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
    /// Interaction logic for CheckboxControl.xaml
    /// </summary>
    public partial class HCategoryControl : UserControl
    {
        private Globals Global = (App.Current as App).Global;

        public HCategoryControl()
        {
            InitializeComponent();
        }

        

        /// <summary>
        /// each time the checkBox clicked, refresh the selected values - including categories and locations;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            string itemString = checkBox.Content as string;
            if ((bool)(checkBox.IsChecked) && !Global.fCategories.Contains(itemString))
            {//checkBox is checked and the filtering categories doesn't contain this itemString, 
                //add it to filtering categories;
                Global.fCategories.Add(itemString);
            }
            else if (!(bool)(checkBox.IsChecked) && Global.fCategories.Contains(itemString))
            {//the checkBox is not checked or unchecked;
                //and the filtering categories contains the itemString;
                //remove it from filtering categories;
                Global.fCategories.Remove(itemString);
            }
            Global.filter();//this operation must be done here for the refreshed locations or categories;
            //Console.WriteLine(checkBox.ToString());//just for test; what content will be output;
            e.Handled = true;
        }
    }
}

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
    /// Interaction logic for TitleComboControl.xaml
    /// </summary>
    public partial class TitleComboControl : UserControl
    {
        private Globals Global = (App.Current as App).Global;

        public TitleComboControl()
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
            Global.titles.Remove(deletedItem);
        }

        /// <summary>
        /// change the location for the currently selected to-do item;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Global.titleFilter = (sender as ComboBox).SelectedValue as string;
            Global.filter();
        }

        private void itemsComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                if (!Global.titles.Contains(itemsComboBox.Text) && itemsComboBox.Text.Length > 0)///only add bran-new items;
                {//at the same time, edit the category of the current item;
                    Global.titles.Add(itemsComboBox.Text);
                }
                Global.titleFilter = itemsComboBox.Text;
                Global.filter();
            }
        }
    }
}

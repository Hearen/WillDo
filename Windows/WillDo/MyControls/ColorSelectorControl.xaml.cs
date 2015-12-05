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
    /// Interaction logic for ColorSelectorControl.xaml
    /// </summary>
    public partial class ColorSelectorControl : UserControl
    {
        Globals Global = (App.Current as App).Global;
        public ColorSelectorControl()
        {
            InitializeComponent();
        }

        private void itemsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            string ss = comboBox.SelectedItem.ToString();
            if(ss != null && ss.Contains(' '))
            {
                string[] s = ss.Split(new char[] { ' ' });
                ///Global.getItemFromInnerList(Global.currentItem.ID).Color = s[1];
                //////directly using the Global.currentItem to access the currently selected item in listview;
                Global.currentItem.Color = s[1];

                ///used to test the result;
                ///Console.WriteLine(s[1]);
            }
        }
    }
}

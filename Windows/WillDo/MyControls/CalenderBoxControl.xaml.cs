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
    /// Interaction logic for calendarBoxControl.xaml
    /// </summary>
    public partial class CalendarBoxControl : UserControl
    {
        private Globals Global = (App.Current as App).Global;
        public CalendarBoxControl()
        {
            InitializeComponent();
            this.dateSelector.DisplayDateStart = DateTime.Now;
            this.dateSelector.SelectedDate = DateTime.Now;
            this.dateSelector.SelectedDateFormat = DatePickerFormat.Long;
        }

        private void dateSelector_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker calendar = sender as DatePicker;
            Grid grid = LogicalTreeHelper.GetParent(calendar) as Grid;
            string title = (grid.FindName("titleTextBlock") as TextBlock).Text;
            switch(title)
            {
                case "Due Date": 
                    //MessageBox.Show("from due by " + calendar.SelectedDate); 
                    ///directly using the Global.currentItem to access the currently selected item in listview;
                    ///Global.getItemFromInnerList(Global.currentItem.ID).Due = calendar.SelectedDate.Value;
                    Global.currentItem.Due = calendar.SelectedDate.Value;
                    break;
                case "Started from": 
                    //MessageBox.Show("from stared from " + calendar.SelectedDate);  
                    break;
                case "Specific Date": filterBySpecificDate(calendar); break;
                default: break;
            }
        }

        private void filterBySpecificDate(DatePicker calendar)
        {
            Global.dueFilter = "<specific date>";
            Global.specificFilter = calendar.SelectedDate.Value;
            Global.filter();
        }
    }
}

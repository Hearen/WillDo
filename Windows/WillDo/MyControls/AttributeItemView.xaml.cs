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
    /// Interaction logic for AttributeItemView.xaml
    /// </summary>
    public partial class AttributeItemView : UserControl
    {
        private Globals Global = (App.Current as App).Global;
        public AttributeItemView()
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
                case "Show":
                    {
                        Global.showFilter = comboBox.SelectedItem as string;
                        Global.filter();
                    }
                    //MessageBox.Show("you just change the show selection!"); 
                    break;
                case "Due by":
                    {
                        dueByHandler(comboBox);
                    }
                    //MessageBox.Show("you just change the due"); 
                    break;
                case "%Complete":
                    {
                        ///directly using the Global.currentItem to access the currently selected item in listview;
                        //ToDoItem currentItem = Global.getItemFromInnerList(Global.currentItem.ID);//get the original object;
                        Global.currentItem.Progress = int.Parse(comboBox.SelectedItem as string);
                        if (Global.currentItem.Progress == 100)
                            Global.currentItem.IsComplete = true;
                        else//make sure when the progress is changed back to less than 100, set it false again;
                            Global.currentItem.IsComplete = false;
                    }
                    //MessageBox.Show((DateTime.Now.DayOfWeek <= DayOfWeek.Sunday).ToString()); 
                    break;
                default: break;
            }
        }

        //this method is used to filter by the date and 
        //insert a new control to header when selected <specific date>;
        private void dueByHandler(ComboBox comBox)
        {
            Global.dueFilter = comBox.SelectedValue as string;
            Global.filter();

            Grid grid = LogicalTreeHelper.GetParent(comBox) as Grid;
            AttributeItemView itemView = LogicalTreeHelper.GetParent(grid) as AttributeItemView;
            StackPanel headerPanel = LogicalTreeHelper.GetParent(itemView) as StackPanel;
            int count = headerPanel.Children.Count;
            //PriorityControl priorityControl = headerPanel.Children[count - 2] as PriorityControl;
            //CheckboxControl categoryControl = headerPanel.Children[count - 1] as CheckboxControl;
            
            if(comBox.SelectedValue.Equals("<specific date>"))//add a new calendar;
            {
                CalendarBoxControl specificCalender = new CalendarBoxControl();
                specificCalender.titleTextBlock.Text = "Specific Date";
                specificCalender.Margin = new Thickness(10);
                specificCalender.Width = 150;
                headerPanel.Children.Insert(count - 3, specificCalender);
                //MessageBox.Show(LogicalTreeHelper.GetParent(itemView).ToString());

                //to meet the need of the Globals.filter, setting the value of specific value to today;

                Global.specificFilter = DateTime.Now; //after this assignment, there is no filtering operation;
            } 
            else//if there is a calendar of specific date, delete it;
            {
                //MessageBox.Show(headerPanel.Children[count - 4].ToString());
                if ((headerPanel.Children[count - 4] as CalendarBoxControl) != null)
                    headerPanel.Children.RemoveAt(count - 4);
            }
        }
    }
}

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

namespace YetAnotherCalendar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DateTime monthToDisplay = DateTime.Now.AddMonths(0);
            createMonthButtons();
            setupCalendarGrid(monthToDisplay);
        }

        /// <summary>
        /// Handle Button clicks of month buttons. Updates CalendarGrid and
        /// window title to reflect this.Discerns which button was pressed by
        /// inspecting the content. 
        /// </summary>
        /// <param name="sender">Assumed to be the button object. No error checking on this.</param>
        /// <param name="e">The event args object</param>
        private void onMonthButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            DateTime month = DateTime.Parse("1 " + button.Content + " " + DateTime.Today.Year.ToString());
            setupCalendarGrid(month);
        }

        /// <summary>
        /// Programatically generate all of the month buttons and the grid
        /// columns to go with them.
        /// </summary>
        private void createMonthButtons()
        {
            DateTime date = DateTime.Parse("1 January 2000");

            for (int i = 0; i < 12; i++)
            {
                // Create a new Column
                ColumnDefinition colDef = new ColumnDefinition();
                colDef.Width = new GridLength(1,GridUnitType.Star);
                MonthButtonsGrid.ColumnDefinitions.Add(colDef);

                // Create a new button
                Button newMonthButton = new Button();
                newMonthButton.Content = date.AddMonths(i).ToString("MMM");
                newMonthButton.Click += new RoutedEventHandler(onMonthButtonClick);
                MonthButtonsGrid.Children.Add(newMonthButton);
                
                // Place the button in the appropriate column
                Grid.SetColumn(newMonthButton, i);
            }

        }
        
        /// <summary>
        /// Creates a single "day" and places it on the grid.
        /// </summary>
        /// <param name="row">The row within the grid in which to place the day</param>
        /// <param name="col">The column within the grid in which to place the day</param>
        /// <param name="day"></param>
        private void createDayOnGrid(int row, int col, int day)
        {
            Button newButton = new Button();
            newButton.Background = (row % 2 == 0 ? Brushes.LightBlue : Brushes.LightGray);
            newButton.BorderThickness = new Thickness(0.5);
            newButton.BorderBrush = Brushes.White;
            newButton.Content = day;
            CalendarGrid.Children.Add(newButton);
            Grid.SetColumn(newButton, col);
            Grid.SetRow(newButton, row);
        }

        /// <summary>
        /// Setups up the XAML defined CalendarGrid to look like a monthly calendar
        /// which displays the given month.
        /// </summary>
        /// <param name="month">The Date whose month we should display.</param>
        private void setupCalendarGrid(DateTime month)
        {
            // setup window title
            MonthViewWindow.Title = month.ToString("MMMM");

            DateTime startOfMonth = new DateTime(month.Year, month.Month, 1);
            DateTime nextMonth = startOfMonth.AddMonths(1);

            // start at the beginning of a week.
            DateTime date = startOfMonth.AddDays(-(int)startOfMonth.DayOfWeek);

            // clear any dates that already exist (e.g. if the month is already populated
            // but another month is selected)
            if (CalendarGrid.RowDefinitions.Count > 2)
                CalendarGrid.RowDefinitions.RemoveRange(2, CalendarGrid.RowDefinitions.Count - 2);
            for (int row = 2; date.Month != nextMonth.Month; row++)
            {
                // create a new row for our dates
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new GridLength(1, GridUnitType.Star);
                CalendarGrid.RowDefinitions.Add(rowDef);
                for (int col = 0; col < CalendarGrid.ColumnDefinitions.Count; col++)
                {
                    createDayOnGrid(row, col, date.Day);
                    date = date.AddDays(1);
                }
            }
        }
    }
}

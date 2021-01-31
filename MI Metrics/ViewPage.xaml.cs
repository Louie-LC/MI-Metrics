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

namespace MI_Metrics
{
    /// <summary>
    /// Interaction logic for ViewPage.xaml
    /// </summary>
    public partial class ViewPage : Page
    {
        ViewPageViewModel viewModel;
        

        public ViewPage(Product product)
        {
            viewModel = new ViewPageViewModel(this, product);
            InitializeComponent();
            this.Title = product.Name + " Metrics";
            ExpandDataGrid(MetricsDataGrid);
        }


        public Product GetViewedProduct()
        {
            return viewModel.ViewedProduct;
        }


        public void ExpandDataGrid(DataGrid dataGrid)
        {
            Style headerStyle = this.FindResource("NumericDataGridHeader") as Style;
            Style elementStyle = this.FindResource("NumericGridElement") as Style;

            // Adds an additional column to the datagrid for each test associated with the current product. 
            foreach (Test test in viewModel.ViewedProduct.Tests)
            {
                string name = test.Name.Replace(" ", "");
                var column = new DataGridTextColumn()
                {
                    Header = test.Name,
                    Binding = new Binding("TestValues." + name),
                    HeaderStyle = headerStyle,
                    ElementStyle = elementStyle,
                    MinWidth = 160,
                    MaxWidth = 320
                };
                dataGrid.Columns.Add(column);
            }
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            FilterPopup.IsOpen = true;
        }

        private void AddTagButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            viewModel.TagSelected((Tag)button.Tag);
        }

        private void RemoveSelectedTagButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Tag tag = (Tag)button.Tag;
            viewModel.RemoveSelectedTag(tag);
        }

        private void SearchBoxButton_Click(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            Button button = (Button)e.OriginalSource;

            if (button.Name.Equals("ClearButton"))
            {
                textBox.Clear();
                viewModel.SearchText = SearchBar.Text;
                viewModel.FilterDHRView();
            }
            else if (button.Name.Equals("SearchButton"))
            {
                viewModel.SearchText = SearchBar.Text;
                viewModel.FilterDHRView();
            }
        }
        
        private void SearchBar_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                viewModel.SearchText = SearchBar.Text;
                viewModel.FilterDHRView();
            }
        }

        private void ClearAllFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.SelectedTags.Clear();
            viewModel.FilterDHRView();
        }

        private void DatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            DatePicker datePicker = (DatePicker)sender;
            DateTime tomorrow = DateTime.Today.AddDays(1);

            datePicker.BlackoutDates.Add(new CalendarDateRange(tomorrow, DateTime.MaxValue));
        }

        private void StartDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StartDatePicker != null && EndDatePicker != null && StartDatePicker.SelectedDate > EndDatePicker.SelectedDate)
            {
                EndDatePicker.SelectedDate = null;
            }

            if(StartDatePicker.SelectedDate != null && EndDatePicker.SelectedDate == null)
            {
                EndDatePicker.DisplayDate = (DateTime)StartDatePicker.SelectedDate;
            }

            viewModel.FilterDHRView();
        }

        private void EndDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StartDatePicker != null && EndDatePicker != null && EndDatePicker.SelectedDate < StartDatePicker.SelectedDate)
            {
                StartDatePicker.SelectedDate = null;
            }
            
            if(EndDatePicker.SelectedDate != null && StartDatePicker.SelectedDate == null)
            {
                StartDatePicker.DisplayDate = (DateTime)EndDatePicker.SelectedDate;
            }

            viewModel.FilterDHRView();
        }

        private void DatePicker_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)e.OriginalSource;
            if (button.Name.Equals("ClearButton"))
            {
                ((DatePicker)sender).SelectedDate = null;
            }
        }
    }
}

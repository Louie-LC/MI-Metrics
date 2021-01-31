using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
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
    /// Interaction logic for UploadPage.xaml
    /// </summary>
    public partial class UploadPage : Page
    {
        UploadPageViewModel viewModel;

        bool validFileSelected = false;

        
        public UploadPage()
        {
            InitializeComponent();
            Results results = (Results)PageContentGrid.TryFindResource("results");
                
            viewModel = new UploadPageViewModel(this, results);
        }

        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            SelectFileResult result = viewModel.SelectCSVFile();

            if (!result.filePath.Equals(""))
            {
                if (result.validFileSelected)
                {
                    validFileSelected = true;
                    ErrorIconGrid.Visibility = Visibility.Hidden;
                    SuccessIconGrid.Visibility = Visibility.Visible;
                }
                else if (!result.validFileSelected)
                {
                    validFileSelected = false;
                    ErrorIconGrid.Visibility = Visibility.Visible;
                    SuccessIconGrid.Visibility = Visibility.Hidden;
                }
            }
            CheckPullButtonStatus();
        }

        private void RemoveFileButton_Click(object sender, RoutedEventArgs e)
        {
            validFileSelected = false;
            ErrorIconGrid.Visibility = Visibility.Hidden;
            SuccessIconGrid.Visibility = Visibility.Hidden;
            viewModel.ClearSelectedFile();
            CheckPullButtonStatus();
        }

        private void CheckPullButtonStatus()
        {
            bool systemSelected = UploadProductSelectBox.SelectedItem != null;
            PullDataButton.IsEnabled = systemSelected && validFileSelected;
        }

        private void UploadProductSelectBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckPullButtonStatus();
        }

        private void PullDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (UploadProductSelectBox.SelectedItem == null || !validFileSelected) return;

            Product product = (Product)UploadProductSelectBox.SelectedItem;
            bool pullSuccesful = viewModel.PullData(product);

            if (pullSuccesful)
            {
                // If the pull was succesful, the viewmodel will clear it's selected file.
                validFileSelected = false;
                CheckPullButtonStatus();
                CheckUploadDataButtonStatus();
                ErrorIconGrid.Visibility = Visibility.Hidden;
                SuccessIconGrid.Visibility = Visibility.Hidden;
            }

        }

        private void DeleteResultMenuItem_Click(object sender, RoutedEventArgs e)
        {
            viewModel.DeleteSelectedTestResult();
            CheckUploadDataButtonStatus();
        }

        private void DataGridOptionsButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            IntermediateTestResult result = (IntermediateTestResult)button.Tag;
            viewModel.IntermediateTestResultSelected(result);
        }

        private void CheckUploadDataButtonStatus()
        {
            UploadDataButton.IsEnabled = viewModel.ErrorCount == 0 && viewModel.TotalCount > 0;
        }

        private void UploadDataButton_Click(object sender, RoutedEventArgs e)
        {
            // The first two clauses of the IF statment shouldn't ever really be needed.
            // The UploadDataButton is disabled if either the TestCount is 0 or if the
            // error count isn't at 0. The IF is mostly a precaution in case something
            // goes wrong and the button is unintentionally enabled and pressed when
            // it shouldn't be.
            int totalTestCount = viewModel.TotalCount;
            int numberOfErrors = viewModel.ErrorCount;
            if (totalTestCount == 0)
            {
                UploadPageModalUtil.ShowNoTestResultsModal(Window.GetWindow(this));
            }
            else if (numberOfErrors != 0)
            {
                UploadPageModalUtil.ShowErrorModal(numberOfErrors, Window.GetWindow(this));
            }
            else
            {
                int numberOfWarnings = viewModel.WarningCount;
                int addedTestcount = totalTestCount - numberOfWarnings;
                if (UploadPageModalUtil.DisplayConfirmationWindow(addedTestcount, numberOfWarnings, Window.GetWindow(this)))
                {
                    int uploadErrors = viewModel.UploadTestResults();
                    CheckUploadDataButtonStatus();

                    if(uploadErrors == 0)
                    {
                        UploadPageModalUtil.ShowUploadSuccessModal(totalTestCount, Window.GetWindow(this));
                    }
                    else
                    {
                        UploadPageModalUtil.ShowUploadErrorModal(totalTestCount, uploadErrors, Window.GetWindow(this));
                    }
                    viewModel.ClearTestResults();
                    CheckUploadDataButtonStatus();
                }
            }
        }

        private void TestGroupingToggle_Checked(object sender, RoutedEventArgs e)
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(TestResultsDataGrid.ItemsSource);
            if(collectionView != null && collectionView.CanGroup)
            {
                collectionView.GroupDescriptions.Clear();
                collectionView.GroupDescriptions.Add(new PropertyGroupDescription("SystemDescription"));
            }
        }

        private void TestGroupingToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(TestResultsDataGrid.ItemsSource);
            if (collectionView != null)
            {
                collectionView.GroupDescriptions.Clear();
            }
        }

        private void ClearDataButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.ClearTestResults();
            CheckUploadDataButtonStatus();
        }
    }
}

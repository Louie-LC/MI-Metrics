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
using System.IO;

namespace MI_Metrics
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        HomePageViewModel viewModel;

        static ListBox activeListBox;

        // Members used when adding a new system.
        private bool validProductImageSelected = true;
        private bool validProductNameEntered = false;

        // Members used when editing a system.
        private bool validEditProductImageSelected = false;
        private bool validEditProductNameEntered = false;
        private bool editProductImageChanged = false;
        private bool editProductNameChanged = false;



        public HomePage()
        {
            viewModel = new HomePageViewModel(this);
            viewModel.productManager = MainWindow.productManager;

            InitializeComponent();
            ImageExtensionText.Text = "Valid Image Extensions: " + viewModel.ValidImageExtensions;
            EditImageExtensionText.Text = "Valid Image Extensions: " + viewModel.ValidImageExtensions;
            AddProductUtil.DeleteAllTempImageFiles();
            EditProductUtil.DeleteAllTempImageFiles();
        }

        private void HandlePreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Routes the mouse wheel event directed at the list of products to the homepage window so that the window will scroll properly.
            if (!e.Handled)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                var parent = ((Control)sender).Parent as UIElement;
                parent.RaiseEvent(eventArg);
            }
        }


        private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Clear();
            Keyboard.Focus(SearchBox);

        }
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            SetDeleteButtonVisibility((TextBox)sender);
            viewModel.FilterProductView(SearchBox.Text);
            ClearEditProductData();
        }
        private void SetDeleteButtonVisibility(TextBox searchBox)
        {
            if (searchBox.Text.Equals(""))
            {
                ClearSearchButton.Visibility = Visibility.Hidden;
            }
            else
            {
                ClearSearchButton.Visibility = Visibility.Visible;
            }
        }
        public void StreamImageFromSource(string sourcePath, Image destinationImage)
        {
            if (File.Exists(sourcePath) || sourcePath.Equals("pack://application:,,,/Images/logo_rgb_2x_l.png"))
            {
                BitmapImage bitImage = new BitmapImage();
                bitImage.BeginInit();
                bitImage.CacheOption = BitmapCacheOption.OnLoad;
                bitImage.DecodePixelWidth = 320;
                bitImage.UriSource = new Uri(sourcePath);
                bitImage.EndInit();
                destinationImage.Source = bitImage;
            }
        }
        private void ExpandButton_Click(object sender, RoutedEventArgs e)
        {
            InputDrawer.Visibility = Visibility.Visible;
            ContractButton.Visibility = Visibility.Visible;
            ExpandButton.Visibility = Visibility.Hidden;
            ManagementTabControl.Visibility = Visibility.Visible;
        }
        private void ContractButton_Click(object sender, RoutedEventArgs e)
        {
            InputDrawer.Visibility = Visibility.Collapsed;
            ContractButton.Visibility = Visibility.Hidden;
            ExpandButton.Visibility = Visibility.Visible;
            ManagementTabControl.Visibility = Visibility.Collapsed;
        }

        private void SubmitDataButton_Click(object sender, RoutedEventArgs e)
        {
            Button allDataButton = (Button)sender;
            Product product = (Product)allDataButton.Tag;
        }

        private void ClearSelection_Click(object sender, RoutedEventArgs e)
        {
            if(activeListBox != null)
            {
                activeListBox.SelectedItem = null;
                activeListBox = null;
            }
        }

        private void UnitSelectList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            Product product = (Product)listBox.Tag;

            if (activeListBox == null) activeListBox = listBox;
            else if (listBox != activeListBox)
            {
                activeListBox.SelectedItem = null;
                activeListBox = listBox;
            }
        }

        private void NavigationUploadData_Click(object sender, RoutedEventArgs e)
        {
            Page uploadPage = MainWindow.GetPage(PageType.Upload);
            NavigationService.Navigate(uploadPage);
        }

        #region The logic for adding new systems
        private void UploadFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (SubmitSuccessGrid.IsVisible) SubmitSuccessGrid.Visibility = Visibility.Hidden;
            SelectFileResult selectedFile = viewModel.SelectFile();
            if (!selectedFile.filePath.Equals(""))
            {
                SetFileUIElements(selectedFile.validFileSelected);
                if (selectedFile.validFileSelected)
                    validProductImageSelected = true;
                else
                    validProductImageSelected = false;
            }

            CheckSubmitProductButtonState();
        }
        private void SetFileUIElements(bool isValidFile)
        {
            // Shows the correct status icon based on if the seleted file was validated.
            if (!isValidFile)
            {
                SuccessIconGrid.Visibility = Visibility.Hidden;
                ErrorIconGrid.Visibility = Visibility.Visible;
            }
            else
            {
                ErrorIconGrid.Visibility = Visibility.Hidden;
                SuccessIconGrid.Visibility = Visibility.Visible;
            }
            // Show the remove button if a file was selected
            if (!(SelectedImageFile.Text.Equals("") || SelectedImageFile.Text == null))
            {
                RemoveFileButton.IsEnabled = true;
            }


        }
        private void RemoveFileButton_Click(object sender, RoutedEventArgs e)
        {
            SetFileToDefaultState();
        }
        private void SetFileToDefaultState()
        {
            viewModel.ClearSelectedFile();
            ErrorIconGrid.Visibility = Visibility.Hidden;
            SuccessIconGrid.Visibility = Visibility.Hidden;
            RemoveFileButton.IsEnabled = false;
            validProductImageSelected = true;
            CheckSubmitProductButtonState();
            viewModel.addProductUtil.RemoveAllFileBasedErrors();
        }
        private void ProductNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SubmitSuccessGrid.IsVisible) SubmitSuccessGrid.Visibility = Visibility.Hidden;
            string productName = ((TextBox)sender).Text;
            // Check if the current name entered is valid
            if (viewModel.addProductUtil.ValidateName(productName)) validProductNameEntered = true;
            else validProductNameEntered = false;
            CheckSubmitProductButtonState();
        }
        private void CheckSubmitProductButtonState()
        {
            if (validProductImageSelected && validProductNameEntered) SubmitProductButton.IsEnabled = true;
            else SubmitProductButton.IsEnabled = false;
        }
        private void SubmitProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.addProductUtil.ValidateCharactersInName(ProductNameBox.Text))
            {
                viewModel.addProductUtil.AddProduct(ProductNameBox.Text, SelectedImageFile.Text);
                SetFileToDefaultState();
                ProductNameBox.Text = "";
                viewModel.FilterProductView(SearchBox.Text);
                SubmitSuccessGrid.Visibility = Visibility.Visible;
                viewModel.AddProductErrorMessages.Clear();
            }
            else
            {
                validProductNameEntered = false;
                CheckSubmitProductButtonState();
            }
        }
        private void ClearProductButton_Click(object sender, RoutedEventArgs e)
        {
            ProductNameBox.Text = "";
            SetFileToDefaultState();
        }
        #endregion

        #region The logic for editing a system.
        private void AbbreviatedProductList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UpdateSuccessGrid.IsVisible)
                UpdateSuccessGrid.Visibility = Visibility.Hidden;
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                ClearEditProductData();
                viewModel.editProductUtil.InformSystemNotRequired();
                Product product = (Product)e.AddedItems[0];
                if (!product.ImagePath.Equals("pack://application:,,,/Images/logo_rgb_2x_l.png"))
                {
                    viewModel.EditedImageFile = System.IO.Path.GetFileName(product.ImagePath);
                    EditSuccessIconGrid.Visibility = Visibility.Visible;
                    EditRemoveFileButton.IsEnabled = true;
                }
                EditProductNameBox.Text = product.Name;
                EditUploadFileButton.IsEnabled = true;
                validEditProductNameEntered = true;
                validEditProductImageSelected = true;
            }
            else
            {
                viewModel.editProductUtil.InformSystemRequired();
            }

            CheckUpdateProductButtonState();
        }

        private void ClearEditProductData()
        {
            viewModel.ClearEditImageFile();
            EditProductNameBox.Text = "";
            UpdateProductButton.IsEnabled = false;
            EditUploadFileButton.IsEnabled = false;
            EditRemoveFileButton.IsEnabled = false;
            EditSuccessIconGrid.Visibility = Visibility.Hidden;
            EditErrorIconGrid.Visibility = Visibility.Hidden;
            editProductImageChanged = false;
            editProductNameChanged = false;
            validEditProductImageSelected = false;
            validEditProductNameEntered = false;

            viewModel.editProductUtil.RemoveAllFileBasedErrors();
            viewModel.editProductUtil.RemoveAllNameBasedErrors();

            CheckUpdateProductButtonState();
        }
        private void CheckUpdateProductButtonState()
        {
            bool updateAvailable = (editProductNameChanged || editProductImageChanged) && validEditProductNameEntered && validEditProductImageSelected;
            if (updateAvailable)
            {
                UpdateProductButton.IsEnabled = true;
            }
            else
            {
                UpdateProductButton.IsEnabled = false;
            }
        }
        private void EditProductNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (UpdateSuccessGrid.IsVisible)
                UpdateSuccessGrid.Visibility = Visibility.Hidden;

            if (AbbreviatedProductList.SelectedIndex != -1)
            {
                if (EditProductNameBox.Text.Equals(((Product)AbbreviatedProductList.SelectedItem).Name))
                {
                    editProductNameChanged = false;
                    validEditProductNameEntered = true;
                    viewModel.editProductUtil.RemoveAllNameBasedErrors();
                }
                else
                {
                    if (!editProductNameChanged)
                        viewModel.editProductUtil.NameChagned();
                    editProductNameChanged = true;
                    string productName = ((TextBox)sender).Text;
                    if (viewModel.editProductUtil.ValidateName(productName))
                        validEditProductNameEntered = true;
                    else
                        validEditProductNameEntered = false;
                }
            }
            CheckUpdateProductButtonState();
        }

        private void EditSystemClearButton_Click(object sender, RoutedEventArgs e)
        {
            AbbreviatedProductList.SelectedItem = null;
            ClearEditProductData();
        }

        private void EditUploadFileButton_Click(object sender, RoutedEventArgs e)
        {
            Product selectedProduct = (Product)AbbreviatedProductList.SelectedItem;
            if (UpdateSuccessGrid.IsVisible) UpdateSuccessGrid.Visibility = Visibility.Hidden;
            SelectFileResult selectedFile = viewModel.SelectEditFile();
            if (!selectedFile.filePath.Equals(""))
            {
                editProductImageChanged = true;
                SetEditFileUIElements(selectedFile.validFileSelected);
                if (selectedFile.validFileSelected)
                    validEditProductImageSelected = true;
                else
                    validEditProductImageSelected = false;
            }
            CheckUpdateProductButtonState();
        }

        private void SetEditFileUIElements(bool isValidFile)
        {
            // Shows the correct status icon based on if the seleted file was validated.
            if (!isValidFile)
            {
                EditSuccessIconGrid.Visibility = Visibility.Hidden;
                EditErrorIconGrid.Visibility = Visibility.Visible;
            }
            else
            {
                EditErrorIconGrid.Visibility = Visibility.Hidden;
                EditSuccessIconGrid.Visibility = Visibility.Visible;
            }
            // Show the remove button if a file was selected
            if (!(EditSelectedImageFile.Text.Equals("") || EditSelectedImageFile.Text == null))
            {
                EditRemoveFileButton.IsEnabled = true;
            }
        }

        private void EditRemoveFileButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.editProductUtil.InformEditImageRemoved();
            viewModel.ClearEditImageFile();
            editProductImageChanged = true;
            validEditProductImageSelected = true;
            EditRemoveFileButton.IsEnabled = false;
            EditSuccessIconGrid.Visibility = Visibility.Hidden;
            EditErrorIconGrid.Visibility = Visibility.Hidden;
            CheckUpdateProductButtonState();
        }

        private void UpdateProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.editProductUtil.ValidateCharactersInName(EditProductNameBox.Text))
            {
                Product product = (Product)AbbreviatedProductList.SelectedItem;

                if (editProductNameChanged && editProductImageChanged)
                {
                    viewModel.editProductUtil.UpdateProduct(product, EditProductNameBox.Text, EditSelectedImageFile.Text);
                }
                else if (editProductNameChanged)
                {
                    viewModel.editProductUtil.UpdateProductName(product, EditProductNameBox.Text);
                }
                else if (editProductImageChanged)
                {
                    viewModel.editProductUtil.UpdateProductImage(product, EditSelectedImageFile.Text);
                }
                AbbreviatedProductList.SelectedItem = null;
                ClearEditProductData();
                UpdateSuccessGrid.Visibility = Visibility.Visible;
                // If the system was updated successfully, EditProductUtil needs to think a system has been 
                // selected to hide the warning message about there not being a system selected,
                viewModel.editProductUtil.InformSystemNotRequired();
                viewModel.FilterProductView(SearchBox.Text);
            }
            else
            {
                validProductNameEntered = false;
                CheckUpdateProductButtonState();
            }
        }

        private void OnlyImage_Loaded(object sender, RoutedEventArgs e)
        {
            Image image = (Image)sender;
            Product product = (Product)image.Tag;

            StreamImageFromSource(product.ImagePath, image);

            viewModel.images.Add(image);
        }
        #endregion


    }
}

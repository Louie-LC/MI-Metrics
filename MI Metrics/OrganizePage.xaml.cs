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

namespace MI_Metrics
{
    /// <summary>
    /// Interaction logic for OrganizePage.xaml
    /// </summary>
    public partial class OrganizePage : Page
    {
        private OrganizePageViewModel viewModel;
        public OrganizePage()
        {
            viewModel = new OrganizePageViewModel(this);
            InitializeComponent();
        }

        private void SyncProductSelectBoxes(Product product)
        {
            Product unitSelectedProduct = (Product)UnitProductSelectBox.SelectedItem;
            Product tagSelectedProduct = (Product)TagProductSelectBox.SelectedItem;
            Product testSelectedProduct = (Product)TestProductSelectBox.SelectedItem;

            if(product != unitSelectedProduct)
            {
                UnitProductSelectBox.SelectedItem = product;
            }
            if(tagSelectedProduct != product)
            {
                TagProductSelectBox.SelectedItem = product;
            }
            if(testSelectedProduct != product)
            {
                TestProductSelectBox.SelectedItem = product;
            }
        }

        private void MainProductSelectBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Only one product select box should use this as its selection changed event.
            // All other comboboxes should use ProductSelectBox_SelectionChanged
            Product product = e.AddedItems.Count > 0 ? (Product)e.AddedItems[0] : null;
            SyncProductSelectBoxes(product);
            if (product != null)
            {
                viewModel.UpdateUnitView(product);
                viewModel.UpdateTagView(product);
            }
            SetTagToDefault();
            SetUnitToDefault();

            CheckSubmitUnitButtonStatus();

        }

        private void ProductSelectBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Product product = e.AddedItems.Count > 0 ? (Product)e.AddedItems[0] : null;
            SyncProductSelectBoxes(product);
        }


        #region System Tests
        private void TestNameTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (SubmitTestButton.Visibility == Visibility.Visible)
                {
                    AutomationUtil.PressButton(SubmitTestButton);
                }
                else if (UpdateTestButton.Visibility == Visibility.Visible)
                {
                    AutomationUtil.PressButton(UpdateTestButton);
                }
            }
        }

        private void ClearTestButton_Click(object sender, RoutedEventArgs e)
        {
            ClearTestInformation();
            CheckSubmitTestButtonStatus();
        }

        private void ClearTestInformation()
        {
            ProductTestGrid.SelectedItem = null;
            TestNameTextBox.Text = null;

            UpdateTestButton.Visibility = Visibility.Hidden;
            SubmitTestButton.Visibility = Visibility.Visible;
        }
        public void CheckSubmitTestButtonStatus()
        {
            bool testNameEntered = TextBoxHelper.GetSuccess(TestNameTextBox);
            bool testProductSelected = TestProductSelectBox.SelectedItem != null;
            SubmitTestButton.IsEnabled = testProductSelected && testNameEntered;
            UpdateTestButton.IsEnabled = testNameEntered;
        }

        private void SubmitTestButton_Click(object sender, RoutedEventArgs e)
        {
            string name = TestNameTextBox.Text;
            if (name.Trim().Equals("") || name == null || !DatabaseHelper.ValidateCharacters(name))
            {
                TextBoxHelper.SetSuccess(TestNameTextBox, false);
                TextBoxHelper.SetError(TestNameTextBox, true);
                CheckSubmitTestButtonStatus();
                return;
            }

            Product product = (Product)(TestProductSelectBox.SelectedItem);
            if (product == null) return; // This statement should never be possible, since the submit button only activates if a product is selected.

            if( viewModel.AddTest(name, product))
            {
                ClearTestInformation();
            }
            CheckSubmitTestButtonStatus();
        }

        private void ProductTestGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 0)
            {
                SubmitTestButton.Visibility = Visibility.Hidden;
                UpdateTestButton.Visibility = Visibility.Visible;
                Test test = (Test)e.AddedItems[0];
                TestNameTextBox.Text = test.Name;
            }
        }

        private void UpdateTestButton_Click(object sender, RoutedEventArgs e)
        {
            string name = TestNameTextBox.Text;
            if (name == null || name.Trim().Equals("") || !DatabaseHelper.ValidateCharacters(name))
            {
                TextBoxHelper.SetSuccess(TestNameTextBox, false);
                TextBoxHelper.SetError(TestNameTextBox, true);
                CheckSubmitTestButtonStatus();
                return;
            }

            Test test = (Test)ProductTestGrid.SelectedItem;
            if (test == null) return; // This statement should never be possible, since the submit button only activates if a product is selected.

            if(viewModel.UpdateTest(test, name))
            {
                ClearTestInformation();
            }
            CheckSubmitTestButtonStatus();
        }

        private void TestNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text.Trim().Equals(""))
            {
                TextBoxHelper.SetSuccess(TestNameTextBox, false);
                TextBoxHelper.SetError(TestNameTextBox, true);
            }
            else
            {
                TextBoxHelper.SetSuccess(TestNameTextBox, true);
                TextBoxHelper.SetError(TestNameTextBox, false);
            }
            CheckSubmitTestButtonStatus();
        }

        private void TestProductSelectBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            Product product = e.AddedItems.Count > 0 ? (Product)e.AddedItems[0] : null;
            SyncProductSelectBoxes(product);
            ClearTestInformation();
            if (product != null)
            {
                viewModel.BuilTestView(product);
            }
            CheckSubmitTestButtonStatus();
        }

        private void TestNameTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            DatabaseHelper.ValidateCharacters(e);
        }

        private void TestNameTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            TextValidator.ValidatePastedText(e);
        }

        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (e.KeyboardDevice.IsKeyDown(Key.Tab))
                ((TextBox)sender).SelectAll();
        }

        private void TextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            ((TextBox)sender).Select(0, 0);
        }
        #endregion


        #region TagUnitLinks

        private void TagNameBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!DatabaseHelper.ValidateCharacters(e.Text))
            {
                e.Handled = true;
            }
        }

        private void TagNameBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            TextValidator.ValidatePastedText(e);
        }

        private void TagNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (String.IsNullOrEmpty(textBox.Text))
            {
                TextBoxHelper.SetError(textBox, true);
                TextBoxHelper.SetSuccess(textBox, false);
            }
            else
            {
                TextBoxHelper.SetError(textBox, false);
                TextBoxHelper.SetSuccess(textBox, true);
            }
            CheckSubmitTagButtonStatus();
        }

        private void CheckSubmitTagButtonStatus()
        {
            Product product = (Product)TagProductSelectBox.SelectedItem;
            SubmitTagButton.IsEnabled = TextBoxHelper.GetSuccess(TagDescriptionBox) && product != null;
            UpdateTagButton.IsEnabled = SubmitTagButton.IsEnabled;

        }

        private void SetTagToDefault()
        {
            TagDescriptionBox.Text = "";
            TagDataGrid.SelectedItem = null;
            viewModel.SelectedTag = null;
            SubmitTagButton.Visibility = Visibility.Visible;
            UpdateTagButton.Visibility = Visibility.Hidden;
        }

        private void SubmitTagButton_Click(object sender, RoutedEventArgs e)
        {
            Product product = (Product)TagProductSelectBox.SelectedItem;
            string tagDescription = TagDescriptionBox.Text.Trim();
            if (product != null && !String.IsNullOrEmpty(tagDescription))
            {
                if(viewModel.AddTag(tagDescription, product))
                {
                    SetTagToDefault();
                    TagDescriptionBox.Focus();
                }
            }
        }

        private void TagNameBox_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if(SubmitTagButton.Visibility == Visibility.Visible)
                {
                    AutomationUtil.PressButton(SubmitTagButton);
                }
                else if(UpdateTagButton.Visibility == Visibility.Visible)
                {
                    AutomationUtil.PressButton(UpdateTagButton);
                }
            }
        }

        private void UnitDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems.Count != 0)
            {
                TagCheckBoxColumn.Visibility = Visibility.Visible;
                viewModel.CheckTagSelectionCheckBoxes((Unit)e.AddedItems[0]);
            }
            else
            {
                TagCheckBoxColumn.Visibility = Visibility.Collapsed;
            }
        }

        void TagChecked(object sender, RoutedEventArgs e)
        {
            Tag selectedTag = (Tag)TagDataGrid.SelectedItem;
            Unit selectedUnit = (Unit)UnitDataGrid.SelectedItem;
            if(selectedTag != null && selectedUnit != null && selectedTag.ApplicableToUnit)
            {
                viewModel.CreateLink(selectedTag, selectedUnit);
            }
        }

        void TagUnchecked(object sender, RoutedEventArgs e)
        {
            Tag selectedTag = (Tag)TagDataGrid.SelectedItem;
            Unit selectedUnit = (Unit)UnitDataGrid.SelectedItem;
            if(selectedTag != null &&  selectedUnit != null && !selectedTag.ApplicableToUnit)
            {
                viewModel.DestoryLink(selectedTag, selectedUnit);
            }

        }

        public void TagDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems.Count != 0)
            {
                Tag selectedTag = (Tag)e.AddedItems[0];
                TagDescriptionBox.Text = selectedTag.Description;
                SubmitTagButton.Visibility = Visibility.Hidden;
                UpdateTagButton.Visibility = Visibility.Visible;
                viewModel.SelectedTag = selectedTag;
            }
        }

        private void ClearTagButton_Click(object sender, RoutedEventArgs e)
        {
            SetTagToDefault();
        }

        private void UpdateTagButton_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxHelper.GetSuccess(TagDescriptionBox))
            {
                if (viewModel.UpdateSelectedTag(TagDescriptionBox.Text))
                {
                    SetTagToDefault();
                    TagDescriptionBox.Focus();
                }
            }
        }
        #endregion

        private void CheckSubmitUnitButtonStatus()
        {
            Product product = (Product)UnitProductSelectBox.SelectedItem;
            SubmitUnitButton.IsEnabled = product != null && TextBoxHelper.GetSuccess(UnitDescriptionBox) && TextBoxHelper.GetSuccess(UnitPartNumberBox);
            UpdateUnitButton.IsEnabled = SubmitUnitButton.IsEnabled;
        }

        private void UnitPartNumberBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            long dummy;
            if(!Int64.TryParse(e.Text, out dummy))
            {
                e.Handled = true;
            }
        }

        private void UnitPartNumberBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            long dummy;
            if(String.IsNullOrEmpty(textBox.Text) || !Int64.TryParse(textBox.Text, out dummy))
            {
                TextBoxHelper.SetError(textBox, true);
                TextBoxHelper.SetSuccess(textBox, false);
            }
            else
            {
                TextBoxHelper.SetError(textBox, false);
                TextBoxHelper.SetSuccess(textBox, true);
            }

            CheckSubmitUnitButtonStatus();
        }

        private void UnitPartNumberBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            TextValidator.ValidatePastedLong(e);
        }

        private void UnitDescriptionBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!DatabaseHelper.ValidateCharacters(e.Text))
            {
                e.Handled = true;
            }
        }

        private void UnitDescriptionBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (String.IsNullOrEmpty(textBox.Text))
            {
                TextBoxHelper.SetError(textBox, true);
                TextBoxHelper.SetSuccess(textBox, false);
            }
            else
            {
                TextBoxHelper.SetError(textBox, false);
                TextBoxHelper.SetSuccess(textBox, true);
            }

            CheckSubmitUnitButtonStatus();
        }

        private void UnitDescriptionBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            TextValidator.ValidatePastedText(e);
        }

        private void ClearUnitButton_Click(object sender, RoutedEventArgs e)
        {
            SetUnitToDefault();
        }

        private void SetUnitToDefault()
        {
            UnitPartNumberBox.Text = "";
            UnitDescriptionBox.Text = "";
            UpdateUnitButton.Visibility = Visibility.Hidden;
            SubmitUnitButton.Visibility = Visibility.Visible;
            UnitManagerDataGrid.SelectedItem = null;
        }

        private void UnitManagerDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            Unit unit = (Unit)dataGrid.SelectedItem;
            if(unit != null)
            {
                UnitPartNumberBox.Text = unit.PartNumber.ToString();
                UnitDescriptionBox.Text = unit.Description;
                UpdateUnitButton.Visibility = Visibility.Visible;
                SubmitUnitButton.Visibility = Visibility.Hidden;
            }
        }

        private void SubmitUnitButton_Click(object sender, RoutedEventArgs e)
        {
            Product product = (Product)UnitProductSelectBox.SelectedItem;
            if (product == null)
                return;
            string partNumber = UnitPartNumberBox.Text;
            string description = UnitDescriptionBox.Text;
            if(viewModel.AddUnit(partNumber, description, product))
            {
                SetUnitToDefault();
                UnitPartNumberBox.Focus();
            }
        }

        private void UnitTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter && SubmitUnitButton.Visibility == Visibility.Visible)
            {
                AutomationUtil.PressButton(SubmitUnitButton);
            }
            else if(e.Key == Key.Enter && UpdateUnitButton.Visibility == Visibility.Visible)
            {
                AutomationUtil.PressButton(UpdateUnitButton);
            }
        }

        private void UpdateUnitButton_Click(object sender, RoutedEventArgs e)
        {
            Unit unit = (Unit)UnitManagerDataGrid.SelectedItem;
            if (unit == null)
                return;
            string partNumber = UnitPartNumberBox.Text;
            string description = UnitDescriptionBox.Text;
            if (viewModel.UpdateUnit(partNumber, description, unit))
            {
                SetUnitToDefault();
                UnitPartNumberBox.Focus();
            }
        }
    }
}

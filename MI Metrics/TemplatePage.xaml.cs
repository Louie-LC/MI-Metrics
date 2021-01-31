using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;


namespace MI_Metrics
{
    /// <summary>
    /// Interaction logic for TemplatePage.xaml
    /// </summary>
    public partial class TemplatePage : Page
    {
        private TemplatePageViewModel viewModel;

        private List<DataGrid> dataGrids;

        private DataGrid selectedDataGrid;
        private DataGridRow selectedRow;
        private DataGridRow editedRow;
        private TestStep editedTestStep;

        public TemplatePage()
        {
            InitializeComponent();
            viewModel = new TemplatePageViewModel(this);
            selectedDataGrid = null;
            selectedRow = null;
            editedRow = null;
            editedTestStep = null;
            dataGrids = new List<DataGrid>();
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

        private void TestStepProductSelectBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetTemplateToDefault();
            SetTestStepToDefault();
            dataGrids.Clear();
            TestSelectBox.SelectedItem = null;
            if (e.AddedItems.Count != 0)
            {
                Product product = (Product)e.AddedItems[0];

                viewModel.BuildViewedTestList(product);
                viewModel.BuildViewedControlFileList(product);
            }
            else
            {
                viewModel.ClearTestList();
                viewModel.ClearControlFileList();
            }
            CheckControlFileBoxStatus();
            CheckCreateTemplateButtonState();
        }


        private void LimitTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            TextValidator.ValidatePastedDouble(e);
        }

        private void LimitTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int i;
            TextBox textBox = (TextBox)sender;
            if (Int32.TryParse(e.Text, out i)) return;

            bool newPeriodEntered = e.Text.Equals(".");
            bool newNegativeEntered = e.Text.Equals("-");
            if (!(newPeriodEntered || newNegativeEntered))
            {
                e.Handled = true;
            }
            else if (newPeriodEntered && textBox.Text.Contains(".") && !textBox.SelectedText.Contains("."))
            {
                e.Handled = true;
            }

            else if (newNegativeEntered && textBox.Text.Contains("-") && !textBox.SelectedText.Contains("-"))
            {
                e.Handled = true;
            }
        }

        private void LimitTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            double dummy;
            if (Double.TryParse(textBox.Text, out dummy) || textBox.Text.Equals(""))
            {
                SetLimitStatus(textBox, true);
            }
            else
            {
                SetLimitStatus(textBox, false);
            }

            CheckSubmitTestStepButtonStatus();
        }

        private void StepTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int dummy;
            if (!(e.Text.Equals(".") || Int32.TryParse(e.Text, out dummy)))
            {
                e.Handled = true;
            }
        }

        private void MStepTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int dummy;
            if (!Int32.TryParse(e.Text, out dummy))
            {
                e.Handled = true;
            }
        }

        private void SubStepTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int dummy;
            if (!Int32.TryParse(e.Text, out dummy))
            {
                e.Handled = true;
            }
        }

        private void StepTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            SetMandatoryTextBoxState(textBox);
            CheckSubmitTestStepButtonStatus();
        }

        private void MStepTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            SetMandatoryTextBoxState(textBox);
            CheckSubmitTestStepButtonStatus();
        }

        public void SetMandatoryTextBoxState(TextBox textBox)
        {
            if (textBox.Text.Trim().Equals("") || textBox.Text == null)
            {
                TextBoxHelper.SetError(textBox, true);
                TextBoxHelper.SetSuccess(textBox, false);
            }
            else
            {
                TextBoxHelper.SetError(textBox, false);
                TextBoxHelper.SetSuccess(textBox, true);
            }
        }

        private void SetLimitStatus(UIElement limitTextBox, bool isValid)
        {
            // This function determines which of the validLimit booleans needs to be set when a calling function needs to track if the limits entered are valid.

            if (isValid)
            {
                TextBoxHelper.SetError(limitTextBox, false);
                TextBoxHelper.SetSuccess(limitTextBox, true);
            }
            else
            {
                TextBoxHelper.SetSuccess(limitTextBox, false);
                TextBoxHelper.SetError(limitTextBox, true);
            }
        }

        private void SubmitTestStepButton_Click(object sender, RoutedEventArgs e)
        {
            Test test = (Test)TestSelectBox.SelectedItem;
            viewModel.AddTemporaryTestStep(StepTextBox.Text, MStepTextBox.Text, SubStepTextBox.Text, LowerLimitTextBox.Text, UpperLimitTextBox.Text, test);
            SetTestStepToDefault();
            StepTextBox.Focus();
        }

        public void SetTestStepToDefault()
        {
            StepTextBox.Text = "";
            TextBoxHelper.SetError(StepTextBox, true);
            TextBoxHelper.SetSuccess(StepTextBox, false);

            MStepTextBox.Text = "";
            TextBoxHelper.SetError(MStepTextBox, true);
            TextBoxHelper.SetSuccess(MStepTextBox, false);

            SubStepTextBox.Text = "";
            TextBoxHelper.SetError(SubStepTextBox, false);
            TextBoxHelper.SetSuccess(SubStepTextBox, true);

            LowerLimitTextBox.Text = "";
            TextBoxHelper.SetError(LowerLimitTextBox, false);
            TextBoxHelper.SetSuccess(LowerLimitTextBox, true);

            UpperLimitTextBox.Text = "";
            TextBoxHelper.SetError(UpperLimitTextBox, false);
            TextBoxHelper.SetSuccess(UpperLimitTextBox, true);

            editedTestStep = null;
            if (selectedDataGrid != null)
            {
                selectedDataGrid.SelectedItem = null;
                selectedDataGrid = null;
            }

            SubmitTestStepButton.Visibility = Visibility.Visible;
            EditTestStepButton.Visibility = Visibility.Hidden;
            ComboBoxHelper.SetSafety(TestSelectBox, false);

            if (editedRow != null)
            {
                DataGridHelper.SetSafety(editedRow, false);
                editedRow = null;
            }
        }

        private void TestStepInput_KeyUp(object sender, KeyEventArgs e)
        {
            // This presses the submit button for the row of test step inputs that is currently being entered.
            if (e.Key == Key.Enter)
            {
                if (SubmitTestStepButton.Visibility == Visibility.Visible)
                {
                    AutomationUtil.PressButton(SubmitTestStepButton);
                }
                else if (EditTestStepButton.Visibility == Visibility.Visible)
                {
                    AutomationUtil.PressButton(EditTestStepButton);
                }
            }
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


        private void DataGridScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            // This will keep the header datagrid algined with the datagrids that are stored in the scrollviewer.
            ScrollViewer scrollViewer = (ScrollViewer)sender;
            HeaderGridSpacer.Visibility = scrollViewer.ComputedVerticalScrollBarVisibility;
        }



        private void TestSelectBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox testComboBox = (ComboBox)sender;
            CheckSubmitTestStepButtonStatus();
        }

        private void CheckSubmitTestStepButtonStatus()
        {
            bool testSelected = TestSelectBox.SelectedItem != null;
            bool stepEntered = TextBoxHelper.GetSuccess(StepTextBox);
            bool mStepEntered = TextBoxHelper.GetSuccess(MStepTextBox);
            bool subStepEntered = TextBoxHelper.GetSuccess(SubStepTextBox);
            bool lowerLimitEntered = TextBoxHelper.GetSuccess(LowerLimitTextBox);
            bool upperLimitEntered = TextBoxHelper.GetSuccess(UpperLimitTextBox);
            bool canBeEnabled = testSelected && stepEntered && mStepEntered && subStepEntered && lowerLimitEntered && upperLimitEntered;
            SubmitTestStepButton.IsEnabled = canBeEnabled;
            EditTestStepButton.IsEnabled = canBeEnabled;
        }

        private void ControlFileNumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int dummy;
            if (!Int32.TryParse(e.Text, out dummy)) e.Handled = true;
        }

        private void ControlFileNumberTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            TextValidator.ValidatePastedInt(e);
        }

        private void ControlFileTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetMandatoryTextBoxState((TextBox)sender);
            CheckCreateTemplateButtonState();
        }

        private void ControlFileTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!DatabaseHelper.ValidateCharacters(e.Text))
            {
                e.Handled = true;
            }
        }

        private void ControlFileTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            TextValidator.ValidatePastedText(e);
        }

        private void CheckCreateTemplateButtonState()
        {
            bool productSelected = TestStepProductSelectBox.SelectedItem != null;
            bool validNameEntered = TextBoxHelper.GetSuccess(ControlFileNumberTextBox);
            bool validTypeEntered = TextBoxHelper.GetSuccess(ControlFileTypeTextBox);
            bool validPartEntered = TextBoxHelper.GetSuccess(ControlFilePartTextBox);
            bool validVersionEntered = TextBoxHelper.GetSuccess(ControlFileVersionTextBox);
            CreateTemplateButton.IsEnabled = productSelected && validNameEntered && validTypeEntered && validPartEntered && validVersionEntered;
            UpdateTemplateButton.IsEnabled = CreateTemplateButton.IsEnabled;
        }

        private void CreateTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            Product product = (Product)TestStepProductSelectBox.SelectedItem;
            if (product == null) return;

            string number = ControlFileNumberTextBox.Text;
            string type = ControlFileTypeTextBox.Text;
            string part = ControlFilePartTextBox.Text;
            string version = ControlFileVersionTextBox.Text;
            ControlFile controlFile = viewModel.controlFileUtil.AddControlFile(number, type, part, version, product);

            if (controlFile != null)
            {
                viewModel.AddAllTestSteps(controlFile);
                viewModel.ControlFileView.Add(controlFile);
                SetTemplateToDefault();
                SetTestStepToDefault();
                CheckCreateTemplateButtonState();
            }
        }

        private void SetTemplateToDefault()
        {
            ControlFileNumberTextBox.Text = "";
            ControlFileTypeTextBox.Text = "";
            ControlFilePartTextBox.Text = "";
            ControlFileVersionTextBox.Text = "";

            TextBoxHelper.SetError(ControlFileNumberTextBox, true);
            TextBoxHelper.SetError(ControlFileTypeTextBox, true);
            TextBoxHelper.SetError(ControlFilePartTextBox, true);
            TextBoxHelper.SetError(ControlFileVersionTextBox, true);

            TextBoxHelper.SetSuccess(ControlFileNumberTextBox, false);
            TextBoxHelper.SetSuccess(ControlFileTypeTextBox, false);
            TextBoxHelper.SetSuccess(ControlFilePartTextBox, false);
            TextBoxHelper.SetSuccess(ControlFileVersionTextBox, false);

            ControlFileSelectBox.SelectedItem = null;
            viewModel.ClearTestList();

            if (TestStepProductSelectBox.SelectedItem != null)
            {
                Product product = (Product)TestStepProductSelectBox.SelectedItem;
                viewModel.BuildViewedTestList(product);
            }
            
            viewModel.RecordTemplateChangeStop();
            CheckControlFileBoxStatus();
            ComboBoxHelper.SetSafety(ControlFileSelectBox, false);
        }



        private void ControlFileSelectBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (e.AddedItems.Count != 0 && TestStepProductSelectBox.SelectedItem != null)
            {
                ControlFile controlFile = (ControlFile)e.AddedItems[0];
                Product product = (Product)TestStepProductSelectBox.SelectedItem;

                SetControlFileInformation(controlFile);

                controlFile.BuildTestStepList();
                viewModel.BuildViewedTestList(product);
                viewModel.AssignTestStepsToTests(controlFile);

                UpdateTemplateButton.Visibility = Visibility.Visible;
                CreateTemplateButton.Visibility = Visibility.Hidden;
                viewModel.RecordTemplateChangesStart();

                CheckControlFileBoxStatus();
                ComboBoxHelper.SetSafety(ControlFileSelectBox, true);

                ControlFileNumberTextBox.Focus();
                ControlFileNumberTextBox.SelectAll();
            }
            else
            {
                UpdateTemplateButton.Visibility = Visibility.Hidden;
                CreateTemplateButton.Visibility = Visibility.Visible;
            }
        }

        private void SetControlFileInformation(ControlFile controlFile)
        {
            ControlFileNumberTextBox.Text = controlFile.Number.ToString();
            ControlFileTypeTextBox.Text = controlFile.Type;
            ControlFilePartTextBox.Text = controlFile.Part;
            ControlFileVersionTextBox.Text = controlFile.Version;

            TextBoxHelper.SetError(ControlFileNumberTextBox, false);
            TextBoxHelper.SetError(ControlFileTypeTextBox, false);
            TextBoxHelper.SetError(ControlFilePartTextBox, false);
            TextBoxHelper.SetError(ControlFileVersionTextBox, false);

            TextBoxHelper.SetSuccess(ControlFileNumberTextBox, true);
            TextBoxHelper.SetSuccess(ControlFileTypeTextBox, true);
            TextBoxHelper.SetSuccess(ControlFilePartTextBox, true);
            TextBoxHelper.SetSuccess(ControlFileVersionTextBox, true);
        }

        private void ClearTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            SetTestStepToDefault();
            SetTemplateToDefault();
            CheckCreateTemplateButtonState();
        }

        private void TestStepDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            if (selectedDataGrid != null && selectedDataGrid != dataGrid)
            {
                selectedDataGrid.SelectedItem = null;
            }
            selectedDataGrid = dataGrid;

        }

        private void EditTestStepMenuItem_Click(object sender, RoutedEventArgs e)
        {

            if (selectedDataGrid == null) return;
            if (editedRow != null) DataGridHelper.SetSafety(editedRow, false);
            SubmitTestStepButton.Visibility = Visibility.Hidden;
            EditTestStepButton.Visibility = Visibility.Visible;

            TestStep testStep = (TestStep)selectedDataGrid.SelectedItem;
            StepTextBox.Text = testStep.Step;
            MStepTextBox.Text = testStep.MStep;
            SubStepTextBox.Text = testStep.SubStep;
            LowerLimitTextBox.Text = testStep.LowerLimit.ToString();
            UpperLimitTextBox.Text = testStep.UpperLimit.ToString();
            editedTestStep = testStep;

            // This is a visual que to let the person know what test has a test step being edited.
            Test test = (Test)selectedDataGrid.Tag;
            TestSelectBox.SelectedItem = test;
            TestSelectBox.IsEnabled = false;

            editedRow = selectedRow;
            DataGridHelper.SetSafety(editedRow, true);
            ComboBoxHelper.SetSafety(TestSelectBox, true);

            StepTextBox.Focus();
            StepTextBox.SelectAll();
        }

        private void EditTestStepButton_Click(object sender, RoutedEventArgs e)
        {
            string step = StepTextBox.Text;
            string mStep = MStepTextBox.Text;
            string subStep = SubStepTextBox.Text;
            string lowerLimit = LowerLimitTextBox.Text;
            string upperLimit = UpperLimitTextBox.Text;

            viewModel.EditTemporaryTestStep(step, mStep, subStep, lowerLimit, upperLimit, editedTestStep);
            SetTestStepToDefault();
        }

        private void DeleteTestStepMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Test selectedTest = (Test)selectedDataGrid.Tag;
            TestStep testStep = (TestStep)selectedDataGrid.SelectedItem;

            viewModel.DeleteTestStepFromTest(selectedTest, testStep);
        }

        private void ClearTestStepButton_Click(object sender, RoutedEventArgs e)
        {
            SetTestStepToDefault();
        }

        private void UpdateTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            ControlFile controlFile = (ControlFile)ControlFileSelectBox.SelectedItem;
            string numberText = ControlFileNumberTextBox.Text;
            string type = ControlFileTypeTextBox.Text;
            string part = ControlFilePartTextBox.Text;
            string version = ControlFileVersionTextBox.Text;

            bool controlFileUpdated = viewModel.EditTemplate(controlFile, numberText, type, part, version);
            if (controlFileUpdated)
            {
                Product product = (Product)TestStepProductSelectBox.SelectedItem;
                viewModel.BuildViewedControlFileList(product);
                SetTestStepToDefault();
                SetTemplateToDefault();
            }
        }

        private void TestStepDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrids.Add((DataGrid)sender);
        }

        private void TestStepRow_Selected(object sender, RoutedEventArgs e)
        {
            selectedRow = (DataGridRow)sender;
        }

        private void TestStepDataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            if (e.Column.SortDirection == ListSortDirection.Descending) e.Column.SortDirection = ListSortDirection.Ascending;
            else e.Column.SortDirection = ListSortDirection.Descending;

            int selectedColumnIndex = TestStepDataGrid.Columns.IndexOf(e.Column);

            foreach (DataGrid dataGrid in dataGrids)
            {
                SortDataGrid(dataGrid, selectedColumnIndex, (ListSortDirection)e.Column.SortDirection);
            }
        }

        public static void SortDataGrid(DataGrid dataGrid, int columnIndex = 0, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            // This solution was found on stackoverflow.
            //https://stackoverflow.com/questions/16956251/sort-a-wpf-datagrid-programmatically


            var column = dataGrid.Columns[columnIndex];

            // Clear current sort descriptions
            dataGrid.Items.SortDescriptions.Clear();

            // Add the new sort description
            dataGrid.Items.SortDescriptions.Add(new SortDescription(column.SortMemberPath, sortDirection));

            // Apply sort
            foreach (var col in dataGrid.Columns)
            {
                col.SortDirection = null;
            }
            column.SortDirection = sortDirection;

            // Refresh items to display sort
            dataGrid.Items.Refresh();
        }

        private void PageContentGrid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && viewModel.ChangesAreRecording)
            {
                AutomationUtil.PressButton(ClearTemplateButton);
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            TestStepProductSelectBox.SelectedItem = null;
        }

        private void CheckControlFileBoxStatus()
        {
            // Comboboxes are styled to enable and disable themselves depending on if they have items or not.
            // In the case of the control file box, there are extra conditions that keep it enabled or disabled.
            // This function ensures that the combobox transitions between the different states correctly.

            if (viewModel.ChangesAreRecording)
            {
                ControlFileSelectBox.IsEnabled = false;
            }
            else if (ControlFileSelectBox.HasItems)
            {
                ControlFileSelectBox.IsEnabled = true;
            }
            else if (!ControlFileSelectBox.HasItems)
            {
                ControlFileSelectBox.IsEnabled = false;
            }
        }
    }
}

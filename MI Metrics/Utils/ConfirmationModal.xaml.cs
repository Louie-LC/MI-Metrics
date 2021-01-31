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
using System.Windows.Shapes;

namespace MI_Metrics
{
    /// <summary>
    /// Interaction logic for AcceptTestResultsModal.xaml
    /// </summary>
    public partial class ConfirmationModal : Window
    {
        public string ModalLabel { get => modalLabel; set => modalLabel = value; }
        private string modalLabel;

        public string Message { get => message; set => message = value; }
        private string message;

        public bool SubmitButtonPressed { get => submitButtonPressed; set => submitButtonPressed = value; }
        private bool submitButtonPressed;

        public ConfirmationModal(string label, string message)
        {
            InitializeComponent();
            this.DataContext = this;
            SubmitButtonPressed = false;

            ModalLabel = label;
            Message = message;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            SubmitButtonPressed = true;
            this.Close();
        }
    }
}

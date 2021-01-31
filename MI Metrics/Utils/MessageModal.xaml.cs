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
    public enum ModalType
    {
        Normal,
        Success,
        Warning,
        Error
    }

    /// <summary>
    /// Interaction logic for MessageModel.xaml
    /// </summary>
    public partial class MessageModal : Window
    {
        public string ModalLabel { get => modalLabel; set => modalLabel = value; }
        private string modalLabel;

        public string Message { get => message; set => message = value; }
        private string message;

        public ModalType Type { get => type; set => type = value; }
        private ModalType type;
        

        public MessageModal(string label, string message, ModalType type)
        {
            InitializeComponent();
            this.DataContext = this;
            ModalLabel = label;
            Message = message;
            Type = type;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

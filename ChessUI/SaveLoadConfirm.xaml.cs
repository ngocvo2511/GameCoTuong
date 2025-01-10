using System.Windows;
using System.Windows.Controls;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for SaveLoadConfirm.xaml
    /// </summary>
    public partial class SaveLoadConfirm : UserControl
    {
        public event Action<bool> result;
        public SaveLoadConfirm()
        {
            InitializeComponent();
        }
        public void SetMessage(string message)
        {
            Message.Text = message;
        }
        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            result?.Invoke(true);
        }
        private void NoButton_Click(Object sender, RoutedEventArgs e)
        {
            result?.Invoke(false);
        }
    }
}

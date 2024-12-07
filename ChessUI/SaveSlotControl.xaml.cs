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
using System.Collections.ObjectModel;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for SaveSlotControl.xaml
    /// </summary>
    public partial class SaveSlotControl : UserControl
    {
        public ObservableCollection<string> SaveSlots { get; set; } = new ObservableCollection<string>();

        public SaveSlotControl()
        {
            InitializeComponent();
            for (int i = 1; i <= 5; i++)
            {
                SaveSlots.Add($"Empty Slot {i}");
            }

            SaveSlotList.ItemsSource = SaveSlots;
        }
        public event Action<int> SaveSlotSelected;

        private void Slot_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Grid grid && grid.DataContext is string slot)
            {
                int index = SaveSlots.IndexOf(slot);
                SaveSlotSelected?.Invoke(index);
            }
        }
    }
}


using System;
using System.IO;
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
using ChessUI.Menus;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for SaveSlotControl.xaml
    /// </summary>
    public partial class SaveSlotControl : UserControl
    {
        public ObservableCollection<string> SaveSlots { get; set; } = new ObservableCollection<string>();
        private readonly string SaveDirectory;
        public SaveSlotControl(bool isSave)
        {
            InitializeComponent();
            if (isSave == true) title.Text = "LƯU";
            else title.Text = "TẢI";
            string projectRoot = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\");
            SaveDirectory = System.IO.Path.Combine(projectRoot, "SavedData");
            if(!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }
            LoadFileToList();
            SaveSlotList.ItemsSource = SaveSlots;
        }
        private void LoadFileToList()
        {
            SaveSlots.Clear();
            var saveFiles = Directory.GetFiles(SaveDirectory, "*.xqi");

            for (int i = 0; i < 5; i++)
            {
                if (i < saveFiles.Length)
                {
                    var fileName = System.IO.Path.GetFileNameWithoutExtension(saveFiles[i]);
                    var lastWriteTime = File.GetLastWriteTime(saveFiles[i]);
                    SaveSlots.Add($"{lastWriteTime:ddd, dd/MM/yyyy HH:mm:ss}");
                }
                else
                {
                    SaveSlots.Add($"Empty Slot");
                }
            }
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
        public static readonly RoutedEvent BackButtonClickedEvent = EventManager.RegisterRoutedEvent(
            "BackButtonClicked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(SaveSlotControl)
        );
        public event RoutedEventHandler BackButtonClicked
        {
            add { AddHandler(BackButtonClickedEvent, value); }
            remove { RemoveHandler(BackButtonClickedEvent, value); }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(BackButtonClickedEvent));
        }
    }
}


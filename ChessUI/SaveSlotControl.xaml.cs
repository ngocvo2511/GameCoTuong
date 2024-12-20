﻿using System;
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
using ChessLogic;
using ChessLogic.GameStates.GameState;
using System.Security.Policy;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for SaveSlotControl.xaml
    /// </summary>
    public partial class SaveSlotControl : UserControl
    {
        public ObservableCollection<string> SaveSlots { get; set; } = new ObservableCollection<string>();
        private readonly string[] SaveFiles = new string[5]
        {
            "save1.xqi",
            "save2.xqi",
            "save3.xqi",
            "save4.xqi",
            "save5.xqi"
        };
        private readonly string SaveDirectory;
        private bool isSave;
        private GameState currentGameState;
        public SaveSlotControl(GameState gameState)
        {
            InitializeComponent();
            this.currentGameState = gameState;
            this.isSave = true;
            title.Text = "LƯU";
            string projectRoot = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\");
            SaveDirectory = System.IO.Path.Combine(projectRoot, "SaveData");
            if(!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }
            LoadFileToList();
            SaveSlotList.ItemsSource = SaveSlots;
        }
        public SaveSlotControl()
        {
            InitializeComponent();
            this.isSave = false;
            title.Text = "TẢI";
            string projectRoot = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\");
            SaveDirectory = System.IO.Path.Combine(projectRoot, "SaveData");
            if (!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }
            LoadFileToList();
            SaveSlotList.ItemsSource = SaveSlots;
        }
        private void LoadFileToList()
        {
            SaveSlots.Clear();

            for (int i = 0; i < SaveFiles.Length; i++)
            {
                string filePath = System.IO.Path.Combine(SaveDirectory, SaveFiles[i]);

                if (File.Exists(filePath))
                {
                    DateTime lastWriteTime = File.GetLastWriteTime(filePath);
                    SaveSlots.Add($"{lastWriteTime:ddd, dd/MM/yyyy HH:mm:ss}");
                }
                else
                {
                    SaveSlots.Add("Trống");
                }
            }
        }

        public static readonly RoutedEvent CloseButtonClickedEvent = EventManager.RegisterRoutedEvent(
            "CloseButtonClicked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(SaveSlotControl)
        );
        public event RoutedEventHandler CloseButtonClicked
        {
            add { AddHandler(CloseButtonClickedEvent, value); }
            remove { RemoveHandler(CloseButtonClickedEvent, value); }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CloseButtonClickedEvent));
        }
        public static readonly RoutedEvent SelectedLoadSlotEvent = EventManager.RegisterRoutedEvent(
            "SelectedLoadSlot",
            RoutingStrategy.Bubble,
            typeof(EventHandler<SaveSlotEventArgs>),
            typeof(SaveSlotControl)
        );
        public event EventHandler<SaveSlotEventArgs> SelectedLoadSlot
        {
            add { AddHandler(SelectedLoadSlotEvent, value); }
            remove { RemoveHandler(SelectedLoadSlotEvent, value); }
        }

        private void SaveSlotList_MouseUp(object sender, MouseButtonEventArgs e)
        {
            int index = SaveSlotList.SelectedIndex;
            if (index < 0 || index > SaveFiles.Count()) return;
            string filePath = System.IO.Path.Combine(SaveDirectory, SaveFiles[index]);
            if (isSave == true)
            {
                if (File.Exists(filePath))
                {
                    ShowConfirmationDialog("Bạn có muốn ghi đè trận đấu trước đó?", result =>
                    {
                        if (result)
                        {
                            SaveService.Save(currentGameState, filePath);
                            LoadFileToList();
                        }
                    });
                }
                else
                {
                    SaveService.Save(currentGameState, filePath);
                    LoadFileToList();
                }
            }
            else
            {
                if (File.Exists(filePath))
                {
                    ShowConfirmationDialog("Bạn có muốn tải trận đấu này?", result =>
                    {
                        if (result)
                        {
                            RaiseEvent(new SaveSlotEventArgs(SelectedLoadSlotEvent, filePath));
                        }
                    });                                     
                }
            }
        }
        private void ShowConfirmationDialog(string message, Action<bool> callback)
        {
            ConfirmationControl.SetMessage(message);
            ConfirmationControl.result += (result) =>
            {
                ConfirmationDialogContainer.Visibility = Visibility.Collapsed;
                callback(result);
            };
            ConfirmationDialogContainer.Visibility = Visibility.Visible;
        }
    }
    public class SaveSlotEventArgs : RoutedEventArgs
    {
        public string FilePath { get; }

        public SaveSlotEventArgs(RoutedEvent routedEvent, string filePath) : base(routedEvent)
        {
            FilePath = filePath;
        }
    }   
}


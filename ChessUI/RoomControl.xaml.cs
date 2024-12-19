using ChessLogic;
using Microsoft.AspNetCore.SignalR.Client;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace ChessUI
{
    public partial class RoomControl : UserControl
    {
        

        public RoomControl()
        {
            InitializeComponent();
            
        }

        



        
        
        public static readonly RoutedEvent CreateRoomButtonClickedEvent = EventManager.RegisterRoutedEvent(
            "CreateRoomButtonClicked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(RoomControl)
        );

        public event RoutedEventHandler CreateRoomButtonClicked
        {
            add { AddHandler(CreateRoomButtonClickedEvent, value); }
            remove { RemoveHandler(CreateRoomButtonClickedEvent, value); }
        }

        private void CreateRoomButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CreateRoomButtonClickedEvent));
        }

        public static readonly RoutedEvent JoinRoomButtonClickedEvent = EventManager.RegisterRoutedEvent(
            "JoinRoomButtonClicked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(RoomControl)
        );

        public event RoutedEventHandler JoinRoomButtonClicked
        {
            add { AddHandler(JoinRoomButtonClickedEvent, value); }
            remove { RemoveHandler(JoinRoomButtonClickedEvent, value); }
        }

        private void JoinRoomButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(JoinRoomButtonClickedEvent));
        }

        public static readonly RoutedEvent RandomMatchButtonClickedEvent = EventManager.RegisterRoutedEvent(
            "RandomMatchButtonClicked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(RoomControl)
        );

        public event RoutedEventHandler RandomMatchButtonClicked
        {
            add { AddHandler(RandomMatchButtonClickedEvent, value); }
            remove { RemoveHandler(RandomMatchButtonClickedEvent, value); }
        }

        private void RandomMatchButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(RandomMatchButtonClickedEvent));
        }


        public static readonly RoutedEvent BackButtonClickedEvent = EventManager.RegisterRoutedEvent(
            "BackButtonClicked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(RoomControl)
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

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

namespace ChessUI.Menus
{
    /// <summary>
    /// Interaction logic for SettingsMenu.xaml
    /// </summary>
    public partial class SettingsMenu : UserControl
    {
        public SettingsMenu()
        {
            InitializeComponent();
        }

        public static readonly RoutedEvent BackButtonClickedEvent = EventManager.RegisterRoutedEvent(
            "BackButtonClicked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(SettingsMenu)
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

        public static readonly RoutedEvent RedCheckedEvent = EventManager.RegisterRoutedEvent(
            "RedChecked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(SettingsMenu)
        );

        public event RoutedEventHandler RedChecked
        {
            add { AddHandler(RedCheckedEvent, value); }
            remove { RemoveHandler(RedCheckedEvent, value); }
        }

        private void Red_Checked(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(RedCheckedEvent));
        }

        public static readonly RoutedEvent BlackCheckedEvent = EventManager.RegisterRoutedEvent(
            "BlackChecked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(SettingsMenu)
        );

        public event RoutedEventHandler BlackChecked
        {
            add { AddHandler(BlackCheckedEvent, value); }
            remove { RemoveHandler(BlackCheckedEvent, value); }
        }

        private void Black_Checked(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(BlackCheckedEvent));
        }


        public static readonly RoutedEvent VolumeSliderValueChangedEvent = EventManager.RegisterRoutedEvent(
            "VolumeSliderValueChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(SettingsMenu)
        );

        public event RoutedEventHandler VolumeSliderValueChanged
        {
            add { AddHandler(VolumeSliderValueChangedEvent, value); }
            remove { RemoveHandler(VolumeSliderValueChangedEvent, value); }
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedEventArgs e)
        {
            if (VolumeValue != null)
                VolumeValue.Text = VolumeSlider.Value.ToString();
            RaiseEvent(new RoutedEventArgs(VolumeSliderValueChangedEvent));
        }

    }
}

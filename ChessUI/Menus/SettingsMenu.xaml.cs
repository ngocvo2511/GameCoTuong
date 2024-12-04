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

        public static readonly RoutedEvent humanFirstCheckedEvent = EventManager.RegisterRoutedEvent(
            "humanFirstChecked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(SettingsMenu)
        );

        public event RoutedEventHandler humanFirstChecked
        {
            add { AddHandler(humanFirstCheckedEvent, value); }
            remove { RemoveHandler(humanFirstCheckedEvent, value); }
        }

        private void humanFirst_Checked(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(humanFirstCheckedEvent));
        }

        public static readonly RoutedEvent botFirstCheckedEvent = EventManager.RegisterRoutedEvent(
            "botFirstChecked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(SettingsMenu)
        );

        public event RoutedEventHandler botFirstChecked
        {
            add { AddHandler(botFirstCheckedEvent, value); }
            remove { RemoveHandler(botFirstCheckedEvent, value); }
        }

        private void botFirst_Checked(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(botFirstCheckedEvent));
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

        public static readonly RoutedEvent isTimeLimitCheckedEvent = EventManager.RegisterRoutedEvent(
            "isTimeLimitChecked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(SettingsMenu)
        );

        public event RoutedEventHandler isTimeLimitChecked
        {
            add { AddHandler(isTimeLimitCheckedEvent, value); }
            remove { RemoveHandler(isTimeLimitCheckedEvent, value); }
        }

        private void isTimeLimit_Checked(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(isTimeLimitCheckedEvent));
        }
        public static readonly RoutedEvent isTimeLimitUncheckedEvent = EventManager.RegisterRoutedEvent(
            "isTimeLimitUnchecked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(SettingsMenu)
        );

        public event RoutedEventHandler isTimeLimitUnchecked
        {
            add { AddHandler(isTimeLimitUncheckedEvent, value); }
            remove { RemoveHandler(isTimeLimitUncheckedEvent, value); }
        }

        private void isTimeLimit_Unchecked(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(isTimeLimitUncheckedEvent));
        }

        public static readonly RoutedEvent TimeLimitTextBoxChangedEvent = EventManager.RegisterRoutedEvent(
            "TimeLimitTextBoxChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(SettingsMenu)
            );

        public event RoutedEventHandler TimeLimitTextBoxChanged
        {
            add { AddHandler(TimeLimitTextBoxChangedEvent, value); }
            remove { RemoveHandler(TimeLimitTextBoxChangedEvent, value); }
        }

        private void TimeLimitTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            RaiseEvent(new RoutedEventArgs(TimeLimitTextBoxChangedEvent));
        }
    }
}

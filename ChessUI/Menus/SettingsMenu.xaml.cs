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
        public SettingsModel settings = new SettingsModel();
        public event Action<SettingsModel> SettingsChanged;
        private bool isInitializing = true;


        public SettingsMenu(SettingsModel settings)
        {
            InitializeComponent();
            isInitializing = true; // Bắt đầu khởi tạo

            this.settings = settings;
            VolumeSlider.Value = settings.Volume;
            VolumeValue.Text = settings.Volume.ToString();

            humanFirst.IsChecked = settings.HumanFirst;
            botFirst.IsChecked = !settings.HumanFirst;

            isTimeLimit.IsChecked = settings.IsTimeLimit;
            TimeLimitTextBox.Text = settings.TimeLimit.ToString();

            isInitializing = false; // Kết thúc khởi tạo

        }

        public static readonly RoutedEvent CloseButtonClickedEvent = EventManager.RegisterRoutedEvent(
            "CloseButtonClicked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(SettingsMenu)
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
            if (isInitializing) return;
            Sound.PlayButtonClickSound();

            settings.HumanFirst = true;
            SettingsChanged?.Invoke(settings);

            //RaiseEvent(new RoutedEventArgs(humanFirstCheckedEvent));
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
            if (isInitializing) return;
            Sound.PlayButtonClickSound();

            settings.HumanFirst = false;
            SettingsChanged?.Invoke(settings);

            //RaiseEvent(new RoutedEventArgs(botFirstCheckedEvent));
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
            if (isInitializing) return;
            if (double.TryParse(VolumeValue.Text, out double volume))
            {
                settings.Volume = (int)VolumeSlider.Value;
                VolumeValue.Text = settings.Volume.ToString();
                SettingsChanged?.Invoke(settings);
            }

            //RaiseEvent(new RoutedEventArgs(VolumeSliderValueChangedEvent));
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
            if (isInitializing) return;
            Sound.PlayButtonClickSound();

            settings.IsTimeLimit = true;
            TimeLimitTextBox.IsEnabled = true;
            SettingsChanged?.Invoke(settings);

            //RaiseEvent(new RoutedEventArgs(isTimeLimitCheckedEvent));
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
            if (isInitializing) return;
            Sound.PlayButtonClickSound();

            settings.IsTimeLimit = false;
            TimeLimitTextBox.IsEnabled = false;
            SettingsChanged?.Invoke(settings);

            //RaiseEvent(new RoutedEventArgs(isTimeLimitUncheckedEvent));
        }

        

        private void TimeLimitTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            int time;
            if (!int.TryParse(TimeLimitTextBox.Text, out time))
            {
                return;
                //TimeLimitTextBox.Text = settings.TimeLimit.ToString();
                //RaiseEvent(new RoutedEventArgs(TimeLimitTextBoxChangedEvent));
            }
            else
            {
                TimeLimitTextBox.Text = time.ToString();
                settings.TimeLimit = time;
                SettingsChanged?.Invoke(settings);
            }
        }
    }
}

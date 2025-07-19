using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;
using System.Linq;
using WinDurango.UI.Settings;


namespace WinDurango.UI.Pages.Settings
{
    public sealed partial class UiSettings : Page
    {
        public UiSettings()
        {
            this.InitializeComponent();

            ComboBoxItem psbSelected = PatchSourceButton.Items
                .OfType<ComboBoxItem>()
                .FirstOrDefault(item => item.Tag.ToString() == App.Settings.Settings.DownloadSource.ToString());
            if (psbSelected != null && (ComboBoxItem)PatchSourceButton.SelectedItem != psbSelected)
            {
                PatchSourceButton.SelectedItem = psbSelected;
            }

            ComboBoxItem themeSelected = backdropComboBox.Items
                .OfType<ComboBoxItem>()
                .FirstOrDefault(item => item.Tag.ToString() == App.Settings.Settings.Backdrop.ToString());
            if (themeSelected != null && (ComboBoxItem)backdropComboBox.SelectedItem != themeSelected)
            {
                backdropComboBox.SelectedItem = themeSelected;
            }

            HorizontalScrollingToggle.IsOn = App.Settings.Settings.AppViewIsHorizontalScrolling;
        }

        private void OnBackdropSelected(object sender, RoutedEventArgs e)
        {
            if (backdropComboBox.SelectedItem is not ComboBoxItem sel) return;
            if (!Enum.TryParse(sel.Tag.ToString(), out UiConfigData.BackdropType backdrop)) return;
            if (App.Settings.Settings.Backdrop == backdrop) return;

            App.Settings.Set("Backdrop", backdrop);
        }

        private async void OnSourceSelected(object sender, RoutedEventArgs e)
        {
            if (PatchSourceButton.SelectedItem is not ComboBoxItem sel)
            {
                return;
            }

            if (!Enum.TryParse(sel.Tag.ToString(), out UiConfigData.PatchSource source))
            {
                return;
            }

            if (App.Settings.Settings.DownloadSource == source)
            {
                return;
            }

            PatchSourceButton.SelectedItem = sel.Content;
            App.Settings.Set("DownloadSource", source);
        }

        private async void OnDebugLogToggled(object sender, RoutedEventArgs e)
        {
            App.Settings.Set("DebugLoggingEnabled", ((ToggleSwitch)sender).IsOn);
        }

        private void OpenAppData(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo(App.DataDir) { UseShellExecute = true });
        }

        public void OnToggleSetting(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleSwitch toggleSwitch && toggleSwitch.Tag is string settingName)
            {
                App.Settings.Set(settingName, toggleSwitch.IsOn);
            }
        }

        private void OnDebugLogToggleLoaded(object sender, RoutedEventArgs e)
        {
            if (!Debugger.IsAttached || ((ToggleSwitch)sender).IsEnabled)
            {
                return;
            }

            ((ToggleSwitch)sender).IsEnabled = false;
            ((ToggleSwitch)sender).IsOn = true;
            ((ToggleSwitch)sender).OnContent = "Enable debug logging (currently debugging)";
        }

        private void HorizontalScrollingToggle_Toggled(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleSwitch toggleSwitch)
            {
                App.Settings.Set("AppViewIsHorizontalScrolling", toggleSwitch.IsOn);
                App.MainWindow.AppsListPage.SwitchScrollDirection(toggleSwitch.IsOn);
            }
        }
    }
}

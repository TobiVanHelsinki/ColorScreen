using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ColorScreen
{
    public sealed partial class LegalDialog : ContentDialog
    {
        public LegalDialog()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Hide();
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _ = Launcher.LaunchUriAsync(new Uri((sender as HyperlinkButton).CommandParameter as string));
            }
            catch (Exception)
            {
            }
        }
    }
}

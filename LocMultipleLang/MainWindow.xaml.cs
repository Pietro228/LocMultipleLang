using Microsoft.UI.Xaml;
using System.Diagnostics;
using WinUI3Localizer;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LocMultipleLang
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Microsoft.UI.Xaml.Controls.Button;

            Localizer.Get().SetLanguage(btn.Content.ToString());

            Debug.WriteLine($"Text string: {Localizer.Get().GetLocalizedString("Settings_About")}");
        }
    }
}

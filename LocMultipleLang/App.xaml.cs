using Microsoft.UI.Xaml;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;
using WinUI3Localizer;
using Path = System.IO.Path;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LocMultipleLang
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private Window? _window;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            await Initialize();

            _window = new MainWindow();
            _window.Activate();
        }


        public static async Task Initialize()
        {
            // Initialize a "Strings" folder in the "LocalFolder" for the packaged app.
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFolder stringsFolder = await localFolder.CreateFolderAsync("Strings", CreationCollisionOption.OpenIfExists);

            // Create string resources file from app resources if doesn't exists.
            string resourceFileName = "Resources.resw";
            await CreateStringResourceFileIfNotExists(stringsFolder, "en-GB", resourceFileName);
            await CreateStringResourceFileIfNotExists(stringsFolder, "en-US", resourceFileName);
            await CreateStringResourceFileIfNotExists(stringsFolder, "cs-CZ", resourceFileName);

            // Build localizer
            ILocalizer localizer = await new LocalizerBuilder()
                .AddStringResourcesFolderForLanguageDictionaries(stringsFolder.Path)
                .SetOptions(options =>
                {
                    options.DefaultLanguage = "en-US";
                })
                .Build();
        }

        private static async Task CreateStringResourceFileIfNotExists(StorageFolder stringsFolder, string language, string resourceFileName)
        {
            try
            {
                StorageFolder languageFolder = await stringsFolder.CreateFolderAsync(
                    language,
                    CreationCollisionOption.ReplaceExisting);

                string resourceFilePath = Path.Combine(stringsFolder.Name, language, resourceFileName);
                StorageFile resourceFile = await LoadStringResourcesFileFromAppResource(resourceFilePath);
                _ = await resourceFile.CopyAsync(languageFolder);

                /*
                if (await languageFolder.TryGetItemAsync(resourceFileName) is null)
                {
                    string resourceFilePath = Path.Combine(stringsFolder.Name, language, resourceFileName);
                    StorageFile resourceFile = await LoadStringResourcesFileFromAppResource(resourceFilePath);
                    _ = await resourceFile.CopyAsync(languageFolder);
                }*/
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error creating string resource file for language '{language}': {ex.Message}");
            }
        }

        private static async Task<StorageFile> LoadStringResourcesFileFromAppResource(string filePath)
        {
            Uri resourcesFileUri = new($"ms-appx:///{filePath}");
            return await StorageFile.GetFileFromApplicationUriAsync(resourcesFileUri);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media.Imaging;
using Windows.ApplicationModel.DataTransfer;
using System.Threading.Tasks;
using Windows.Storage.Streams;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace AppMonsters
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : AppMonsters.Common.LayoutAwarePage
    {
        public AppMonster myAppMonster;

        public MainPage()
        {
            this.InitializeComponent();

            // Lead data and create the App Monster
            createAppMonsterAsync();

            // Register for sharing you AppMoster with the App Monster Hub App, if you include a challenge or mini game you can enable sharing later
            RegisterForSharing();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            MainContentFrame.Navigate(typeof(MonsterGamePage));
        }

        /// <summary>
        /// In this method we will create you App Monster
        /// </summary>
        private async void createAppMonsterAsync()
        {
            // First we will load the avatar file, please make sure that the image exists
            // This sample image is based on: OCAL, http://www.clker.com/clipart-26433.html, License: Public Domain
            var imageUri = new System.Uri("ms-appx:///Assets/AppMonsterTemplateAvatar.png");
            var file = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(imageUri);
            
            // Set the values for you own App Monster here. Check wether your App Monster complies with the rules

            // The name is restricted to 30 letter, the description to 300 letters. You should think about localizing the description.
            String AppMonsterName = "Template Monster ";
            String AppMonsterDefaultDescription = "This is the default description for the Template Monster. This is the default description for the Template Monster.";
            
            // Is your App Monster a paid App? Rare (paid) App Monster have 20 more skillpoints you can use. The maximum allowed prize is an equivalent of € 4.99
            bool paid = false;

            // Set the values for your App Monster here. You can use up to 40 points for a free and up to 60 for a paid App Monster
            int defense = 8;
            // This is the damage your monster can take in a fight
            int hitpoints = 20;
            // This is the healing power of you monster
            int heal = 3;

            // Define up to two attacks for your AppMonster
            AppMonsterAttack firstAttack = new AppMonsterAttack("Firepunch", 3);
            firstAttack.AddLocalizedName("en-US", "Firepunch");
            firstAttack.AddLocalizedName("de-DE", "Feuerhieb");

            AppMonsterAttack secondAttack = new AppMonsterAttack("Faststrike", 3);
            secondAttack.AddLocalizedName("en-US", "Faststrike");
            secondAttack.AddLocalizedName("de-DE", "Schnellschlag");

            // This where things get exciting. Here we try to bring you new App Monster to life. While doing this we check whether you App Monster complies with the rules. Feels like being Frankenstein, doesn't it?
            try
            {
                myAppMonster = new AppMonster(AppMonsterName, AppMonsterDefaultDescription, firstAttack, secondAttack, defense, hitpoints, heal, paid, file, imageUri);
            }
            catch
            {
                throw;
            }
            
            // You can add localized descriptions for your App Monster - this is optional. You still have to comply with the 300 letter limit. The translations in the example were created using http://translate.bing.com.
            myAppMonster.AddLocalizedDescription("en-US", "This is the english description for the Template Monster. This is the english description for the Template Monster.");
            myAppMonster.AddLocalizedDescription("de-DE", "Dies ist die deutsche Beschreibung für das Template Monster. Dies ist die deutsche Beschreibung für das Template Monster.");
         
            // Set your new AppMonster as the Datacontext for this page to display the App Monster Information
            this.DataContext = myAppMonster;
        }

        private void RegisterForSharing()
        {
            //The DataTransferManager is used for sharing your App Monster with the App Monster Hub App
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(shareAppMonster);
        }

        private void shareAppMonster(DataTransferManager sender, DataRequestedEventArgs e)
        {
            // Get the data request for sharing
            DataRequest request = e.Request;

            if (myAppMonster != null)
            {                
                DataRequestDeferral deferral = request.GetDeferral();
                try
                {

                    request.Data = myAppMonster.GetShareData();

                    // If you want to modify the default title, description or text for sharing from this page you can do it here
                    //request.Data.Properties.Title = "This is " + myAppMonster.Name + " - the lastest addition to my App Monster team";
                    //request.Data.Properties.Description = "Share " + myAppMonster.Name + " with the App Monsters Hub App";
                    //request.Data.SetText("I just downloaded a new App Monster from the Windows Store: " + myAppMonster.Name + "! Check it out yourself at http://apps.microsoft.com/windows/app/" + myAppMonster.AppId);
                }
                catch
                {
                    request.FailWithDisplayText("Your App Monster could not be shared.");
                }
                finally
                {
                    deferral.Complete();
                }
            }
            else
            {
                request.FailWithDisplayText("Your app monster has not been correctly initialized");
            }
        }

        /// <summary>
        /// Use if you would like to share from your app without opening the charms bar you can call this method from you user interface
        /// </summary>
        //private void shareProgrammatically()
        //{
        //    Windows.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI();
        //}

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }
    }
}

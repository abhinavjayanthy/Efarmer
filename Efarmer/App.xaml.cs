using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace Efarmer
{
    
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        public List<string> testid = new List<string>();
        public List<string> filteredtestid = new List<string>();
        public List<string> datetime = new List<string>();
        public List<string> Filtereddatetime = new List<string>();
        public int index =0 ;
        public bool check = true;
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            var conn = new SQLiteConnection(Class1.dbpath1);
            conn.CreateTable<analysis>();
            var query = conn.Table<analysis>().Where(x => x.datetime != null).OrderByDescending(x => x.datetime); //query is list<T>
            foreach (var v1 in query)
            {
                testid.Add(v1.testid);
                datetime.Add(v1.datetime);
               
                filteredtestid = testid.Distinct().ToList();
                Filtereddatetime = datetime.Distinct().ToList();

            }
            //timer
            DispatcherTimer timer = new DispatcherTimer();

            timer.Tick += tiler;
            timer.Interval = new TimeSpan(0, 0, 2);
            timer.Start(); 

           
        }

        private void tiler(object sender, object e)
        {
            XmlDocument tilexml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWideText09);
            XmlNodeList textfields = tilexml.GetElementsByTagName("text");

            //TileText
            if (check)
            {

                textfields[0].AppendChild(tilexml.CreateTextNode("E-Farmer"));
                check = false;
            }
            else
            {
                if (filteredtestid.Count != 0)
                {
                    textfields[0].AppendChild(tilexml.CreateTextNode(" Recent test's:"));
                    textfields[1].AppendChild(tilexml.CreateTextNode(filteredtestid[index]+" On " + Filtereddatetime[index]));
                    index++;
                    if (index == filteredtestid.Count)
                    {
                        index = 0;
                        check = true;
                    }
                }
                else
                {
                    textfields[0].AppendChild(tilexml.CreateTextNode("E-Farmer"));
                    textfields[1].AppendChild(tilexml.CreateTextNode("No Recent Test's"));
                    check = true;
                }
            }
      
            //UpdatingTile
            TileNotification tilenotificaton = new TileNotification(tilexml);

            TileUpdateManager.CreateTileUpdaterForApplication().Update(tilenotificaton);

        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), args.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}

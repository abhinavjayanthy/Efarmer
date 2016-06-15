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
using System.Diagnostics;
using SQLite;
using Windows.UI.Popups;
using Windows.Storage;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Efarmer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }

      void MainPage_Loaded(object sender, RoutedEventArgs e)
        {

            
       
                    
         //getting data from database:code start

                   SQLiteConnection conn = new SQLiteConnection(Class1.dbPath); //connecting to db(which exists already) which is in the path defined at "Class1.dbPath". Creates new one if does not exists

                   conn.CreateTable<userdata>(); //Creating table with zero rows if there is no table. Including this to avoid exception:"no table  named userdata exists"

                   var query = conn.Table<userdata>(); //.Where(x=>x.id != null).OrderByDescending(x =>x.id).Take(1);


                  // var result =  query.ToListAsync();

               
                        foreach (var item in query)
                        {
                            name_from_db.Text = "," + item.firstname;
                        }
                  
                    
         //getting data from database:code end

          }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private  void profile_button_Click(object sender, RoutedEventArgs e)
        {

          this.Frame.Navigate(typeof(BlankPage1));
           
        }


    /*  async  public void seedDatabase() 
        {

            //grabing sdb.db from package installed location to "seedfile"
            StorageFile seedfile = await StorageFile.GetFileFromPathAsync(Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path, "sdb.sqlite"));
            //placing sdb.db from seedfile to application data folder
            await seedfile.CopyAsync(Windows.Storage.ApplicationData.Current.LocalFolder);
        } */

      
        

        private async void test_button_Click(object sender, RoutedEventArgs e)
        {
            
            if (name_from_db.Text == "")
            {
                MessageDialog msg = new MessageDialog("You need to update profile atleast once to continue", "Oops!");
                await msg.ShowAsync();
            }
            else
            {
                this.Frame.Navigate(typeof(testmode));
            }
        }

        private void reports_button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(reportslist));
        }

        private void help_button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(help));
        }

        
       
      
    }

}

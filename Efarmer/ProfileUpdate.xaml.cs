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
using Windows.UI.Popups;
using SQLite;
using System.Diagnostics;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Efarmer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
   
    public sealed partial class BlankPage1 : Page
    {
        public BlankPage1()
        {
            this.InitializeComponent();
            Loaded += BlankPage1_Loaded;
          
        }

        async void BlankPage1_Loaded(object sender, RoutedEventArgs e)
        {
            

            //getting latest data from database:code start
    

            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(Class1.dbPath);

            var query = conn.Table<userdata>().Where(x => x.id != null).OrderByDescending(x => x.id).Take(1);

            var result = await query.ToListAsync();

            foreach (var item in result)
            {
                firstname_box.Text = item.firstname;
                lastname_box.Text = item.lastname;
                place_box.Text = item.place;
                zipcode_box.Text = item.zipcode;
                

               // longitude_box.Text = item.longitude;
               // latitude_box.Text = item.latitude;
            }
            //getting data from database: code end */

            if (firstname_box.Text == "Farmer")
            {
                firstname_box.Text = "";
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        public string trans_data;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            

        }

       

       async private void save_Click(object sender, RoutedEventArgs e)
        {

           MainPage mp = new MainPage();


            if (firstname_box.Text != "" && lastname_box.Text != "" && zipcode_box.Text !=""&&place_box.Text !="")
            {
                int i = 1;
                var conn = new SQLite.SQLiteConnection(Class1.dbPath); //creates db if does not exist
                                                                       //if db exists continues with that db
                conn.CreateTable<userdata>();//creates table if does not exists
                                             //if table exists continues with that table by adding new data with out deleting old data.
                conn.Insert(new userdata() { id = i, firstname = firstname_box.Text, lastname = lastname_box.Text,  zipcode = zipcode_box.Text,place = place_box.Text,dateandtime = DateTime.Now.ToString()});
                MessageDialog msg = new MessageDialog("Updated Successfully", "Success!");
                await msg.ShowAsync();
               // this.Frame.Navigate(typeof(MainPage));

            }
            else
            {
                MessageDialog msg = new MessageDialog( "Missed some fields,please enter them to update profile sucessfully", "Error");
                await msg.ShowAsync();
            }
        }

       private void GoBack(object sender, RoutedEventArgs e)
       {
           this.Frame.Navigate(typeof(MainPage));
       }

    }
    public class userdata
    {
        [AutoIncrement, PrimaryKey]
        public int id{ get; set; }
        [MaxLength(70)]
        public string firstname { get; set; }
        [MaxLength(70)]
        public string lastname { get; set; }
        //[MaxLength(100)]
       // public string adress { get; set; }
        //[MaxLength(50)]
        //public string district { get; set; }
        //[MaxLength(50)]
        //public string state { get; set; }
        [MaxLength(50)]
        public string place { get; set; }
        [MaxLength(30)]
        public string zipcode { get; set; }
       // [MaxLength(50)]
        //public string country { get; set; }
        //[MaxLength(20)]
        //public string latitude { get; set; }
       // [MaxLength(20)]
        //public string longitude { get; set; }
        [MaxLength(20)]
        public string dateandtime { get; set; }
    }
}

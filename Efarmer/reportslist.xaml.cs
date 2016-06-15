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
using SQLite;
using System.Diagnostics;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Efarmer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class reportslist : Page
    {
        public List<string> testid = new List<string>();
        public List<string> filteredtestid = new List<string>();

        public List<string> crops = new List<string>();

        public string selectedtestid;  //current selected test id
        public string selectedcrop; //current selected crop
        public reportslist()
        {
            this.InitializeComponent();
            Loaded += reportslist_Loaded;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
           
        }

        void reportslist_Loaded(object sender, RoutedEventArgs e)
        {
            var conn = new SQLiteConnection(Class1.dbpath1);
            conn.CreateTable<analysis>();
            var query = conn.Table<analysis>().Where(x => x.datetime != null).OrderByDescending(x => x.datetime); //query is list<T>
            foreach (var v1 in query)
            {
                testid.Add(v1.testid);

                filteredtestid = testid.Distinct().ToList();
            }

            reports_list.ItemsSource = filteredtestid;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void reports_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //for good UX
            selectedcrop = "";
            landcovered_block.Text = "";
            expectedyield_block.Text ="";
            n_db.Text = ""; 
            p_db.Text = "";
            k_db.Text = "";
            n_from_file.Text = "";
            p_from_file.Text = "";
            k_from_file.Text = "";
            location_block.Text = "";
            
            ///////////////////////////////////////
            actualyield_box.Text = "";
            n_used.SelectedIndex = -1;
            p_used.SelectedIndex = -1;
            k_used.SelectedIndex = -1;
            marketprice_box.Text = "";



            if (e.AddedItems.Count > 0)
            {
                var lbi = e.AddedItems[0] as string;
                if (null != lbi)
                {
                    selectedtestid = lbi;
                }
            }
            displaycrops();
            
        }


        public void displaycrops()
        {
            crops.Clear(); //clearing the list
            var con = new SQLiteConnection(Class1.dbpath1);
            var query = con.Table<analysis>();

            foreach (var v2 in query)
            {
                if (v2.testid == selectedtestid)
                {
                    crops.Add(v2.rc);
                    date_time_block.Text = v2.datetime;
                    soil_type_block.Text = v2.soil;
                    season_block.Text = v2.period;
                }
            }
            crops_list.ItemsSource = null; //refreshing the list
            crops_list.ItemsSource = crops; //binding to updated crops
        }

        private void crops_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var lbi = e.AddedItems[0] as string;
                if (null != lbi)
                {
                    selectedcrop = lbi;
                }
           }
            displaydata();
        }
        public void displaydata()
        {
            var con = new SQLiteConnection(Class1.dbpath1);
            var query = con.Table<analysis>();
            foreach (var v4 in query)
            {
                if (v4.testid == selectedtestid && v4.rc == selectedcrop)
                {
                    landcovered_block.Text = v4.landcov+"Hectares";
                    expectedyield_block.Text = v4.e_yield+"tons";

                    
                    n_db.Text = v4.sf_n; //soifertilityl 
                    p_db.Text = v4.sf_p;
                    k_db.Text = v4.sf_k;
                    n_from_file.Text = v4.rc_n; //required fertility
                    p_from_file.Text = v4.rc_p;
                    k_from_file.Text = v4.rc_k;
                    location_block.Text = v4.location;
                   
                    ///////////////////////////////////////
                    actualyield_box.Text = v4.actualyield;
                    n_used.SelectedIndex = v4.n_used;
                    p_used.SelectedIndex = v4.p_used;
                    k_used.SelectedIndex = v4.k_used;
                    marketprice_box.Text = v4.sold_price;

                }
            }
        }

        async private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (landcovered_block.Text != "") //checking weather crop is selected from list or not
            {
                //setting the value of "actualyield,n_used,p_used,k_used,sold_price" to user defined values
                var con = new SQLiteConnection(Class1.dbpath1);
                var query = con.Table<analysis>();

                foreach (var v5 in query)
                {
                    if (v5.testid == selectedtestid && v5.rc == selectedcrop)
                    {
                        //query to update 
                        con.Query<analysis>("UPDATE analysis SET actualyield='" + actualyield_box.Text + "',n_used='" + n_used.SelectedIndex + "',p_used='" + p_used.SelectedIndex + "',k_used='" + k_used.SelectedIndex + "',sold_price='" + marketprice_box.Text + "' Where testid='" + v5.testid + "' AND rc='" + v5.rc + "'");
                    }
                }

                makefile(); //funtion which write file to store in cloud

                MessageDialog msg = new MessageDialog("Saved Succesfully", "Done!");
                await msg.ShowAsync();
            }
            else
            {
                MessageDialog msg = new MessageDialog("Please select SavedCrop associated with named TestID to edit/make its report", "Error!");
                await msg.ShowAsync();
            }
        }

        public void makefile()
        {

        }

        
    }
}

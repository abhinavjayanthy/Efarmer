using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SQLite;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Efarmer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class hardware_overview : Page
    {
        public List<string> soiltype = new List<string>();
        public List<string> season = new List<string>();
        public hardware_overview()
        {
            this.InitializeComponent();

            soiltype.Add("Red soil");
            soiltype.Add("Lateritic soil");
            soiltype.Add("Black soil");
            soiltype.Add("Alluvial soil");
            soiltype.Add("Desert soil");
            soiltype.Add("Forest and Hill soils");
            soiltype.Add("Saline and Alkali soil");
            soiltype.Add("Acid soil");
            soiltype.Add("Peaty and Marshy soil");
            soiltype.Add("Sandy and loam soil");
            soiltype.Add("Sandy and loam soil - drained");
            soiltype.Add("Sandy and loam soil - not drained");
            soil_type_combo.ItemsSource = soiltype;

            season.Add("Summer");
            season.Add("Winter");
            season.Add("Rainy");

            season_box_combo.ItemsSource = soiltype;
            
            
           sensor(); // function for getting values from sesnsor


        }

       
            
        

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            to_hardware_overview to = (to_hardware_overview)e.Parameter;
            testname_box.Text = to.testname1;
            soil_type_combo.SelectedIndex = to.soiltype;
            land_covered_box.Text = to.landcover;
            season_box_combo.SelectedIndex = to.season;
            if (to.temp_c1 != "") { temp_to_db_block.Text = to.temp_c1; }
            else{temp_to_db_block.Text = "Unavailable";}
            if (to.humidity1 != "") { humidity_to_db_block.Text = to.humidity1; }
            else { humidity_to_db_block.Text = "Unavailable"; }
        }

        private void cont_overview_Click(object sender, RoutedEventArgs e)
        {
            var con = new SQLite.SQLiteConnection(Class1.dbPath);
            con.CreateTable<testdata>(); //creates table if does not exist. If table exist continues with existing table
            con.Insert(new testdata() { id = 442/*this values does not effect*/, testname = testname_box.Text, soiltype = soil_type_combo.SelectedItem.ToString(), landcovered = land_covered_box.Text, season = season_box_combo.SelectedItem.ToString(), temperature = temp_to_db_block.Text, humidity = humidity_to_db_block.Text, nitrogen = amount_n_to_db_block.Text, phosphorous = amount_p_to_db_block.Text, potassium = amount_k_to_db_block.Text, ph = ph_to_db_block.Text, moisture = moisture_to_db_block.Text, ec = ec_to_db_block.Text, mode = "sensor", datetime = DateTime.Now.ToString() });
            this.Frame.Navigate(typeof(recommendedcrops));
        }

        private void cont_overview_Copy_Click(object sender, RoutedEventArgs e)
        {
            var md = new MessageDialog("Are you sure you want to cancel the test?");
            md.Commands.Add(new UICommand("Yes", (UICommandInvokedHandler) =>
            {
                this.Frame.Navigate(typeof(MainPage));
            }));
            md.Commands.Add(new UICommand("No"));
            md.ShowAsync();
        }
        private void sensor()
        {
            //Dummy values for testing
            amount_n_to_db_block.Text = "200"; //string values from sensor
            amount_p_to_db_block.Text = "150";
            amount_k_to_db_block.Text = "98";
            ph_to_db_block.Text = "7";
            moisture_to_db_block.Text = "High";
            ec_to_db_block.Text = "2-4";
            //Dummy values for testing

        }
    }
}

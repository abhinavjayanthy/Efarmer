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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Efarmer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
   
    public sealed partial class test_overview : Page
    {
        public List<string> soiltype = new List<string>();
        public List<string> season = new List<string>();
        List<string> moisture = new List<string>();
        List<string> ph = new List<string>();
        List<string> ec = new List<string>();
        public test_overview()
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
            soil_type_combo_todb.ItemsSource = soiltype;

            season.Add("Summer");
            season.Add("Winter");
            season.Add("Rainy");
            season_combo_todb.ItemsSource = season;

            moisture.Add("High");
            moisture.Add("Medium");
            moisture.Add("Low");
            moisture_combo_todb.ItemsSource = moisture;

            ph.Add("1"); ph.Add("2"); ph.Add("3"); ph.Add("4"); ph.Add("5"); ph.Add("6"); ph.Add("7"); ph.Add("8"); ph.Add("9"); ph.Add("10"); ph.Add("11"); ph.Add("12"); ph.Add("13"); ph.Add("14");
            ph_combo_todb.ItemsSource = ph;

            ec.Add("lessthan 2");
            ec.Add("2-4");
            ec.Add("4-8");
            ec.Add("above 8");
            ec_combo_todb.ItemsSource = ec;


        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected  override void OnNavigatedTo(NavigationEventArgs e)
        {
            t_to_overview tf = (t_to_overview)e.Parameter;

            testname_to_db_block.Text = tf.testname1;
            soil_type_combo_todb.SelectedIndex = tf.soiltype1;   //
            landcovered_to_db_block.Text = tf.landcovered1;
            season_combo_todb.SelectedIndex = tf.season1;       //
            if (tf.temp_c1 == "")
            {temp_to_db_block.Text = "unavailable"; }
            else { temp_to_db_block.Text = tf.temp_c1; }
            if(tf.humidity1 =="")
            {humidity_to_db_block.Text = "unavailable";}
            else{ humidity_to_db_block.Text = tf.humidity1;}
            amount_n_to_db_block.Text = tf.amount_n;
            amount_p_to_db_block.Text = tf.amount_p;
            amount_k_to_db_block.Text = tf.amount_k;
            ph_combo_todb.SelectedIndex = tf.ph;               //
            moisture_combo_todb.SelectedIndex = tf.moisture;
            ec_combo_todb.SelectedIndex = tf.ec;              //

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

        private void cont_overview_Click(object sender, RoutedEventArgs e)
        {
            int i=1;
            var conn = new SQLite.SQLiteConnection(Class1.dbPath);
            conn.CreateTable<testdata>();
            conn.Insert(new testdata() { id = i, testname = testname_to_db_block.Text, soiltype = soil_type_combo_todb.SelectedItem.ToString(), landcovered = landcovered_to_db_block.Text, season = season_combo_todb.SelectedItem.ToString(), temperature = temp_to_db_block.Text, humidity = humidity_to_db_block.Text, nitrogen = amount_n_to_db_block.Text, phosphorous = amount_p_to_db_block.Text, potassium = amount_k_to_db_block.Text, ph = ph_combo_todb.SelectedItem.ToString(), moisture = moisture_combo_todb.SelectedItem.ToString(), ec = ec_combo_todb.SelectedItem.ToString(), mode = "manual", datetime = DateTime.Now.ToString() });
            this.Frame.Navigate(typeof(recommendedcrops));
        }

       
    }
    public class testdata
    {
        [AutoIncrement,PrimaryKey]
        public int id {get; set;}
        
        [MaxLength(10)]
        public string testname { get; set; }  
        [MaxLength(50)]
        public string soiltype { get; set; }
        [MaxLength(20)]
        public string landcovered { get; set; }
         [MaxLength(20)]
        public string season { get; set; }
         [MaxLength(10)]   
        public string temperature { get; set; }
         [MaxLength(10)]
        public string humidity { get; set;}
         [MaxLength(20)]
        public string nitrogen { get; set; }
         [MaxLength(20)]
        public string phosphorous { get; set;}
         [MaxLength(20)]
        public string potassium { get; set; }
         [MaxLength(10)]
        public string ph { get; set; }
         [MaxLength(10)] 
        public string moisture { get; set; }
         [MaxLength(10)]
        public string ec { get; set; }
         [MaxLength(50)]
         public string mode { get; set; }
         [MaxLength(20)]
        public string datetime { get; set; }        
    }
}

 
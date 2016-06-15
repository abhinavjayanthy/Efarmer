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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Efarmer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class soiltestOptions : Page
    {
        /*variables to store values from test_info_common*/
        public string testname; 
        public int soiltype;
        public string landcovered;
        public int season;
        public string temp_c;
        public string humidity;
        /*variables to store values from test_info_common*/

        List<string> moisture = new List<string>();
        List<string> ph = new List<string>();
        List<string> ec = new List<string>();
        public soiltestOptions()
        {
            this.InitializeComponent();
            moisture.Add("High");
            moisture.Add("Medium");
            moisture.Add("Low");
            moisture_box.ItemsSource = moisture;

            ph.Add("1"); ph.Add("2"); ph.Add("3"); ph.Add("4"); ph.Add("5"); ph.Add("6"); ph.Add("7"); ph.Add("8"); ph.Add("9"); ph.Add("10"); ph.Add("11"); ph.Add("12"); ph.Add("13"); ph.Add("14");
            ph_box.ItemsSource = ph;

            ec.Add("lessthan 2");
            ec.Add("2-4");
            ec.Add("4-8");
            ec.Add("above 8");
            ec_box.ItemsSource = ec;
            

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            t_from_testcommon tf = (t_from_testcommon)e.Parameter; //getting data from test_info_common
           
           testname = tf.testname;
           soiltype = tf.soiltype;
           landcovered = tf.landcovered;
           season = tf.season;
           temp_c = tf.temp_c;
           humidity = tf.humidity;

        }
        private async void cont_Click(object sender, RoutedEventArgs e)
        {
            if (n_amount_box.Text != "" && p_amount_box.Text != "" && k_amount_box.Text != "" && ph_box.SelectedIndex!= -1 && moisture_box.SelectedIndex!=-1 && ec_box.SelectedIndex != -1)
            {
                t_to_overview tf1 = new t_to_overview() { testname1 = testname, soiltype1 = soiltype, landcovered1 = landcovered, season1 = season, temp_c1 = temp_c, humidity1 = humidity, amount_n = n_amount_box.Text, amount_p = p_amount_box.Text, amount_k = k_amount_box.Text, ph = ph_box.SelectedIndex, moisture = moisture_box.SelectedIndex, ec = ec_box.SelectedIndex };
                this.Frame.Navigate(typeof(test_overview), tf1); //transfering data to test_overview
            }
            else
            {
                MessageDialog msg = new MessageDialog("Missed some fields,please enter them to continue", "Error");
                await msg.ShowAsync();
            }
        }

      

      

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var md = new MessageDialog("Are you sure you want to cancel the test?");
            md.Commands.Add(new UICommand("Yes", (UICommandInvokedHandler) =>
            {
              
                this.Frame.Navigate(typeof(MainPage));

            }));
            md.Commands.Add(new UICommand("No"));
            md.ShowAsync();
        }

        
 }
    public class t_to_overview
    {
        public string testname1 { get; set; }
        public int soiltype1 { get; set; }
        public string landcovered1 { get; set; }
        public int season1 { get; set; }
        public string temp_c1 { get; set; }
        public string humidity1 { get; set; }

        public string amount_n { get; set; }
        public string amount_p { get; set; }
        public string amount_k { get; set; }
        public int ph { get; set; }
        public int moisture { get; set; }
        public int ec { get; set; }
    }
}
 


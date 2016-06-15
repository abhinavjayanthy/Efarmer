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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Efarmer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class hardware_details2 : Page
    {
        public string testname;
        public string temp_c;
        public string humidity;

        public List<string> soiltype = new List<string>();
        public List<string> season = new List<string>();

        public hardware_details2()
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
            season_box.ItemsSource = season;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            to_hardware_overview2 tf2 = (to_hardware_overview2)e.Parameter;
            testname = tf2.testname;
            temp_c = tf2.temp_c;
            humidity = tf2.humidity;
        }

        async private void soil_test_commom_cont_button_Click(object sender, RoutedEventArgs e)
        {
            if(soil_type_combo.SelectedIndex!= -1&&land_covered_box.Text!=""&&season_box.SelectedIndex!= -1)
            {
                to_hardware_overview to = new to_hardware_overview() { testname1 = testname, temp_c1 = temp_c, humidity1 = humidity, soiltype = soil_type_combo.SelectedIndex, landcover = land_covered_box.Text, season = season_box.SelectedIndex };
                this.Frame.Navigate(typeof(hardware_overview),to);
            }
            else
            {
                MessageDialog msg = new MessageDialog("Missed some fields, please enter them to continue", "Error");
                await msg.ShowAsync();
            }
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
    }
    public class to_hardware_overview
    {
        public string testname1;
        public string temp_c1;
        public string humidity1;

        public int soiltype;
        public string landcover;
        public int season;
    }
}

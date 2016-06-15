using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;
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
    public sealed partial class hardware_details1 : Page
    {
        public  double lat;
        public  double longi;
        public  string zip;
        public  string place;
        public hardware_details1()
        {
            this.InitializeComponent();
            Loaded += hardware_details1_Loaded;
        }

        void hardware_details1_Loaded(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //getting data from database:code start
            SQLiteConnection conn = new SQLiteConnection(Class1.dbPath);
            var query = conn.Table<userdata>();
            foreach (var item in query)
            {
                zip = item.zipcode;
                place = item.place;
            }
            //getting data from database:code end 
            if (IsConnectedToInternet())
            {
               // Debug.WriteLine("get coordinate called");
                Getcoordinates(place, zip); //getting latitute and longitute values from google API


              //  string theURI = "http://free.worldweatheronline.com/feed/weather.ashx?q=" + lat + "," + longi + "&format=json&num_of_days=2&key=5e02e86375070423131001";
              //  GetjasonValues(theURI);

            }
            else
            {
                Internet_notifier.Text = "no data connection";
            }
            //}
            //catch
            //{
            //}
        }


        public bool IsConnectedToInternet()
        {
            ConnectionProfile connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            return (connectionProfile != null && connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess);
        }
        async private void Getcoordinates(string place, string postalcode)
        {
            var http = new HttpClient();
            http.MaxResponseContentBufferSize = Int32.MaxValue;
            var response = await http.GetStringAsync("https://maps.googleapis.com/maps/api/geocode/json?address=" + place + "-" + postalcode + "&sensor=false");
           // Debug.WriteLine(response);
            var rootobject = JsonConvert.DeserializeObject<Class3.RootObject>(response); //parsing jason data 
            foreach (var v2 in rootobject.results)
            {
                lat = v2.geometry.location.lat;
                longi = v2.geometry.location.lng;
            }
           
            GetjasonValues(lat.ToString(), longi.ToString());
        }
        public async void GetjasonValues(string latitude, string longitude)
        {
            var http = new HttpClient();
            http.MaxResponseContentBufferSize = Int32.MaxValue;
            string URI = "http://free.worldweatheronline.com/feed/weather.ashx?q=" + latitude + "," + longitude + "&format=json&num_of_days=2&key=5e02e86375070423131001";
            var response = await http.GetStringAsync(URI);
            Debug.WriteLine(response);
            var rootObject = JsonConvert.DeserializeObject<RootObject>(response); //parsing jason data 
            if (rootObject.data.current_condition != null)
            {
                foreach (var v1 in rootObject.data.current_condition)
                {
                    present_weat_block.Text = v1.temp_C + "°C";
                    humidity_block_txt_Copy.Text = v1.humidity + "%";
                    windspeed_block_txt_Copy.Text = v1.windspeedKmph + "Kmph";
                    visibility_block_txt_Copy.Text = v1.visibility + "Km";
                    cloud_cover_block_txt_Copy.Text = v1.cloudcover + "%";
                    pricip_block_txt_copy.Text = v1.precipMM + "mm";
                    pree_block_txt_copy.Text = v1.pressure + "miliBars";
                }
            }
            else
            {
                present_weat_block.Text = "";
                humidity_block_txt_Copy.Text = "";
                windspeed_block_txt_Copy.Text = "";
                visibility_block_txt_Copy.Text = "";
                cloud_cover_block_txt_Copy.Text = "";
                pricip_block_txt_copy.Text = ""; 
                pree_block_txt_copy.Text = "";

                MessageDialog msg = new MessageDialog("It's seems the PinCode you entered are wrong. You can check it in myprofile to include weather conditions in your analysis", "Oops!");
                await msg.ShowAsync();
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(testmode));
        }

        private async void soil_test_commom_cont_button_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
                SQLiteConnection conn = new SQLiteConnection(Class1.dbPath);
                conn.CreateTable<testdata>();
                var query = conn.Table<testdata>();
                string testname = "Ntej";
                foreach (var v5 in query)
                {
                    if (testname_box.Text == v5.testname)
                    {
                        testname = v5.testname;
                    }
                }
                if (testname == testname_box.Text)  //if
                {
                    MessageDialog msg = new MessageDialog("The TestID you typed already exists, please choose aother", "Sorry");
                    await msg.ShowAsync();
                }
                else
                {
                    //string theURI = "http://free.worldweatheronline.com/feed/weather.ashx?q=" + lat + "," + longi + "&format=json&num_of_days=2&key=5e02e86375070423131001";
                    if (testname_box.Text != "")
                    {
                        to_hardware_overview2 th2 = new to_hardware_overview2() { testname = testname_box.Text, temp_c = present_weat_block.Text, humidity = humidity_block_txt_Copy.Text };
                        if (Internet_notifier.Text != "")
                        {
                            present_weat_block.Text = "";
                            humidity_block_txt_Copy.Text = "";
                            windspeed_block_txt_Copy.Text = "";
                            visibility_block_txt_Copy.Text = "";
                            cloud_cover_block_txt_Copy.Text = "";
                            pricip_block_txt_copy.Text = "";
                            pree_block_txt_copy.Text = "";

                            var md = new MessageDialog("Are you sure you want to continue without weather condition results?");
                            md.Commands.Add(new UICommand("Yes", (UICommandInvokedHandler) =>
                            {
                                this.Frame.Navigate(typeof(hardware_details2),th2);
                            }));
                            md.Commands.Add(new UICommand("Recheck Internet Connection", (UICommandInvokedHandler) =>
                            {
                                if (IsConnectedToInternet())  //else --> if --> if --> if
                                {
                                    Internet_notifier.Text = "";
                                    Getcoordinates(place, zip); //calling this method to set lat , longi and location values and weather API
                                    //Debug.WriteLine(location);
                                    //GetjasonValues(theURI);
                                }
                                else
                                {
                                    present_weat_block.Text = "";
                                    humidity_block_txt_Copy.Text = "";
                                    windspeed_block_txt_Copy.Text = "";
                                    visibility_block_txt_Copy.Text = "";
                                    cloud_cover_block_txt_Copy.Text = "";
                                    pricip_block_txt_copy.Text = "";
                                    pree_block_txt_copy.Text = "";

                                }

                            }));
                            await md.ShowAsync();
                        }
                        else
                        {
                            this.Frame.Navigate(typeof(hardware_details2),th2);
                        }
                    }
                    else
                    {
                        MessageDialog msg = new MessageDialog("Missed some fields, please enter them to continue", "Error");
                        await msg.ShowAsync();
                    }

                }
           // }
          //  catch
           // {

           // }
        }
    }
    public class to_hardware_overview2
    {
        public string testname { get; set; }
        public string temp_c { get; set; }
        public string humidity { get; set; }
    }

}

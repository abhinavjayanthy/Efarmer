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
using Windows.Data.Json;
using System.Net.Http;
using Newtonsoft.Json;
using System.Diagnostics;
using Windows.Networking.Connectivity;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;
using SQLite;
//using Windows.Devices.Geolocation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Efarmer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class test_info_common : Page
    {
        public  double lat ;
        public  double longi;
        public  string zip;
        public  string place;
        public List<string> soiltype = new List<string>();
        public List<string> season = new List<string>();
       // public static string location;
        public test_info_common()
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


            Loaded += test_info_common_Loaded;
        }

       public void test_info_common_Loaded(object sender, RoutedEventArgs e)
        {
            //try
            //{
                //getting data from database:code start

            

                SQLiteConnection conn = new SQLiteConnection(Class1.dbPath);

                var query = conn.Table<userdata>(); //.Where(x => x.id != null).OrderByDescending(x => x.id).Take(1);



                foreach (var item in query)
                {
                    zip = item.zipcode;
                    place = item.place;
                }



                //getting data from database:code end 


                if (IsConnectedToInternet())
                {
                    

                    Getcoordinates(place, zip); //getting latitute and longitute values from google API and calling freeworldweather API

 
                    //GetjasonValues(lat.ToString(),longi.ToString());
                }
               /* if (IsConnectedToInternet())
                {
                    string theURI = "http://free.worldweatheronline.com/feed/weather.ashx?q=" + lat + "," + longi + "&format=json&num_of_days=2&key=5e02e86375070423131001";

                    Debug.WriteLine("before giving to jason lat:::{0}", lat);
                    Debug.WriteLine("before giving to jason langi:::{0}", longi);
                    GetjasonValues(theURI);
                }*/
                else
                {
                    Internet_notifier.Text = "no data connection";
                }

                //Debug.WriteLine(lat);
                //Debug.WriteLine(longi);

                //Debug.WriteLine(theURI);

                //Debug.WriteLine(IsConnectedToInternet());

            //}
            //catch
            //{
                //exception handling

            //}
            
        }
       
             
        

        public bool IsConnectedToInternet()
        {
            ConnectionProfile connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            return (connectionProfile != null && connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess);
        }

        async private void Getcoordinates(string place,string postalcode)
        {
            var http = new HttpClient();
            http.MaxResponseContentBufferSize = Int32.MaxValue;
            var response = await http.GetStringAsync("https://maps.googleapis.com/maps/api/geocode/json?address="+place+"-"+postalcode+"&sensor=false");
            
           Debug.WriteLine("GeoCodeResponse:{0}",response); 
           
           var rootobject = JsonConvert.DeserializeObject<Class3.RootObject>(response); //parsing jason data 
            foreach (var v2 in rootobject.results)
            {
                lat = v2.geometry.location.lat;

              Debug.WriteLine("lat value : {0}",lat);

                longi = v2.geometry.location.lng;

              Debug.WriteLine("long value : {0}", longi);
            }

            GetjasonValues(lat.ToString(), longi.ToString());


           
        }

        public async void GetjasonValues(string latitude, string longitude)
        {
           
            var http = new HttpClient();
            http.MaxResponseContentBufferSize = Int32.MaxValue;

            string URI = "http://free.worldweatheronline.com/feed/weather.ashx?q=" + latitude + "," + longitude + "&format=json&num_of_days=2&key=5e02e86375070423131001";

            var response = await http.GetStringAsync(URI);

            Debug.WriteLine("Weather API response{0}",response); //getting json data-checked

          var rootObject = JsonConvert.DeserializeObject<RootObject>(response); //parsing jason data 
if(rootObject.data.current_condition!=null)
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
                 
                  MessageDialog msg = new MessageDialog("It's seems the PinCode you entered are wrong. Correct it in update profile to include weather conditions in your analysis", "Oops!");
                  await msg.ShowAsync();
              }
        /*  foreach (var v2 in rootObject.data.weather)
          {
            var v3 = v2.weatherIconUrl;
            foreach (var v4 in v3)
            {
                if (v4.value != null)
                {
                    weather_con_image.Source = ImageFromRelativePath(this, v4.value);
                    Debug.WriteLine(v4.value);
                }
            } 
          }*/
         
         
      }
    
   
        //function to set image source

        /*public static BitmapImage ImageFromRelativePath(FrameworkElement parent, string path)
        {
            var uri = new Uri(parent.BaseUri, path);
            BitmapImage bmp = new BitmapImage();
            bmp.UriSource = uri;
            return bmp;
        }*/
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

      

        private async void soil_test_commom_cont_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                //checking weather test name already exists or not : start
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
                    MessageDialog msg = new MessageDialog("The testname you typed already exists, please choose aother", "Sorry");
                    await msg.ShowAsync();
                }
                //checking weather test name already exists or not : end
                else
                {


                    //string theURI = "http://free.worldweatheronline.com/feed/weather.ashx?q=" + lat + "," + longi + "&format=json&num_of_days=2&key=5e02e86375070423131001";

                    if (testname_box.Text != "" && soil_type_combo.SelectedIndex != -1 && land_covered_box.Text != "" && season_box.SelectedIndex != -1) //else --> if
                    {
                        t_from_testcommon tf = new t_from_testcommon() { testname = testname_box.Text, soiltype = soil_type_combo.SelectedIndex, landcovered = land_covered_box.Text, season = season_box.SelectedIndex, temp_c = present_weat_block.Text, humidity = humidity_block_txt_Copy.Text };
                        if (Internet_notifier.Text != "") //else --> if --> if
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
                                this.Frame.Navigate(typeof(soiltestOptions), tf);
                            }));
                            md.Commands.Add(new UICommand("Recheck Internet Connection", (UICommandInvokedHandler) =>
                            {
                                if (IsConnectedToInternet())  //else --> if --> if --> if
                                {
                                    Internet_notifier.Text = "";
                                    Getcoordinates(place, zip); //calling this method to set lat , longi and location values 
                                    //Debug.WriteLine(location);
                                   // GetjasonValues(lat.ToString(), longi.ToString());
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
                            this.Frame.Navigate(typeof(soiltestOptions), tf);
                        }
                    }
                    else
                    {
                        MessageDialog msg = new MessageDialog("Missed some fields, please enter them to continue", "Error");
                        await msg.ShowAsync();
                    }
                }
            }
            catch
            {
                //exception handling



            }
        }

      

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(testmode));
        }

        
      
       

 }
    public class t_from_testcommon
    {
        public string testname { get; set; }
        public int soiltype { get; set; }
        public string landcovered { get; set; }
        public int season { get; set; }
        public string temp_c { get; set; }
        public string humidity { get; set; }
    }
}


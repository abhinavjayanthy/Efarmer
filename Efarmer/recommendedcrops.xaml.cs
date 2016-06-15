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
using Windows.Storage;
using SQLite;
using System.Diagnostics;
using Windows.UI.Popups;
using HtmlAgilityPack;
using System.Text;



// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Efarmer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class recommendedcrops : Page
    {
        /*variables used for comparision*/
        public string season;
        public string soiltype;
        public string ph;
        public string ec;
        /*other*/
        public string testname;
        public string pincode;
        public string place;
        public float landcovered;
        public string temp_c;
        public string humidity;
        public float n;
        public float p;
        public float k;
        public string mositure;
        public string mode;
        public string date_time; 



        public List<string> keyword = new List<string>(); //list which stores the  "crop"
        //public Dictionary<string, int> keyword = new Dictionary<string, int>();

        public string presentkeyword; //present selected crop in the list 

        public List<string> checker = new List<string>();

        public recommendedcrops()
        {
            this.InitializeComponent();
            Loaded += recommendedcrops_Loaded;
        }
        void recommendedcrops_Loaded(object sender, RoutedEventArgs e)
        {
            

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            compare(); //method that compares the efarmer.db->testdata->(season,soiltype,ph,ec) with sdb.db->cropdata->(season,soiltype,ph,ec) and gives the relevent crop
        }




       async private void compare()
        {
            //int i = 0;
            var con = new SQLiteConnection(Class1.stdbpath);
            var con2 = new SQLiteConnection(Class1.dbPath);

            var userdata = con2.Table<userdata>().Where(x => x.id != null).OrderByDescending(x => x.id).Take(1);
            foreach (var v3 in userdata)
            {
                place = v3.place;
                pincode = v3.zipcode;
            }


            var testdata_query = con2.Table<testdata>().Where(x => x.id != null).OrderByDescending(x => x.id).Take(1); //query to get latest test data from efarmer.db
            var standarddata_query = con.Table<cropdata>(); //query to to get standard data from sdb.db

            foreach (var v1 in testdata_query) //v1 pointing to the latest values from efarmer.db => testdata
            {
                /*Variables used for comparison*/
                season = v1.season;
                soiltype = v1.soiltype;
                ph = v1.ph;
                ec = v1.ec;

                /*other*/
                testname = v1.testname;
                landcovered = float.Parse(v1.landcovered); area_from_user.Text = v1.landcovered; 
                temp_c = v1.temperature; temp_from_user.Text = v1.temperature;
                humidity = v1.humidity;
                n = float.Parse(v1.nitrogen); n_db.Text = v1.nitrogen;
                p = float.Parse(v1.phosphorous); p_db.Text = v1.phosphorous;
                k = float.Parse(v1.potassium); k_db.Text = v1.potassium;
                mositure = v1.moisture;
                mode = v1.mode;
                date_time = v1.datetime;
               
            }
            foreach (var v2 in standarddata_query)
            {
                if (season == v2.season && soiltype == v2.soiltype && ph == v2.ph && ec == v2.ec)
                {
                    keyword.Add(v2.crop);
                }
            }
            if (keyword.Count!=0)
            {
                crop_list.ItemsSource = keyword;
            }
            else
            {
                var md = new MessageDialog("Your soil is great but our database is not enough to analysis your soil.Try giving better soil values");
                md.Commands.Add(new UICommand("Perform Soiltest Again", (UICommandInvokedHandler) =>
                {

                    this.Frame.Navigate(typeof(testmode));

                }));
                md.Commands.Add(new UICommand("Return To HomePage", (UICommandInvokedHandler) =>
                {

                    this.Frame.Navigate(typeof(MainPage));

                }));
                await md.ShowAsync();
            }
        }

        private void crop_list_SelectionChanged(object sender, SelectionChangedEventArgs e) //to track change of list items
        {
            string lbi2="";
            if (e.AddedItems.Count > 0)// the items that were added to the "selected" collection
            {
                var lbi = e.AddedItems[0] as string;
                if (null != lbi) // prevents errors if casting fails
                {
                    lbi2 = lbi;
                    presentkeyword = lbi;
                    current_crop_header.Text = presentkeyword;
                }
            }
            cropinfo(presentkeyword);

            //To check the crop is already saved or not
            if (checker.Contains(lbi2))
            {
                save_status.Text = "Saved";
            }
            else
            {
                save_status.Text = "NotSaved";
            }
           
        }

      async  private void cropinfo(string keyword) //function which displays relevent data according to keyword
        {

            string nitrogen = "n"/*Dummy value*/;
            string phosphorous = "p"/*Dummy value*/;
            string pottasium = "k"/*Dummy value*/;
            float yield_from_file =0/*Dummy value*/;  //yield ton/ha from file

           // data.Source = new Uri("http://www.fao.org/nr/water/cropinfo_"+keyword+".html", UriKind.Absolute);
            data.Source = new Uri("ms-appx-web:///Data/"+keyword+".html");


            HtmlDocument htmlDoc = new HtmlDocument();

            Uri u = new Uri("ms-appx:///Data/" + keyword + ".html");

            var file = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(@"Data\" + keyword + ".html");
            var stream = await file.OpenReadAsync();
            var rdr = new StreamReader(stream.AsStream());
            var content = rdr.ReadToEnd();
            htmlDoc.LoadHtml(content);
            foreach (var tag in htmlDoc.DocumentNode.Descendants())
            {
                        if (tag.Id == "avgn") { nitrogen = tag.InnerText; }
                        if (tag.Id == "avgp") { phosphorous = tag.InnerText; }
                        if (tag.Id == "avgk") { pottasium = tag.InnerText; }
                        if (tag.Id == "yph") { yield_from_file = float.Parse(tag.InnerText); }
            }
           
            n_from_file.Text = nitrogen; //required n from file
            p_from_file.Text = phosphorous; //required p from file
            k_from_file.Text = pottasium; //required k from file

            yield.Text = (landcovered * yield_from_file).ToString();


            

            
          float ndiff = ((float.Parse(nitrogen) - n) / float.Parse(nitrogen)) * 100;//n:value from user & nitrogen: value from html file
          if (ndiff >= 0) { n_diff.Text = ndiff.ToString() + "% less"; } else { n_diff.Text = Math.Abs(ndiff).ToString()+"% more"; }
           
          float pdiff = ((float.Parse(phosphorous) - p) / float.Parse(phosphorous)) * 100;
          if (pdiff >= 0) { p_diff.Text = pdiff.ToString() + "% less"; } else { p_diff.Text = Math.Abs(pdiff).ToString() + "% more"; }

          float kdiff = ((float.Parse(pottasium) - k) / float.Parse(pottasium)) * 100;
          if (kdiff >= 0) { k_diff.Text = kdiff.ToString() + "% less"; } else { k_diff.Text = Math.Abs(kdiff).ToString() + "% more"; } 

          if (ndiff >= 25f) { nfr.Text = "YES"; } else { nfr.Text = "NO"; }
          if (pdiff >= 25f) {pfr.Text = "YES"; } else { pfr.Text = "NO"; }
          if (kdiff >= 25f) { kfr.Text = "YES"; } else { kfr.Text = "NO"; }
      }

     
     async private void save_Click(object sender, RoutedEventArgs e)
      {
         
             if (presentkeyword != null)
             {
                 var conn = new SQLiteConnection(Class1.dbpath1);


                 if (save_status.Text == "NotSaved")
                 {
                     conn.CreateTable<analysis>();
                     conn.Insert(new analysis() { rc = presentkeyword, testid = testname, postalcode = pincode, landcov = landcovered.ToString(), location = place, soil = soiltype, period = season, sf_n = n.ToString(), sf_p = p.ToString(), sf_k = k.ToString(), s_ph = ph, s_ec = ec, s_temp = temp_c, rc_n = n_from_file.Text, rc_p = p_from_file.Text, rc_k = k_from_file.Text, s_n_diff = n_diff.Text, s_p_diff = p_diff.Text, s_k_diff = k_diff.Text, nfr_req = nfr.Text, pfr_req = pfr.Text, kfr_req = kfr.Text, e_yield = yield.Text, datetime = date_time, actualyield = "", n_used = -1, p_used = -1, k_used = -1, sold_price = "" });

                     checker.Add(presentkeyword);

                     save_status.Text = "Saved";

                     MessageDialog msg = new MessageDialog(presentkeyword + "Crop Saved Sucessfully", "Done!");
                     await msg.ShowAsync();
                 }
                 else
                 {
                     MessageDialog msg = new MessageDialog("Already Saved", "Error!");
                     await msg.ShowAsync();
                 }

             
                          
              
              
          }
          else
          {
              MessageDialog msg = new MessageDialog("Select Crop From list to save","Oops!");
              await msg.ShowAsync();
          }
     
          
      }

     private void Button_Click_1(object sender, RoutedEventArgs e)
     {
         var md = new MessageDialog("Are you sure you want to final the Analysis.Unsaved information will be lost");
         md.Commands.Add(new UICommand("Yes", (UICommandInvokedHandler) =>
         {
             this.Frame.Navigate(typeof(MainPage));
         }));
         md.Commands.Add(new UICommand("No"));
         md.ShowAsync();
         
     }

   
   
        
       
}
    public class cropdata 
    {
        public string crop { get; set; }
        public string season { get; set; }
        public string soiltype { get; set; }
        public string ph { get; set; }
        public string ec { get; set; }
    }
    public class analysis
    {
        public string rc { get; set; } //reccommeded crop

        public string testid { get; set; }
        public string postalcode { get; set; }
        public string location { get; set; }
        public string landcov { get; set; }
        public string soil { get; set; }
        public string period { get; set; }
        public string sf_n { get; set; } //soil n value
        public string sf_p { get; set; } //soil p value
        public string sf_k { get; set; } //soil k value
        public string s_ph { get; set; }
        public string s_ec { get; set; }
        public string s_temp { get; set; }
       

        public string rc_n { get; set; } //reccommended crop req n
        public string rc_p { get; set; } //reccommended crop req p
        public string rc_k { get; set; } //reccommended crop req k

        public string s_n_diff { get; set; } //n difference
        public string s_p_diff { get; set; } //p difference
        public string s_k_diff { get; set; } //k difference

        public string nfr_req { get; set; }  //n req
        public string pfr_req { get; set; }  //p req
        public string kfr_req { get; set; }  //k req

        public string e_yield { get; set; } //Expected yield

        public string datetime { get; set; }

        /////////////////////////////////////////////////////////

        public string actualyield { get; set; }
        public int n_used { get; set; }
        public int p_used { get; set; }
        public int k_used { get; set; }
        public string sold_price { get; set; }


    }
}

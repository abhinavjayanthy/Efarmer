using System;
using System.IO;

namespace Efarmer
{
    public class Class1
    {
        //read and write database "efarmer.db" path 
        public static string dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "efarmer.db");
        //read only database "sdb.db" path
        public static string stdbpath = Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path, "sdb.db");

        public static string dbpath1 = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "analysis.db");
    }
}

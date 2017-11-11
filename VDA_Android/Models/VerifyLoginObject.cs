using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace VDA_Android.Models
{
    class VerifyLoginObject
    {
        public bool isAdmin { get; set; }
        public bool validUser { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string dealer_name { get; set; }
    }





}
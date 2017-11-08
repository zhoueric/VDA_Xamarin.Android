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
    class ActionObject
    {
        public string kpi { get; set; }
        public string type { get; set; }
        public string actionP { get; set; }
        public string actionLink { get; set; }
    }
}
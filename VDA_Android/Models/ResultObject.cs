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
    public class ResultObject
    {
        public ResultObject()
        {
            action_list = new List<KpiAction>();
        }
        public int id;
        public string name { get; set; }
        public int value { get; set; }
        public double p_val { get; set; }
        public string segment { get; set; }
        public string brand { get; set; }
        public string dealer { get; set; }
        public string month { get; set; }
        public string type { get; set; }
        public string model { get; set; }
        public List<KpiAction> action_list { get; set; }
    }

    public class KpiAction
    {
        public string actionP { get; set; }
        public string kpi { get; set; }
        public string type { get; set; }
    }
}
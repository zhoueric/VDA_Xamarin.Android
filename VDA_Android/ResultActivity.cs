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
using System.Json;
using System.Threading;
using System.Net;


using VDA_Android.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.IO;

namespace VDA_Android
{
    [Activity(Label = "ResultActivity")]
    public class ResultActivity : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Result);

            //ResultObject relatedKPI = JsonConvert.DeserializeObject<ResultObject>("{\"action_list\": null, \"brand\": \"\", \"dealer\": null, \"id\": 0, \"month\": null, \"name\": \"Dealer Sales\", \"p_val\": 0.4955333944296611, \"segment\": \"Total\", \"type\": null, \"value\": 399}");

            string speechStr = Intent.GetStringExtra("speechStr") ?? "Data not available";

            but = FindViewById<Button>(Resource.Id.temp);
            resultLayout = FindViewById<LinearLayout>(Resource.Id.resultLayout);

            //var result1 = (TextView)FindViewById(Resource.Id.result1);
            //result1.Text = "asdf";

            but.Click += async (sender, e) =>
            {
                string urlBase = "https://virtualdealershipadvisorapi.azurewebsites.net/api/RelatedKpi?";

                string queryParsed = Uri.EscapeDataString(speechStr);

                string urlQuery = "query=" + queryParsed + '&';

                string urlDealer = "dealer_name=" + "omega";

                string url = urlBase + urlQuery + urlDealer;

                json = await FetchKPIAsync(url);

                DisplayJson();
            };

            but.PerformClick();

            button1 = FindViewById<Button>(Resource.Id.button1);

            button1.Click += delegate
            {
                var act = new Intent(this, typeof(ActionActivity));
                //act.PutExtra("speechStr", speechStr);
                StartActivity(act);
            };

        }

        private async Task<JsonValue> FetchKPIAsync(string url)
        {
            WebRequest request = WebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";

            using (WebResponse response = await request.GetResponseAsync())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
                    Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());

                    return jsonDoc;
                }
            }
        }


        private void DisplayJson()
        {
            string a = json.ToString();

            relatedKPI = JsonConvert.DeserializeObject<ResultObject>(json.ToString());

            double p = relatedKPI.p_val;

            var value1 = FindViewById<TextView>(Resource.Id.value1);
            value1.Text = relatedKPI.value.ToString();
            value1.Visibility = ViewStates.Visible;

            var p1 = FindViewById<TextView>(Resource.Id.p1);
            p1.Text = "KPI Percentile: " + Math.Round(relatedKPI.p_val * 100, 2).ToString() + "%";
            p1.Visibility = ViewStates.Visible;

            var result1 = FindViewById<TextView>(Resource.Id.result1);
            string text = "You are performing ";

            text += p > 0.5 ? "above" : "below";

            text += " average on this measurment.\n\nClick below to view actions you can take to improve on this metric.";

            result1.Text = text;

            button1.Visibility = ViewStates.Visible;


            //// create a new textview
            //var rowTextView = new TextView(this);

            //// set some properties of rowTextView or something
            //rowTextView.Text = relatedKPI.p_val.ToString();

            //// add the textview to the linearlayout
            //resultLayout.AddView(rowTextView);

            //TextView box = FindViewById<TextView>(Resource.Id.KPIText);

            //JsonValue kpiResults = json
        }

        public LinearLayout resultLayout;
        public ResultObject relatedKPI;
        private JsonValue json;
        private Button but;
        private Button button1;
    }
}
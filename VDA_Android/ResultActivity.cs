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

            // Sets fake button to be simulated clicked to start async
            butRelated = FindViewById<Button>(Resource.Id.butRelated);
            resultLayout = FindViewById<LinearLayout>(Resource.Id.resultLayout);

            butNeeded = FindViewById<Button>(Resource.Id.butNeeded);

            // Starts async for related KPI
            butRelated.Click += async (sender, e) =>
            {
                string urlBase = "https://virtualdealershipadvisorapi.azurewebsites.net/api/RelatedKpi?";

                string queryParsed = Uri.EscapeDataString(speechStr);

                string urlQuery = "query=" + queryParsed + '&';

                string urlDealer = "dealer_name=" + "omega";

                string url = urlBase + urlQuery + urlDealer;

                jsonRelated = await FetchKPIAsync(url);

                DisplayJson();
            };

            butNeeded.Click += async (sender, e) =>
            {
                string urlBase = "https://virtualdealershipadvisorapi.azurewebsites.net/api/NeededKpi?";

                string queryParsed = Uri.EscapeDataString(speechStr);

                string urlQuery = "query=" + queryParsed + '&';

                string urlDealer = "dealer_name=" + "omega";

                string url = urlBase + urlQuery + urlDealer;

                jsonNeeded = await FetchKPIAsync(url);

                DisplayJsonNeeded();
            };

            butRelated.PerformClick();

            butNeeded.PerformClick();

            butToActions = FindViewById<Button>(Resource.Id.butToActions);

            butToActions.Click += delegate
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
            string a = jsonRelated.ToString();

            relatedKPI = JsonConvert.DeserializeObject<ResultObject>(jsonRelated.ToString());

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

            butToActions.Visibility = ViewStates.Visible;

            //// create a new textview
            //var rowTextView = new TextView(this);

            //// set some properties of rowTextView or something
            //rowTextView.Text = relatedKPI.p_val.ToString();

            //// add the textview to the linearlayout
            //resultLayout.AddView(rowTextView);

            //TextView box = FindViewById<TextView>(Resource.Id.KPIText);

            //JsonValue kpiResults = json
        }

        private void DisplayJsonNeeded()
        {
            string a = jsonNeeded.ToString();

            neededKPI = JsonConvert.DeserializeObject<ResultObject>(jsonRelated.ToString());

            string s = neededKPI.GetType().ToString();

            string g = "a";

            //double p = relatedKPI.p_val;

            //var value1 = FindViewById<TextView>(Resource.Id.value1);
            //value1.Text = relatedKPI.value.ToString();
            //value1.Visibility = ViewStates.Visible;

            //var p1 = FindViewById<TextView>(Resource.Id.p1);
            //p1.Text = "KPI Percentile: " + Math.Round(relatedKPI.p_val * 100, 2).ToString() + "%";
            //p1.Visibility = ViewStates.Visible;

            //var result1 = FindViewById<TextView>(Resource.Id.result1);
            //string text = "You are performing ";

            //text += p > 0.5 ? "above" : "below";

            //text += " average on this measurment.\n\nClick below to view actions you can take to improve on this metric.";

            //result1.Text = text;

            //butToActions.Visibility = ViewStates.Visible;

        }

        public LinearLayout resultLayout;
        public ResultObject relatedKPI;
        public ResultObject neededKPI;

        // respective json return variables from API
        private JsonValue jsonRelated;
        private JsonValue jsonNeeded;

        // fake buttons to be click simluated
        private Button butRelated;
        private Button butNeeded;

        private Button butToActions;
    }
}
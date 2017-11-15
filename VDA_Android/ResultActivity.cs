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
using Android.Graphics;
using Android.Support.V7.Widget;

//using System.Net.Http;
//using System.Net.Http.Headers;


namespace VDA_Android
{
    [Activity(Label = "ResultActivity")]
    public class ResultActivity : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Context mContext = Android.App.Application.Context;
            AppPreferences ap = new AppPreferences(mContext);
            string dealer_name = ap.getUsername();

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
                string urlBase = "https://msufall2017virtualdealershipadviserapi.azurewebsites.net/api/RelatedKpi?";

                string queryParsed = Uri.EscapeDataString(speechStr);

                string urlQuery = "query=" + queryParsed + '&';

                string urlDealer = "dealer_name=" + dealer_name;

                string url = urlBase + urlQuery + urlDealer;

                jsonRelated = await FetchKPIAsync(url);

                DisplayJsonRelated();
            };

            butNeeded.Click += async (sender, e) =>
            {
                string urlBase = "https://msufall2017virtualdealershipadviserapi.azurewebsites.net/api/NeededKpi?";

                string queryParsed = Uri.EscapeDataString(speechStr);

                string urlQuery = "query=" + queryParsed + '&';

                string urlDealer = "dealer_name=" + dealer_name;

                string url = urlBase + urlQuery + urlDealer;

                jsonNeeded = await FetchKPIAsync(url);

                DisplayJsonNeeded();
            };

            butRelated.PerformClick();

            butNeeded.PerformClick();

        }

        private void StartActions(string name, double p_val)
        {
            var act = new Intent(this, typeof(ActionActivity));
            act.PutExtra("name", name);
            act.PutExtra("p_val", p_val.ToString());
            StartActivity(act);
        }

        private async Task<JsonValue> FetchKPIAsync(string url)
        {
            //var client = new HttpClient();

            //client.DefaultRequestHeaders.Accept.Clear();
            ////add any default headers below this
            //client.DefaultRequestHeaders.Accept.Add(
            //    new MediaTypeWithQualityHeaderValue("application/json"));

            //HttpResponseMessage response = await client.GetAsync(url);

            //string json_string = "";
            //if (response.StatusCode == HttpStatusCode.OK)
            //{
            //    json_string = await response.Content.ReadAsStringAsync();

            //}
            //return json_string;
            WebRequest request = WebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";

            try
            {
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
            catch{
                return null;
            }
            
        }

        private void DisplayJsonRelated()
        {
            try
            {
                string a = jsonRelated.ToString();

                relatedKPI = JsonConvert.DeserializeObject<ResultObject>(jsonRelated.ToString());

                double p = relatedKPI.p_val;

                var value1 = FindViewById<TextView>(Resource.Id.value1);
                var brand = relatedKPI.brand == "all" ? "" : relatedKPI.brand + " ";
                var model = relatedKPI.model == "all" ? "" : relatedKPI.model + " ";
                var segment = relatedKPI.segment == "all" ? "" : relatedKPI.segment + " ";

                value1.Text = brand + model + segment + relatedKPI.name
                    + ": " + relatedKPI.value.ToString();
                value1.Visibility = ViewStates.Visible;

                var p1 = FindViewById<TextView>(Resource.Id.p1);
                p1.Text = "KPI Percentile: " + Math.Round(relatedKPI.p_val * 100, 2).ToString() + "%";
                p1.Visibility = ViewStates.Visible;

                var result1 = FindViewById<TextView>(Resource.Id.result1);
                string text = "You are performing ";

                text += p > 0.5 ? "above" : "below";

                text += " average on this measurment.\n\nClick below to view actions you can take to improve on this metric.";

                var frame = FindViewById<FrameLayout>(Resource.Id.relatedFrameLayout);
                if (p < .3)
                {
                    frame.SetBackgroundResource(Resource.Drawable.card_edge_red);
                }
                else if (p < .5)
                {
                    frame.SetBackgroundResource(Resource.Drawable.card_edge_yellow);
                }
                result1.Text = text;

                butToActions = FindViewById<Button>(Resource.Id.butToActions);

                butToActions.Click += delegate
                {
                    StartActions(relatedKPI.name, relatedKPI.p_val);
                };

                butToActions.Visibility = ViewStates.Visible;
            }
            catch
            {
                var result1 = FindViewById<TextView>(Resource.Id.result1);
                result1.Text = "I could not found any related KPI for your question";
            }
            
        }

        private void DisplayJsonNeeded()
        {
            neededKPI = new List<ResultObject>();

            neededKPI = JsonConvert.DeserializeObject<List<ResultObject>>(jsonNeeded.ToString());

            foreach (var KPI in neededKPI)
            {
                // ========================================================================================

                // create a new cardview
                var card = new CardView(this);

                // set card elevation
                card.SetMinimumHeight(300);
                card.UseCompatPadding = true;
                card.Elevation = 4;
                card.Radius = 5;
                card.SetForegroundGravity(GravityFlags.CenterHorizontal);
                card.SetPadding(0, 100, 0, 100);

                // ========================================================================================

                // create a new linearlayout
                var linearLayout = new LinearLayout(this);

                LinearLayout.LayoutParams ll2 = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,
                    ViewGroup.LayoutParams.WrapContent);
                linearLayout.SetGravity(GravityFlags.CenterHorizontal);
                linearLayout.LayoutParameters = ll2;

                linearLayout.Orientation = Orientation.Vertical;
                linearLayout.SetGravity(GravityFlags.Center);
                linearLayout.SetPadding(3,3,3,3);

                // ========================================================================================

                // create a new textview for value parameter
                var valueTextView = new TextView(this);

                // set some properties of valueTextView
                LinearLayout.LayoutParams ll = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,
                    ViewGroup.LayoutParams.WrapContent);
                ll.SetMargins(0, 10, 0, 10);
                valueTextView.LayoutParameters = ll;

                valueTextView.Text = KPI.brand + " " + KPI.model + " " + KPI.name
                + ": " + KPI.value.ToString();
                valueTextView.TextSize = 15;

                // add the textview to the linearLayout
                linearLayout.AddView(valueTextView);

                // ========================================================================================

                // create a new textview for p-val parameter
                var pvalTextView = new TextView(this);

                LinearLayout.LayoutParams ll1 = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,
                    ViewGroup.LayoutParams.WrapContent);
                ll1.SetMargins(0, 10, 0, 10);
                pvalTextView.LayoutParameters = ll1;

                pvalTextView.Text = "KPI Percentile: " + Math.Round(KPI.p_val * 100, 2).ToString() + "%";
                pvalTextView.TextSize = 15;
                pvalTextView.SetTextColor(Color.Black);

                // add the textview to the linearLayout
                linearLayout.AddView(pvalTextView);

                // ========================================================================================

                // create a new textview for result
                var resultTextView = new TextView(this);

                LinearLayout.LayoutParams ll3 = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,
                    ViewGroup.LayoutParams.WrapContent);
                ll3.Gravity = GravityFlags.Center;
                ll3.SetMargins(0, 10, 0, 10);

                resultTextView.LayoutParameters = ll3;
                resultTextView.Gravity = GravityFlags.Center;

                string text = "You are performing ";

                text += KPI.p_val > 0.5 ? "above" : "below";

                text += " average on this measurment.\n\nClick below to view actions you can take to improve on this metric.";

                resultTextView.Text = text;

                

                linearLayout.AddView(resultTextView);
                //var testText = new TextView(this);

                //testText.Text = "hello world";

                //linearLayout.AddView(testText);

                // =========================================================================================

                // create a new button to link KPI to action

                var actionsButton = new Button(this);

                LinearLayout.LayoutParams ll5 = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,
                    ViewGroup.LayoutParams.WrapContent);
                ll5.SetMargins(0, 10, 0, 10);
                actionsButton.LayoutParameters = ll5;

                actionsButton.Text = "View Actions";

                actionsButton.Click += delegate
                {
                    StartActions(KPI.name, KPI.p_val);
                };

                //var testText1 = new TextView(this);

                //testText1.Text = "hello world";

                //linearLayout.AddView(testText1);
                linearLayout.AddView(actionsButton);
                //var testText2 = new TextView(this);

                //testText2.Text = "hello world";

                //linearLayout.AddView(testText2);


                //=========================================================================================

                // create a new frameLayout for border

                var frameLayout = new FrameLayout(this);

                LinearLayout.LayoutParams ll4 = new LinearLayout.LayoutParams(4,
                    ViewGroup.LayoutParams.MatchParent);
                frameLayout.LayoutParameters = ll4;
                frameLayout.SetForegroundGravity(GravityFlags.CenterHorizontal);

                frameLayout.SetBackgroundResource(Resource.Drawable.card_edge_red);

                card.AddView(frameLayout);

                card.AddView(linearLayout);

                resultLayout.AddView(card);
            }



        }

        public LinearLayout resultLayout;
        public ResultObject relatedKPI;
        public List<ResultObject> neededKPI;

        // respective json return variables from API
        private JsonValue jsonRelated;
        private JsonValue jsonNeeded;

        // fake buttons to be click simluated
        private Button butRelated;
        private Button butNeeded;

        private Button butToActions;
    }
}
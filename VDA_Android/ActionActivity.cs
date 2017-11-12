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
using Android.Graphics.Drawables;
using Android.Graphics;

using System.Json;
using System.Threading.Tasks;
using System.Net;
using System.IO;

using VDA_Android.Models;
using Newtonsoft.Json;
using Android.Support.V7.Widget;

namespace VDA_Android
{
    [Activity(Label = "Actions")]
    public class ActionActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Action);

            ColorDrawable colorDrawable = new ColorDrawable(Color.Rgb(153,204,0));
            ActionBar.SetBackgroundDrawable(colorDrawable);

            // Grabs values passed from previous activity
            string KPIname = Intent.GetStringExtra("name") ?? "Data not available";
            string KPIp_val = Intent.GetStringExtra("p_val") ?? "Data not available";

            // Grab UI elements to later modify
            actionLayout = FindViewById<LinearLayout>(Resource.Id.actionLayout);
            
            // Invisible button to start Actions async
            butActions = FindViewById<Button>(Resource.Id.butActions);

            // Starts async for related KPI
            butActions.Click += async (sender, e) =>
            {
                string urlBase = "https://msufall2017virtualdealershipadviserapi.azurewebsites.net/api/Actions?";

                string nameParsed = Uri.EscapeDataString(KPIname);

                string urlName = "name=" + nameParsed + '&';

                string urlValue = "value=" + KPIp_val.ToString();

                string url = urlBase + urlName + urlValue;

                jsonActions = await FetchKPIAsync(url);

                DisplayJsonActions();
            };

            var startOverButton = FindViewById<Button>(Resource.Id.startOverButton);

            startOverButton.Click += delegate
            {
                var act = new Intent(this, typeof(MainActivity));
                //act.PutExtra("speechStr", speechStr);
                StartActivity(act);
            };

            butActions.PerformClick();

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

        private void DisplayJsonActions()
        {
            actionKPI = new List<ActionObject>();

            actionKPI = JsonConvert.DeserializeObject<List<ActionObject>>(jsonActions.ToString());

            foreach (var KPI in actionKPI)
            {
                // ========================================================================================

                // create a new cardview
                var card = new CardView(this);

                // set card elevation
                card.SetMinimumHeight(200);
                card.UseCompatPadding = true;
                card.Elevation = 4;
                card.Radius = 5;
                card.SetForegroundGravity(GravityFlags.Center);
                card.SetPadding(0, 100, 0, 100);

                // ========================================================================================

                // create a new linearlayout
                var linearLayout = new LinearLayout(this);

                LinearLayout.LayoutParams ll2 = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.FillParent,
                    295);
                linearLayout.LayoutParameters = ll2;

                linearLayout.Orientation = Orientation.Vertical;
                linearLayout.SetGravity(GravityFlags.CenterHorizontal);
                linearLayout.SetPadding(8, 8, 8, 8);

                // ========================================================================================

                // create a new textview for value parameter
                var descriptionTextView = new TextView(this);

                // set some properties of valueTextView
                LinearLayout.LayoutParams ll = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,
                    ViewGroup.LayoutParams.WrapContent);
                ll.SetMargins(0, 10, 0, 10);
                ll.Gravity = GravityFlags.CenterHorizontal;
                descriptionTextView.LayoutParameters = ll;

                descriptionTextView.Text = KPI.actionP;

                descriptionTextView.TextSize = 25;

                descriptionTextView.SetTextColor(Color.Black);

                descriptionTextView.Gravity = GravityFlags.CenterHorizontal;

                // add the textview to the linearLayout
                linearLayout.AddView(descriptionTextView);

                // ========================================================================================

                // create a new button to link KPI to action

                var takeActionButton = new Button(this);

                LinearLayout.LayoutParams ll5 = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,
                    ViewGroup.LayoutParams.WrapContent);
                ll5.SetMargins(0, 10, 0, 10);

                takeActionButton.LayoutParameters = ll5;

                takeActionButton.Text = "Take Action";

                //actionsButton.Click += delegate
                //{
                //    StartActions(KPI.name, KPI.p_val);
                //};

                linearLayout.AddView(takeActionButton);


                //=========================================================================================

                // create a new frameLayout for border

                var frameLayout = new FrameLayout(this);

                LinearLayout.LayoutParams ll4 = new LinearLayout.LayoutParams(4,
                    ViewGroup.LayoutParams.MatchParent);
                frameLayout.LayoutParameters = ll4;

                frameLayout.SetBackgroundResource(Resource.Drawable.card_edge_red);

                card.AddView(frameLayout);

                card.AddView(linearLayout);

                actionLayout.AddView(card);
            }



        }
        public LinearLayout actionLayout;

        private List<ActionObject> actionKPI;

        private Button butActions;
        private JsonValue jsonActions;
    }
}
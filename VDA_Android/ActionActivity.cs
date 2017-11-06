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

            butActions = FindViewById<Button>(Resource.Id.butRelated);

            string KPIname = Intent.GetStringExtra("name") ?? "Data not available";

            string KPIp_val = Intent.GetStringExtra("p_val") ?? "Data not available";

            var test = FindViewById<TextView>(Resource.Id.action12);
            test.Text = KPIname + " " + KPIp_val;

            //// Starts async for related KPI
            //butActions.Click += async (sender, e) =>
            //{
            //    string urlBase = "https://msufall2017virtualdealershipadviserapi.azurewebsites.net/api/RelatedKpi?";

            //    string queryParsed = Uri.EscapeDataString(speechStr);

            //    string urlQuery = "query=" + queryParsed + '&';

            //    string urlDealer = "dealer_name=" + dealer_name;

            //    string url = urlBase + urlQuery + urlDealer;

            //    jsonRelated = await FetchKPIAsync(url);

            //    DisplayJsonRelated();
            //};

            var startOverButton = FindViewById<Button>(Resource.Id.startOverButton);

            startOverButton.Click += delegate
            {
                var act = new Intent(this, typeof(MainActivity));
                //act.PutExtra("speechStr", speechStr);
                StartActivity(act);
            };

        }

        private Button butActions;
    }
}
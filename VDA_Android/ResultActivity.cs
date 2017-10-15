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
using System.Threading.Tasks;
using System.Net;
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

            string speechStr = Intent.GetStringExtra("speechStr") ?? "Data not available";

            Task.Run(async () =>
            {
                string urlBase = "https://virtualdealershipadvisorapi.azurewebsites.net/api/RelatedKpi?";

                string queryParsed = Uri.EscapeDataString(speechStr);
            
                string urlQuery = "query=" + queryParsed + '&';

                string urlDealer = "dealer_name=" + "omega";

                string url = urlBase + urlQuery + urlDealer;

                JsonValue json = await FetchKPIAsync(url);
                DisplayJson(json);
            });
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

        private void DisplayJson(JsonValue json)
        {
            //TextView box = FindViewById<TextView>(Resource.Id.KPIText);

            //JsonValue kpiResults = json
        }
    }
}
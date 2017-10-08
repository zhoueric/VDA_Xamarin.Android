using System;
using System.Json;
using System.Net;
using System.Threading;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Speech;
using System.IO;
using System.Threading.Tasks;

namespace VDA_Android
{
    [Activity(Label = "VDA_Android", MainLauncher = true)]

    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            speechResult = (TextView)FindViewById(Resource.Id.speechResult);

            Button button = FindViewById<Button>(Resource.Id.confirmText);

            button.Click += async (sender, e) =>
            {
                string url = "https://virtualdealershipadvisorapi.azurewebsites.net/api/kpi?query=[sedan]";

                JsonValue json = await FetchKPIAsync(url);
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

        [Java.Interop.Export("GetSpeechInput")]
        public void GetSpeechInput(View view)
        {
            Intent intent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            intent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);

            StartActivityForResult(intent, 10);

            OnCall();
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if(requestCode == 10){
                var resultList = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                speechResult.Text = resultList[0];
            }
        }

        protected void OnCall()
        {

            
        }

        //protected String doInBackground(string url)
        //{
        //    try
        //    {
        //        URL url = 
        //    }
        //}

        private TextView speechResult;
    }
}


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



namespace VDA_Android
{
    //[Activity(Label = "Virtual Dealership Advisor", MainLauncher = true)]

    [Activity(Label = "LoginActivity", MainLauncher = true)]
    class LoginActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Login);

            userInput = (EditText)FindViewById(Resource.Id.userInput);


            buttonGroup = FindViewById<LinearLayout>(Resource.Id.buttonWrap);

            buttonConfirm = FindViewById<Button>(Resource.Id.confirmText);

            // Starts async for login
            buttonConfirm.Click += async (sender, e) =>
            {
                string urlBase = "https://msufall2017virtualdealershipadviserapi.azurewebsites.net/api/VerifyLogin?";

                string queryParsed = Uri.EscapeDataString(userInput.Text);

                string urlQuery = "dealer_name=" + queryParsed;

                string url = urlBase + urlQuery;

                jsonLogin = await FetchLoginCredentialsAsync(url);

                DisplayJsonLogin();

            };

        }
        private void DisplayJsonLogin()
        {
            VerifyLoginObject login_credentials = JsonConvert.DeserializeObject<VerifyLoginObject>(jsonLogin.ToString());

            if (login_credentials.validUser)
            {
                var res = new Intent(this, typeof(MainActivity));
                //res.PutExtra("dealer_name", userInput.Text);
                Context mContext = Android.App.Application.Context;
                AppPreferences ap = new AppPreferences (mContext);
                string key = userInput.Text;
                ap.saveUsername(key);
                StartActivity(res);
            }
        }

        private async Task<JsonValue> FetchLoginCredentialsAsync(string url)
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

        private EditText userInput;
        private JsonValue jsonLogin;

        private Button buttonConfirm;
        private LinearLayout buttonGroup;
        
    } 
    
}
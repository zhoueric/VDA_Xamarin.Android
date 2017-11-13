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


using System.Net.Http;
using System.Net.Http.Headers;



namespace VDA_Android
{
    //[Activity(Label = "Virtual Dealership Advisor", MainLauncher = true)]

    [Activity(Label = "LoginActivity")]
    class LoginActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Login);

            usernameInput = (EditText)FindViewById(Resource.Id.usernameInput);
            passwordInput = (EditText)FindViewById(Resource.Id.passwordInput);

            loginResponse = (TextView)FindViewById(Resource.Id.loginResponse);

            speechResultTop = (TextView)FindViewById(Resource.Id.speechResultTop);
            speechResultTop.Typeface = Typeface.CreateFromAsset(Assets, "Fonts/Roboto-Light.ttf");

            speechResultBottom = (TextView)FindViewById(Resource.Id.speechResultBottom);
            speechResultBottom.Typeface = Typeface.CreateFromAsset(Assets, "Fonts/Roboto-Light.ttf");
            speechResultTop.Text = "Dealer ID";
            speechResultBottom.Text = "Password";

            buttonGroup = FindViewById<LinearLayout>(Resource.Id.buttonWrap);

            buttonConfirm = FindViewById<Button>(Resource.Id.confirmText);
            
            // Starts async for login
            buttonConfirm.Click += async (sender, e) =>
            {
                string urlBase = "https://msufall2017virtualdealershipadviserapi.azurewebsites.net/api/VerifyLogin?";
                //string urlBase = "http://localhost:65007/api/VerifyLogin?";
                string username = Uri.EscapeDataString(usernameInput.Text);
                string password = Uri.EscapeDataString(passwordInput.Text);
                string url = $"{urlBase}username={username}&password={password}";
                //string url = urlBase + " usernme =" + Uri.EscapeDataString(usernameInput.Text) + "&password=" + Uri.EscapeDataString(passwordInput.Text);
                jsonLogin = await FetchLoginCredentialsAsync(url);
                if(jsonLogin != "")
                {
                    DisplayJsonLogin();
                }
                else
                {
                    loginResponse.Text = "Incorrect Username or Password";
                }
                

            };

        }
        private void DisplayJsonLogin()
        {
            VerifyLoginObject login_credentials = JsonConvert.DeserializeObject<VerifyLoginObject>(jsonLogin);

            if (login_credentials.validUser)
            {
                var res = new Intent(this, typeof(MainActivity));
                //res.PutExtra("dealer_name", userInput.Text);
                Context mContext = Android.App.Application.Context;
                AppPreferences ap = new AppPreferences (mContext);
                string key = login_credentials.dealer_name;
                ap.saveUsername(key);
                StartActivity(res);
            }
        }

        private async Task<JsonValue> FetchLoginCredentialsAsync(string url)
        {
            var credentials_client = new HttpClient();

            credentials_client.DefaultRequestHeaders.Accept.Clear();
            //add any default headers below this
            credentials_client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage credentials_response = await credentials_client.GetAsync(url);

            string json_string = "";
            if (credentials_response.StatusCode == HttpStatusCode.OK)
            {
                json_string = await credentials_response.Content.ReadAsStringAsync();
               
            }
            return json_string;

            //WebRequest request = WebRequest.Create(new Uri(url));
            //request.ContentType = "application/json";
            //request.Method = "GET";

            //using (WebResponse response = await request.GetResponseAsync())
            //{
            //    using (Stream stream = response.GetResponseStream())
            //    {
            //        JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
            //        Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());

            //        return jsonDoc;
            //    }
            //}
        }

        private EditText usernameInput;
        private EditText passwordInput;
        private TextView loginResponse;
        private TextView speechResultTop;
        private TextView speechResultBottom;
        private JsonValue jsonLogin;

        private Button buttonConfirm;
        private LinearLayout buttonGroup;
        
    } 
    
}
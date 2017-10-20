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
using Android.Graphics;

namespace VDA_Android
{
    [Activity(Label = "Virtual Dealership Advisor", MainLauncher = true)]

    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            speechResult = (TextView)FindViewById(Resource.Id.speechResult);
            speechResult.Typeface = Typeface.CreateFromAsset(Assets, "Fonts/Roboto-Light.ttf");

            buttonConfirm = FindViewById<Button>(Resource.Id.confirmText);
            buttonConfirm.Visibility = ViewStates.Invisible;

            buttonConfirm.Click += delegate
            {
                var res = new Intent(this, typeof(ResultActivity));
                res.PutExtra("speechStr", speechStr);
                StartActivity(res);
            };
        }

        [Java.Interop.Export("GetSpeechInput")]
        public void GetSpeechInput(View view)
        {
            Intent intent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            intent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);

            StartActivityForResult(intent, 10);

        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if(requestCode == 10){

                // This will be reinstated with working microphone. 
                // Currently it will be set to dummy text.

                //var resultList = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);

                //speechStr = resultList[0];

                //speechResult.Text = "What you said was: \n\n" + speechStr +
                //    " \n\nIs this correct?";

                speechStr = "How can I improve my sedan sales?";

                speechResult.Text = "What you said was: \n\n" + speechStr +
                    "\n\nIs this correct?";

                buttonConfirm.Visibility = ViewStates.Visible;

                ImageView micImage = (ImageView)FindViewById(Resource.Id.btnSpeak);
                micImage.Visibility = ViewStates.Gone;
            }
        }
        private TextView speechResult;
        private Button buttonConfirm;
        private string speechStr = "";
    }
}


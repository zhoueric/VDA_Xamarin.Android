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
using VDA_Android.Models;


namespace VDA_Android
{
    [Activity(Label = "Virtual Dealership Advisor")]

    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Username
            Context mContext = Android.App.Application.Context;
            AppPreferences ap = new AppPreferences(mContext);
            string dealer_name = ap.getUsername();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            buttonGroup = FindViewById<LinearLayout>(Resource.Id.buttonWrap);

            micImage = (ImageView)FindViewById(Resource.Id.btnSpeak);

            speechResultTop = (TextView)FindViewById(Resource.Id.speechResultTop);
            speechResultTop.Typeface = Typeface.CreateFromAsset(Assets, "Fonts/Roboto-Light.ttf");

            speechResultBottom = (TextView)FindViewById(Resource.Id.speechResultBottom);
            speechResultBottom.Typeface = Typeface.CreateFromAsset(Assets, "Fonts/Roboto-Light.ttf");

            userInput = (EditText)FindViewById(Resource.Id.userInput);

            buttonConfirm = FindViewById<Button>(Resource.Id.confirmText);
            buttonTryAgain = FindViewById<Button>(Resource.Id.tryAgain);

            buttonConfirm.Click += delegate
            {
                var res = new Intent(this, typeof(ResultActivity));
                res.PutExtra("speechStr", userInput.Text);
                StartActivity(res);
            };

            buttonTryAgain.Click += delegate
            {
                buttonConfirm.Visibility = ViewStates.Gone;
                buttonTryAgain.Visibility = ViewStates.Gone;
                buttonGroup.Visibility = ViewStates.Gone;
                micImage.Visibility = ViewStates.Visible;
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

                speechStr = "How can I improve my car sales?";

                userInput.Text = speechStr;

                speechResultTop.Text = "What you said was";
                speechResultBottom.Text = "...is that correct?";

                buttonConfirm.Visibility = ViewStates.Visible;
                buttonTryAgain.Visibility = ViewStates.Visible;

                buttonGroup.Visibility = ViewStates.Visible;

                micImage.Visibility = ViewStates.Gone;
            }
        }
        private TextView speechResultTop;
        private TextView speechResultBottom;
        private EditText userInput;
        private Button buttonConfirm;
        private Button buttonTryAgain;
        private LinearLayout buttonGroup;
        ImageView micImage;
        private string speechStr = "";
    }
}


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

        }
    }
}
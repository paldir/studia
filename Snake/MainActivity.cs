using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Snake
{
    [Activity(Label = "Snake", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        Terrarium terrarium { get { return FindViewById<Terrarium>(Resource.Id.terrarium); } }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
        }
    }
}

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

namespace Snake
{
    [Activity(Label = "Snake", MainLauncher = true, Icon = "@drawable/icon")]
    public class Menu : Activity
    {
        Button start { get { return FindViewById<Button>(Resource.Id.start); } }
        Button best { get { return FindViewById<Button>(Resource.Id.best); } }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            RequestWindowFeature(WindowFeatures.NoTitle);
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            SetContentView(Resource.Layout.Menu);

            start.Click += start_Click;
            best.Click += best_Click;
        }

        void start_Click(object sender, EventArgs e)
        {
            Food.Count = 0;
            
            StartActivity(typeof(MainActivity));
        }

        void best_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(BestScores));
        }
    }
}
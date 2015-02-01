using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using System.Threading.Tasks;

namespace Snake
{
    [Activity(Label = "Snake", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        Terrarium terrarium { get { return FindViewById<Terrarium>(Resource.Id.terrarium); } }
        Button up { get { return FindViewById<Button>(Resource.Id.up); } }
        Button down { get { return FindViewById<Button>(Resource.Id.down); } }
        Button left { get { return FindViewById<Button>(Resource.Id.left); } }
        Button right { get { return FindViewById<Button>(Resource.Id.right); } }
        Viper viper;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            RequestWindowFeature(WindowFeatures.NoTitle);
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;

            SetContentView(Resource.Layout.Main);

            viper = terrarium.Viper;
            terrarium.Touch += terrarium_Touch;
            up.Click += up_Click;
            down.Click += down_Click;
            left.Click += left_Click;
            right.Click += right_Click;
        }

        void up_Click(object sender, EventArgs e)
        {
            if (viper.CurrentCrawlingDirection != CrawlingDirection.Down)
                viper.CurrentCrawlingDirection = CrawlingDirection.Up;
        }

        void down_Click(object sender, EventArgs e)
        {
            if (viper.CurrentCrawlingDirection != CrawlingDirection.Up)
                viper.CurrentCrawlingDirection = CrawlingDirection.Down;
        }

        void left_Click(object sender, EventArgs e)
        {
            if (viper.CurrentCrawlingDirection != CrawlingDirection.Right)
                viper.CurrentCrawlingDirection = CrawlingDirection.Left;
        }

        void right_Click(object sender, EventArgs e)
        {
            if (viper.CurrentCrawlingDirection != CrawlingDirection.Left)
                viper.CurrentCrawlingDirection = CrawlingDirection.Right;
        }

        void terrarium_Touch(object sender, View.TouchEventArgs e)
        {
            float x = e.Event.GetX();
            float y = e.Event.GetY();
            int width = terrarium.Width;

            if (y < x)
            {
                if (y < width - x)
                    up_Click(null, null);
                else
                    right_Click(null, null);
            }
            else
            {
                if (y < width - x)
                    left_Click(null, null);
                else
                    down_Click(null, null);
            }
        }
    }
}
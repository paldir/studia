using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using System.Threading.Tasks;
using System.Linq;

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
        TextView points { get { return FindViewById<TextView>(Resource.Id.points); } }
        Viper viper;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            RequestWindowFeature(WindowFeatures.NoTitle);
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;

            SetContentView(Resource.Layout.Main);

            viper = terrarium.Viper;
            points.Text = (0).ToString("D4");
            terrarium.Touch += terrarium_Touch;
            terrarium.DinnerConsumed += terrarium_DinnerConsumed;
            up.Click += up_Click;
            down.Click += down_Click;
            left.Click += left_Click;
            right.Click += right_Click;
        }

        void up_Click(object sender, EventArgs e)
        {
            if (viper.CurrentCrawlingDirection.First() != CrawlingDirection.Down)
                viper.CurrentCrawlingDirection.Enqueue(CrawlingDirection.Up);
        }

        void down_Click(object sender, EventArgs e)
        {
            if (viper.CurrentCrawlingDirection.First() != CrawlingDirection.Up)
                viper.CurrentCrawlingDirection.Enqueue(CrawlingDirection.Down);
        }

        void left_Click(object sender, EventArgs e)
        {
            if (viper.CurrentCrawlingDirection.First() != CrawlingDirection.Right)
                viper.CurrentCrawlingDirection.Enqueue(CrawlingDirection.Left);
        }

        void right_Click(object sender, EventArgs e)
        {
            if (viper.CurrentCrawlingDirection.First() != CrawlingDirection.Left)
                viper.CurrentCrawlingDirection.Enqueue(CrawlingDirection.Right);
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

        void terrarium_DinnerConsumed(object sender, EventArgs e)
        {
            points.Text = (Convert.ToInt16(points.Text) + 1).ToString("D4");
        }
    }

}
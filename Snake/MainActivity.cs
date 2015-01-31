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

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            RequestWindowFeature(WindowFeatures.NoTitle);
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

            RequestedOrientation = Android.Content.PM.ScreenOrientation.Landscape;

            SetContentView(Resource.Layout.Main);

            terrarium.Touch += terrarium_Touch;
        }

        void terrarium_Touch(object sender, View.TouchEventArgs e)
        {
            float x = e.Event.GetX();
            float y = e.Event.GetY();
            int width = terrarium.Width;
            Viper viper = terrarium.Viper;
            CrawlingDirection currentCrawlingDirection = viper.CurrentCrawlingDirection;

            if (y < x)
            {
                if (y < width - x)
                {
                    if (currentCrawlingDirection != CrawlingDirection.Down)
                        viper.CurrentCrawlingDirection = CrawlingDirection.Up;
                }
                else
                    if (currentCrawlingDirection != CrawlingDirection.Left)
                        viper.CurrentCrawlingDirection = CrawlingDirection.Right;
            }
            else
            {
                if (y < width - x)
                {
                    if (currentCrawlingDirection != CrawlingDirection.Right)
                        viper.CurrentCrawlingDirection = CrawlingDirection.Left;
                }
                else
                    if (currentCrawlingDirection != CrawlingDirection.Up)
                        viper.CurrentCrawlingDirection = CrawlingDirection.Down;
            }
        }
    }
}
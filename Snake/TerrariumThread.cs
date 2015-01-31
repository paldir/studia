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
    class TerrariumThread
    {
        ISurfaceHolder surfaceHolder;
        Terrarium terrarium;

        public bool Running { get; set; }

        public TerrariumThread(ISurfaceHolder surfaceHolder, Terrarium terrarium)
        {
            this.surfaceHolder = surfaceHolder;
            this.terrarium = terrarium;
        }

        async public void Start()
        {
            while (Running)
            {
                await System.Threading.Tasks.Task.Delay(250);
                
                Android.Graphics.Canvas canvas = surfaceHolder.LockCanvas();

                if (canvas != null)
                {
                    terrarium.DrawEnvironment(canvas);
                    surfaceHolder.UnlockCanvasAndPost(canvas);
                }

                terrarium.Viper.LetCrawl();
            }
        }
    }
}
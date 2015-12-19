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

using Android.Graphics;

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
            Random random = new Random();

            while (Running)
            {
                await System.Threading.Tasks.Task.Delay(125);

                List<Point> viperCoordinates = terrarium.Viper.GetCoordinates();
                Point viperHead = viperCoordinates.First();
                Food dinner = terrarium.Food.Find(f => f.X == viperHead.X && f.Y == viperHead.Y);
                Canvas canvas = surfaceHolder.LockCanvas();

                if (canvas != null)
                {
                    terrarium.DrawEnvironment(canvas);
                    surfaceHolder.UnlockCanvasAndPost(canvas);
                }

                if (Food.Count < 1)
                {
                    Food food = new Food(random.Next(0, Terrarium.SideLengthInCells - 1), random.Next(0, Terrarium.SideLengthInCells - 1));

                    if (!viperCoordinates.Exists(c => c.X == food.X && c.Y == food.Y) && !terrarium.Food.Exists(f => f.X == food.X && f.Y == food.Y))
                        terrarium.Food.Add(food);
                    else
                        Food.Count--;
                }

                if (dinner != null)
                {
                    Food.Count--;
                    
                    terrarium.Food.Remove(dinner);
                    terrarium.Viper.LetExtend();
                    terrarium.OnDinnerConsumed(new EventArgs());
                }

                terrarium.Viper.LetCrawl();
            }
        }
    }
}
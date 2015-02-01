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
                Android.Graphics.Canvas canvas = surfaceHolder.LockCanvas();

                if (canvas != null)
                {
                    terrarium.DrawEnvironment(canvas);
                    surfaceHolder.UnlockCanvasAndPost(canvas);
                }

                if ((Food.Count < 1 && random.NextDouble() > 0.90) || (Food.Count < 2 && random.NextDouble() > 0.99))
                {
                    Food food = new Food(random.Next(0, Terrarium.SideLengthInCells - 1), random.Next(0, Terrarium.SideLengthInCells - 1));

                    if (!viperCoordinates.Exists(p => p.X == food.X && p.Y == food.Y) && !terrarium.Food.Exists(p => p.X == food.X && p.Y == food.Y))
                        terrarium.Food.Add(food);
                }

                if (dinner != null)
                {
                    terrarium.Food.Remove(dinner);
                    dinner.Dispose();
                    terrarium.Viper.LetExtend();
                }

                terrarium.Viper.LetCrawl();
            }
        }
    }
}
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
    class Viper
    {
        public List<Android.Graphics.Point> Coordinates { get; private set; }

        public Viper()
        {
            Coordinates = new List<Android.Graphics.Point>();

            /*for (int i = 23; i <= 27; i++)
                Coordinates.Add(new Android.Graphics.Point(i, 25));*/

            for (int i = 0; i < 50; i++)
                for (int j = 0; j < 50; j++)
                    Coordinates.Add(new Android.Graphics.Point(i, j));
        }

        public void Extend()
        {
            throw new NotImplementedException("Roœniêcie ¿mii nie zaimplementowane.");
        }
    }
}
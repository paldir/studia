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
    public class Food
    {
        Point point;

        public int X
        {
            get { return point.X; }
            private set { point.X = value; }
        }

        public int Y
        {
            get { return point.Y; }
            private set { point.Y = value; }
        }

        public Point GetPoint() { return new Point(point); }

        public static int Count { get; set; }

        public Food(int x, int y)
        {
            point = new Point();
            X = x;
            Y = y;
            Count++;
        }
    }
}
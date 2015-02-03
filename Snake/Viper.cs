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
    public class Viper
    {
        public Queue<CrawlingDirection> CurrentCrawlingDirection { get; set; }

        List<Point> coordinates;
        public List<Point> GetCoordinates() { return new List<Point>(coordinates); }

        public Viper()
        {
            coordinates = new List<Android.Graphics.Point>();
            CurrentCrawlingDirection = new Queue<CrawlingDirection>(new[] { CrawlingDirection.Left });
            int terrariumHalf = Terrarium.SideLengthInCells / 2;

            for (int i = terrariumHalf - 2; i <= terrariumHalf + 2; i++)
                coordinates.Add(new Android.Graphics.Point(i, terrariumHalf));
        }

        public void LetCrawl()
        {
            Point currentHead = coordinates.First();
            Point newHead = null;

            coordinates.RemoveAt(coordinates.Count - 1);

            switch (CurrentCrawlingDirection.First())
            {
                case CrawlingDirection.Up:
                    newHead = new Point(currentHead.X, currentHead.Y - 1);

                    break;

                case CrawlingDirection.Down:
                    newHead = new Point(currentHead.X, currentHead.Y + 1);

                    break;

                case CrawlingDirection.Left:
                    newHead = new Point(currentHead.X - 1, currentHead.Y);

                    break;

                case CrawlingDirection.Right:
                    newHead = new Point(currentHead.X + 1, currentHead.Y);

                    break;
            }

            if (CurrentCrawlingDirection.Count > 1)
                CurrentCrawlingDirection.Dequeue();

            if (newHead.X < 0 || newHead.X >= Terrarium.SideLengthInCells || newHead.Y < 0 || newHead.Y >= Terrarium.SideLengthInCells)
                throw new Exception("W¹¿ przypieprzy³ ³bem w œcianê!");

            if (coordinates.Exists(c => Point.Equals(c, newHead)))
                throw new Exception("W¹¿ przypieprzy³ ³bem w swoj¹ dupê!");

            coordinates.Insert(0, newHead);
        }

        public void LetExtend()
        {
            Point tail = coordinates.Last();
            Point almostTail = coordinates.ElementAt(coordinates.Count - 2);
            int x = tail.X - almostTail.X;
            int y = tail.Y - almostTail.Y;

            coordinates.Add(new Point(tail.X + x, tail.Y + y));
        }
    }
}
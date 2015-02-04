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
        const CrawlingDirection defaultCrawlingDirection = CrawlingDirection.Left;
        Terrarium terrarium;
        CrawlingDirection previousCrawlingDirection;

        public Queue<CrawlingDirection> CurrentCrawlingDirection { get; set; }

        List<Point> coordinates;
        public List<Point> GetCoordinates() { return new List<Point>(coordinates); }

        public Viper(Terrarium terrarium)
        {
            this.terrarium = terrarium;
            previousCrawlingDirection = defaultCrawlingDirection;
            coordinates = new List<Android.Graphics.Point>();
            CurrentCrawlingDirection = new Queue<CrawlingDirection>(new[] { defaultCrawlingDirection });
            int terrariumHalf = Terrarium.SideLengthInCells / 2;

            for (int i = terrariumHalf - 2; i <= terrariumHalf + 2; i++)
                coordinates.Add(new Android.Graphics.Point(i, terrariumHalf));
        }

        public void LetCrawl()
        {
            Point currentHead = coordinates.First();
            Point newHead = null;
            CrawlingDirection currentCrawlingDirection = CurrentCrawlingDirection.First();

            coordinates.RemoveAt(coordinates.Count - 1);

            switch (currentCrawlingDirection)
            {
                case CrawlingDirection.Up:
                    if (previousCrawlingDirection == CrawlingDirection.Down)
                        currentCrawlingDirection = previousCrawlingDirection;

                    break;

                case CrawlingDirection.Down:
                    if (previousCrawlingDirection == CrawlingDirection.Up)
                        currentCrawlingDirection = previousCrawlingDirection;

                    break;

                case CrawlingDirection.Left:
                    if (previousCrawlingDirection == CrawlingDirection.Right)
                        currentCrawlingDirection = previousCrawlingDirection;

                    break;

                case CrawlingDirection.Right:
                    if (previousCrawlingDirection == CrawlingDirection.Left)
                        currentCrawlingDirection = previousCrawlingDirection;

                    break;
            }
            
            switch (currentCrawlingDirection)
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

            previousCrawlingDirection = currentCrawlingDirection;

            if (CurrentCrawlingDirection.Count > 1)
                CurrentCrawlingDirection.Dequeue();

            if (newHead.X < 0 || newHead.X >= Terrarium.SideLengthInCells || newHead.Y < 0 || newHead.Y >= Terrarium.SideLengthInCells)
                terrarium.OnViperDead(new EventArgs());

            if (coordinates.Exists(c => c.X == newHead.X && c.Y == newHead.Y))
                terrarium.OnViperDead(new EventArgs());

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
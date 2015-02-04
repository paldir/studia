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
    class Score
    {
        public DateTime Date { get; set; }
        public int Points { get; set; }

        public Score(DateTime date, int points)
        {
            Date = date;
            Points = points;
        }
    }
}
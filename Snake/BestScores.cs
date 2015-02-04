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

using System.IO;
using Android.Graphics;

namespace Snake
{
    [Activity(Label = "BestScores")]
    public class BestScores : Activity
    {
        TableLayout table { get { return FindViewById<TableLayout>(Resource.Id.table); } }
        Button menu { get { return FindViewById<Button>(Resource.Id.menu); } }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            RequestWindowFeature(WindowFeatures.NoTitle);
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            SetContentView(Resource.Layout.BestScores);

            List<Score> scores = new List<Score>();
            int newPoints;
            DateTime now = DateTime.Now;

            try { newPoints = Convert.ToInt16(Intent.Extras.GetString("points", "-1")); }
            catch (NullReferenceException) { newPoints = -1; }

            try
            {
                using (StreamReader streamReader = new StreamReader(OpenFileInput("bestScores.txt")))
                {
                    while (!streamReader.EndOfStream)
                    {
                        string[] row = streamReader.ReadLine().Split(new char[] { ';' });

                        scores.Add(new Score(Convert.ToDateTime(row[0]), Convert.ToInt16(row[1])));
                    }
                }
            }
            catch (Java.IO.FileNotFoundException) { }

            if (newPoints != -1)
            {
                scores.Add(new Score(now, newPoints));

                scores = scores.OrderByDescending(s => s.Points).ToList();
                scores = scores.Take(10).ToList();

                using (StreamWriter streamWriter = new StreamWriter(OpenFileOutput("bestScores.txt", FileCreationMode.Private)))
                {
                    foreach (Score score in scores)
                        streamWriter.WriteLine(score.Date.ToString() + "; " + score.Points.ToString());
                }
            }
            else
                scores = scores.OrderByDescending(s => s.Points).ToList();

            for (int i = -1; i < scores.Count; i++)
            {
                TableRow row = new TableRow(this);
                row.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                string[] rowArray;
                Color textColor = Color.Black;

                if (i == -1)
                {
                    rowArray = new string[] { "Date", "Score" };

                    row.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.HeaderRow));
                }
                else
                {
                    Score score = scores.ElementAt(i);
                    rowArray = new string[] { score.Date.ToString(), score.Points.ToString("D4") };

                    if (newPoints != -1 && score.Date == now)
                    {
                        textColor = Resources.GetColor(Resource.Color.TerrariumBackground);

                        row.SetBackgroundColor(Color.Black);
                    }
                    else
                        row.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.Row));
                }

                for (int j = 0; j < 2; j++)
                {
                    TextView cell = new TextView(this);
                    cell.Text = rowArray[j];

                    cell.SetTypeface(Typeface.Monospace, TypefaceStyle.Normal);
                    cell.SetTextColor(textColor);
                    cell.SetPadding(20, 5, 20, 5);

                    if (i == -1)
                        cell.SetTypeface(Typeface.Monospace, TypefaceStyle.Bold);

                    row.AddView(cell);
                }

                table.AddView(row, new TableLayout.LayoutParams(TableLayout.LayoutParams.MatchParent, TableLayout.LayoutParams.WrapContent));
            }

            menu.Click += menu_Click;
        }

        void menu_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(Menu));
        }
    }
}
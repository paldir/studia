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
    [Activity(Label = "BestScores")]
    public class BestScores : Activity
    {
        TableLayout table { get { return FindViewById<TableLayout>(Resource.Id.table); } }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            RequestWindowFeature(WindowFeatures.NoTitle);
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            SetContentView(Resource.Layout.BestScores);

            for (int i = 0; i < 10; i++)
            {
                TableRow row = new TableRow(this);
                row.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);

                for (int j = 0; j < 5; j++)
                {
                    TextView cell = new TextView(this);
                    cell.Text = i.ToString() + ';' + j.ToString();

                    cell.SetTextColor(Android.Graphics.Color.Black);
                    cell.SetPadding(5, 5, 5, 5);
                    row.AddView(cell);
                }

                table.AddView(row, new TableLayout.LayoutParams(TableLayout.LayoutParams.MatchParent, TableLayout.LayoutParams.WrapContent));
            }
        }
    }
}
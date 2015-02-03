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
    public class Terrarium : SurfaceView, ISurfaceHolderCallback
    {
        int border;
        int sideLength;
        float cellLength;
        Paint viperPaint;
        Paint backgroundPaint;
        Paint borderPaint;
        TerrariumThread terrariumThread;

        public const int SideLengthInCells = 30;
        public Viper Viper { get; private set; }
        public List<Food> Food { get; private set; }

        public Terrarium(Context context, Android.Util.IAttributeSet attrs)
            : base(context, attrs)
        {
            Viper = new Viper();
            Food = new List<Snake.Food>();
            border = 2;
            viperPaint = new Paint();
            viperPaint.Color = Color.Black;
            backgroundPaint = new Paint();
            backgroundPaint.Color = Resources.GetColor(Resource.Color.TerrariumBackground);
            borderPaint = new Paint();
            borderPaint.Color = Color.Black;
            borderPaint.StrokeWidth = border;

            borderPaint.SetStyle(Paint.Style.Stroke);
            Holder.AddCallback(this);
        }

        public void DrawEnvironment(Android.Graphics.Canvas canvas)
        {
            canvas.DrawRect(0, 0, canvas.Width - 1, canvas.Height - 1, backgroundPaint);
            canvas.DrawRect(0, 0, canvas.Width - 1, canvas.Height - 1, borderPaint);

            foreach (Point point in Viper.GetCoordinates().Concat(Food.Select(f => f.GetPoint())))
            {
                float x = point.X * cellLength + border + 1;
                float y = point.Y * cellLength + border + 1;

                canvas.DrawRect(x, y, x + cellLength - 1, y + cellLength - 1, viperPaint);
            }
        }

        public void SurfaceChanged(ISurfaceHolder holder, Format format, int width, int height) { }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            terrariumThread = new TerrariumThread(holder, this);
            terrariumThread.Running = true;

            terrariumThread.Start();
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            terrariumThread.Running = false;
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            int width = MeasureSpec.GetSize(widthMeasureSpec);
            int height = MeasureSpec.GetSize(heightMeasureSpec);
            sideLength = Math.Min(width, height);

            SetMeasuredDimension(sideLength, sideLength);

            sideLength -= border * 4;
            cellLength = (float)sideLength / SideLengthInCells;
        }

        public delegate void DinnerConsumedEventHandler(object sender, EventArgs e);
        public event DinnerConsumedEventHandler DinnerConsumed;

        public void OnDinnerConsumed(EventArgs e)
        {
            if (DinnerConsumed != null)
                DinnerConsumed(this, e);
        }
    }
}
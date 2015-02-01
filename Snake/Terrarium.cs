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
        int sideLength;
        int border = 1;
        Paint viperPaint;
        Paint backgroundPaint;
        Paint borderPaint;
        float cellLength;
        TerrariumThread terrariumThread;

        public const int SideLengthInCells = 30;

        public Viper Viper { get; private set; }
        public List<Food> Food { get; private set; }

        public Terrarium(Context context, Android.Util.IAttributeSet attrs)
            : base(context, attrs)
        {
            Viper = new Viper();
            Food = new List<Snake.Food>();
            Color backgroundColor = Resources.GetColor(Resource.Color.TerrariumBackground);
            viperPaint = new Paint();
            viperPaint.Color = Color.Black;
            backgroundPaint = new Paint();
            backgroundPaint.Color = backgroundColor;
            borderPaint = new Paint();
            borderPaint.Color = Color.Black;
            borderPaint.StrokeWidth = border;

            borderPaint.SetStyle(Paint.Style.Stroke);

            Holder.AddCallback(this);
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            int width = MeasureSpec.GetSize(widthMeasureSpec);
            int height = MeasureSpec.GetSize(heightMeasureSpec);
            sideLength = Math.Min(width, height);
            cellLength = (float)sideLength / SideLengthInCells;

            SetMeasuredDimension(sideLength, sideLength);
        }

        public void DrawEnvironment(Android.Graphics.Canvas canvas)
        {
            sideLength -= border * 2;

            canvas.DrawRect(0, 0, canvas.Width - 1, canvas.Height - 1, backgroundPaint);
            canvas.DrawRect(0, 0, canvas.Width - 1, canvas.Height - 1, borderPaint);

            foreach (Point point in Viper.GetCoordinates().Concat(Food.Select(f => f.GetPoint())))
            {
                float x = point.X * cellLength + border + 1;
                float y = point.Y * cellLength + border + 1;

                canvas.DrawRect(x, y, x + cellLength - 1, y + cellLength - 1, viperPaint);
            }
        }

        public void SurfaceChanged(ISurfaceHolder holder, Format format, int width, int height){}

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            terrariumThread = new TerrariumThread(holder, this);
            terrariumThread.Running = true;

            terrariumThread.Start();
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            terrariumThread.Running = false;

            viperPaint.Dispose();
            backgroundPaint.Dispose();
            borderPaint.Dispose();
        }
    }
}
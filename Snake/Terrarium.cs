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
        Paint paint;
        float cellLength;
        TerrariumThread terrariumThread;
        Color backgroundColor;

        public const int SideLengthInCells = 50;
        
        public Viper Viper { get; private set; }

        public Terrarium(Context context, Android.Util.IAttributeSet attrs)
            : base(context, attrs)
        {
            Viper = new Viper();
            paint = new Paint();
            backgroundColor = Resources.GetColor(Resource.Color.TerrariumBackground);

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

        protected override void OnDraw(Canvas canvas)
        {
            paint.Color = Resources.GetColor(Resource.Color.TerrariumBackground);

            canvas.DrawRect(0, 0, canvas.Width, canvas.Height, paint);
        }

        public void DrawEnvironment(Android.Graphics.Canvas canvas)
        {
            sideLength -= border * 2;
            paint.Color = backgroundColor;

            canvas.DrawRect(0, 0, canvas.Width - 1, canvas.Height - 1, paint);

            paint.Color = Color.Black;
            paint.StrokeWidth = 1;

            paint.SetStyle(Paint.Style.Stroke);

            canvas.DrawRect(0, 0, canvas.Width - 1, canvas.Height - 1, paint);

            paint.SetStyle(Paint.Style.Fill);

            var tmp = Viper.GetCoordinates();

            foreach (Point point in tmp)
            {
                float x = point.X * cellLength + border;
                float y = point.Y * cellLength + border;

                canvas.DrawRect(x, y, x + cellLength, y + cellLength, paint);
            }

            /*paint.Color = Resources.GetColor(Resource.Color.TerrariumBackground);

            for (int i = 0; i < SideLengthInCells - 1; i++)
            {
                float x = (i + 1) * cellLength + terrariumBorder;

                canvas.DrawLine(x, terrariumBorder, x, sideLength - terrariumBorder, paint);
            }

            for (int i = 0; i < SideLengthInCells - 1; i++)
            {
                float y = (i + 1) * cellLength + terrariumBorder;

                canvas.DrawLine(terrariumBorder, y, sideLength - terrariumBorder, y, paint);
            }*/
        }

        public void SurfaceChanged(ISurfaceHolder holder, Format format, int width, int height)
        {
            //throw new NotImplementedException();
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            terrariumThread = new TerrariumThread(holder, this);
            terrariumThread.Running = true;

            terrariumThread.Start();
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            terrariumThread.Running = false;

            paint.Dispose();
        }
    }
}
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
    public class Terrarium : View
    {
        int sideLength;
        Viper viper = new Viper();

        public Terrarium(Context context) : base(context) { }
        public Terrarium(Context context, Android.Util.IAttributeSet attrs) : base(context, attrs) { }
        public Terrarium(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }
        public Terrarium(Context context, Android.Util.IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle) { }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            int width = MeasureSpec.GetSize(widthMeasureSpec);
            int height = MeasureSpec.GetSize(heightMeasureSpec);
            sideLength = Math.Min(width, height);

            SetMeasuredDimension(sideLength, sideLength);
        }

        protected override void OnDraw(Android.Graphics.Canvas canvas)
        {
            int terrariumBorder = Resources.GetDimensionPixelSize(Resource.Dimension.TerrariumBorder) + 1;
            sideLength -= terrariumBorder * 2;
            Paint paint = new Paint();
            float cellLength = sideLength / 50f;
            paint.Color = Color.Black;

            foreach (Point point in viper.Coordinates)
            {
                float x = point.X * cellLength + terrariumBorder;
                float y = point.Y * cellLength + terrariumBorder;

                canvas.DrawRect(x, y, x + cellLength, y + cellLength, paint);
            }

            paint.Color = Resources.GetColor(Resource.Color.TerrariumBackground);

            for (int i = 0; i < 50; i++)
            {
                float x = (i + 1) * cellLength;

                canvas.DrawLine(x, terrariumBorder, x, sideLength - terrariumBorder, paint);
            }

            for (int i = 0; i < 50; i++)
            {
                float y = (i + 1) * cellLength;

                canvas.DrawLine(terrariumBorder, y, sideLength - terrariumBorder, y, paint);
            }
        }
    }
}
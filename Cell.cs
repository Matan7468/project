﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace experience
{
    class Cell
    {
        public static float CellWidth { get; set; }
        public static float CellHeight { get; set; }


        public int Row { get; set; }
        public int Col { get; set; }

        public float x { get; set; }
        public float y { get; set; }
        public float width { get; set; }
        public float Height { get; set; }
        public enum Type
        {
            nothing,
            empty,
            userGreen,
            botBlue,
            botRed,
        }
    public int num { get; set; }

        bool Done;

        public Cell()
        {

        }

        public Cell(float x, float y, float width, float height, int num, int row, int col)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.Height = height;
            CellHeight = height;
            CellWidth = width;
            this.num = num;
            Done = false;

            this.Row = row;
            this.Col = col;
        }

        public void DrawCell(Canvas canvas)
        {
            Paint ccell = new Paint();
            switch (num)
            {
                case (int)Type.nothing:
                    ccell.Color = Color.WhiteSmoke;
                    break;
                case (int)Type.empty:
                    ccell.Color = Color.LightGray;
                    break;
                case (int)Type.userGreen:
                    ccell.Color = Color.Green;
                    break;
                case (int)Type.botBlue:
                    ccell.Color = Color.Blue;
                    break;
                case (int)Type.botRed:
                    ccell.Color = Color.Red;
                    break;
            }

            Rect r = new Rect();
            r.Set((int)x, (int)y, (int)(x + width), (int)(y + Height));
            canvas.DrawRect(r, ccell);

        }

        public bool DidUserTouchedMe(float x, float y)
        {
            if (this.x < x && this.x + this.width > x && this.y < y && this.y + this.Height > y)
                return true;
            return false;
        }

        public int GetColor()
        {
            return this.num;
        }

        public void ChangeType(int num)
        {
            this.num = num;
        }

        public void SetDone(bool done)
        {
            Done = done;
        }

        public bool GetDone()
        {
            return Done;
        }

        public float Getx()
        {
            return x;
        }

        public float Gety()
        {
            return y
;
        }

        public float GetH()
        {
            return Height;
        }

        public float Getw()
        {
            return width;
        }
    }
}

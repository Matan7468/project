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

namespace experience
{
    class AllLevels
    {
        Activity activity;
        int[,] change;
        int[,] Hint;
        List<GameBoard> levels;
        public GameBoard level1;
        public GameBoard level2;
        public GameBoard level3;
        public GameBoard level4;
        public TextView tv { get; set; }

        public AllLevels(Activity activity, TextView tv)
        {
            levels = new List<GameBoard>();
            this.activity = activity;
            this.tv = tv;
            CreateLevel1();
            CreateLevel2();
            Createlevel3();
            Createlevel4();
        }

        public AllLevels()
        {
            levels = new List<GameBoard>();
            CreateLevel1();
            CreateLevel2();
            Createlevel3();
            Createlevel4();
        }

        private void CreateLevel1()
        {
            change = new int[20, 20];
            Hint = new int[20, 20];
            for (int i = 0; i < 20; i++)//יצירת שוליים
            {
                change[i, 0] = 0;
                change[i, 19] = 0;
                change[0, i] = 0;
                change[19, i] = 0;
                Hint[i, 0] = 0;
                Hint[i, 19] = 0;
                Hint[0, i] = 0;
                Hint[19, i] = 0;
            }
            for (int i = 1; i < 19; i++)//הגדרת  כל השטח הנותר כפנוי
            {
                for (int j = 1; j < 19; j++)
                {
                    change[i, j] = 1;
                    Hint[i, j] = 1;
                }
            }
            for (int i = 7; i < 15; i++)
            {
                change[10, i] = 0;
            }
            Hint[9, 7] = 2;
            change[5, 7] = 3;
            level1 = new GameBoard(activity, change, 1, tv, 1, Hint);
            levels.Add(level1);
        }

        private void CreateLevel2()
        {
            change = new int[20, 20];
            Hint = new int[20, 20];
            for (int i = 0; i < 20; i++)//יצירת שוליים
            {
                change[i, 0] = 0;
                change[i, 19] = 0;
                change[0, i] = 0;
                change[19, i] = 0;
                Hint[i, 0] = 0;
                Hint[i, 19] = 0;
                Hint[0, i] = 0;
                Hint[19, i] = 0;
            }
            for (int i = 1; i < 19; i++)//הגדרת  כל השטח הנותר כפנוי
            {
                for (int j = 1; j < 19; j++)
                {
                    change[i, j] = 1;
                    Hint[i, j] = 1;
                }
            }
            for (int i = 5; i < 14; i++)
            {
                for (int j = 10; j < 19; j++)
                {
                    change[j, i] = 0;
                }
            }
            Hint[8, 11] = 2;
            change[2, 5] = 3;
            level2 = new GameBoard(activity, change, 1, tv, 2, Hint);
            levels.Add(level2);
        }

        private void Createlevel3()
        {
            change = new int[20, 20];
            Hint = new int[20, 20];
            for (int i = 0; i < 20; i++)//יצירת שוליים
            {
                change[i, 0] = 0;
                change[i, 19] = 0;
                change[0, i] = 0;
                change[19, i] = 0;
                Hint[i, 0] = 0;
                Hint[i, 19] = 0;
                Hint[0, i] = 0;
                Hint[19, i] = 0;
            }
            for (int i = 1; i < 19; i++)//הגדרת  כל השטח הנותר כפנוי
            {
                for (int j = 1; j < 19; j++)
                {
                    change[i, j] = 1;
                    Hint[i, j] = 1;
                }
            }
            for (int i = 7; i < 12; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    change[i, j] = 0;
                }
                for (int j = 7; j < 11; j++)
                {
                    change[i, j] = 0;
                }
                for (int j = 14; j < 19; j++)
                {
                    change[i, j] = 0;
                }
            }
            Hint[6, 7] = 2;
            Hint[15, 15] = 2;
            change[1, 2] = 4;
            change[15, 7] = 3;
            level3 = new GameBoard(activity, change, 2, tv, 3, Hint);
            levels.Add(level3);
        }

        private void Createlevel4()
        {
            change = new int[20, 20];
            Hint = new int[20, 20];
            for (int i = 0; i < 20; i++)//יצירת שוליים
            {
                change[i, 0] = 0;
                change[i, 19] = 0;
                change[0, i] = 0;
                change[19, i] = 0;
                Hint[i, 0] = 0;
                Hint[i, 19] = 0;
                Hint[0, i] = 0;
                Hint[19, i] = 0;
            }
            for (int i = 1; i < 19; i++)//הגדרת  כל השטח הנותר כפנוי
            {
                for (int j = 1; j < 19; j++)
                {
                    change[i, j] = 1;
                    Hint[i, j] = 1;
                }
            }
            change[1, 1] = 3;
            Hint[10, 10] = 2;
            change[18, 18] = 4;
            level4 = new GameBoard(activity, change, 1, tv, 4, Hint);
            levels.Add(level4);
        }

        public List<GameBoard> GetLevels()
        {
            return levels;
        }
    }
}
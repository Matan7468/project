using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace experience
{
    class GameBoard : SurfaceView
    {
        public const int NUM_CELLS = 20;

        public bool threadRunning = true;
        public bool isRunning = true;

        Cell[,] cell;
        int[,] Change;
        int turns;
        int turnsLeft;
        public int emptyCounter { get; set; }
        public int players { get; set; }
        public int HowMuchEmpty { get; set; }
        public bool IsFull { get; set; }
        TextView tv;
        public const int fps = 225;
        public bool Done { get; set; }
        MainActivity activity;
        public bool playing { get; set; }
        public int id { get; set; }
        public int[,] Hint { get; set; }
        public bool Changed { get; set; }
        Thread hint;

        public Thread t { get; set; }

        public GameBoard(Activity activity, int[,] change, int turns, TextView tv, int id, int[,] Hint)
            : base(activity)
        {
            try
            {
                this.activity = (MainActivity)activity;
            }
            catch
            {
            }
            t = new Thread(Run);
            this.Changed = false;
            this.Hint = Hint;
            this.id = id;
            this.tv = tv;
            this.turns = turns;
            this.turnsLeft = turns;
            cell = new Cell[NUM_CELLS, NUM_CELLS];
            Change = new int[NUM_CELLS, NUM_CELLS];
            for (int i = 0; i < NUM_CELLS; i++)
            {
                for (int j = 0; j < NUM_CELLS; j++)
                {
                    Change[i, j] = change[i, j];
                    if (change[i, j] != 0)
                        this.emptyCounter++;
                }
            }
            playing = false;
            this.HowMuchEmpty = this.emptyCounter;
            for (int i = 0; i < NUM_CELLS; i++)
            {
                for (int j = 0; j < NUM_CELLS; j++)
                {
                    Change[i, j] = change[i, j];
                    if (change[i, j] == 3 || change[i, j] == 4)
                        this.players++;
                }
            }
            Create();
            t.Start();
        }
        public void Run()
        {
            while (threadRunning)
            {
                if (Holder.Surface.IsValid)
                {
                    Canvas canvas = this.Holder.LockCanvas();
                    Draw(canvas);
                    this.Holder.UnlockCanvasAndPost(canvas);

                }
            }
        }
        public override void Draw(Canvas canvas)
        {
            Paint b = new Paint()
            {
                Color = Color.Black
            };
            b.Alpha = 70;
            for (int i = 0; i < NUM_CELLS; i++)
            {
                for (int j = 0; j < NUM_CELLS; j++)
                {
                    GetCells()[i, j].DrawCell(canvas);
                }
            }
            for (int i = 1; i < NUM_CELLS; i++)
            {
                for (int j = 1; j < NUM_CELLS; j++)
                {
                    if (cell[i, j].GetColor() != 0)
                    {
                        canvas.DrawLine(Cell.CellWidth * j, Cell.CellHeight * i, Cell.CellWidth * j, Cell.CellHeight * (i + 1), b);//קווים לאורך
                        canvas.DrawLine(Cell.CellWidth * j, Cell.CellHeight * i, Cell.CellWidth * (j + 1), Cell.CellHeight * i, b);//קווים לרוחב
                    }
                }
            }
        }

        public int[,] GetChange()
        {
            return Change;
        }

        public void Create()//ליצור את המערך הדו מימדי
        {
            float cellx = 0;
            float celly = 0;
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    cell[i, j] = new Cell(cellx, celly, Cell.CellWidth, Cell.CellHeight, this.Change[i, j], i, j);
                    cellx += Cell.CellWidth;
                }
                cellx = 0;
                celly += Cell.CellHeight;
            }
        }

        public Cell[,] GetCells()
        {
            return cell;
        }

        public bool TurnesLeft()
        {
            if (turns > 0)
                return true;
            return false;
        }

        public int GetTurns()
        {
            return turnsLeft;
        }

        public bool IsDone()
        {
            for (int i = 1; i < 19; i++)
            {
                for (int j = 1; j < 19; j++)
                {
                    if (cell[i, j].GetColor() == (int)Cell.Type.nothing)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (e.Action == MotionEventActions.Down)
            {
                if (this.turns > 0)
                {
                    for (int i = 0; i < NUM_CELLS; i++)
                    {
                        for (int j = 0; j < NUM_CELLS; j++)
                        {
                            if (cell[i, j].GetColor() == (int)Cell.Type.empty)
                            {
                                if (cell[i, j].DidUserTouchedMe(e.GetX(), e.GetY()))
                                {
                                    cell[i, j].ChangeType((int)Cell.Type.userGreen);
                                    turns--;
                                    HowMuchEmpty--;
                                    Changed = true;
                                    tv.Text = turns.ToString();
                                    if (turns == 0)
                                    {
                                        playing = true;
                                        StartGame();

                                    }
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }
        public void Restart()
        {
            for (int i = 1; i < NUM_CELLS; i++)
            {
                for (int j = 1; j < NUM_CELLS; j++)
                {
                    if (GetCells()[i, j].GetColor() == (int)Cell.Type.userGreen)
                    {
                        Change[i, j] = (int)Cell.Type.empty;
                        GetCells()[i, j].ChangeType((int)Cell.Type.empty);
                    }

                    if (GetCells()[i, j].GetColor() == (int)Cell.Type.botBlue && Change[i, j] != (int)Cell.Type.botBlue)
                    {
                        Change[i, j] = (int)Cell.Type.empty;
                        GetCells()[i, j].ChangeType((int)Cell.Type.empty);
                    }

                    if (GetCells()[i, j].GetColor() == (int)Cell.Type.botRed && Change[i, j] != (int)Cell.Type.botRed)
                    {
                        Change[i, j] = (int)Cell.Type.empty;
                        GetCells()[i, j].ChangeType((int)Cell.Type.empty);
                    }
                }
            }
            turns = turnsLeft;
            this.HowMuchEmpty = this.emptyCounter;
            this.IsFull = false;
        }

        public async void StartGame()
        {
            List<Cell> cellsToFill = new List<Cell>();
            List<Cell> cellsToFillGreen = new List<Cell>();
            for (int i = 1; i < 19; i++)
            {
                for (int j = 1; j < 19; j++)
                {
                    if (cell[i, j].GetColor() == (int)Cell.Type.botBlue || cell[i, j].GetColor() == (int)Cell.Type.botRed || cell[i, j].GetColor() == (int)Cell.Type.userGreen)//מוצא את כל השחקנים שעל הלוח
                    {
                        if (cell[i, j].GetColor() == (int)Cell.Type.userGreen)
                            cellsToFillGreen.Add(cell[i, j]);
                        else
                            cellsToFill.Add(cell[i, j]);
                    }
                }
            }
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < cellsToFillGreen.Count; i++)
            {
                tasks.Add(TouchedCell(cellsToFillGreen[i]));
            }
            for (int i = 0; i < cellsToFill.Count; i++)
            {
                tasks.Add(TouchedCell(cellsToFill[i]));
            }
            await Task.WhenAll(tasks);
            activity.ShowScoreDialog();
        }

        public async Task TouchedCell(object obj)
        {

            if (obj is Cell)
            {
                Cell currentCell = (Cell)obj;
                await FillArea(currentCell, (Cell.Type)currentCell.GetColor());
            }
        }

        private async Task FillArea(Cell currentCell, Cell.Type type)
        {
            currentCell.ChangeType((int)type);
            HowMuchEmpty--;
            Thread.Sleep(1000 / fps);

            List<Task> t = new List<Task>();
            await Task.Yield();
            if (currentCell.Row > 0) // Left
            {
                Cell leftCell = cell[currentCell.Row - 1, currentCell.Col];
                if (leftCell.GetColor() == (int)Cell.Type.empty)
                {
                    t.Add(FillArea(leftCell, type));
                }
                else
                {
                    Console.WriteLine("Stop Left");
                }
            }

            if (currentCell.Row < NUM_CELLS - 1) // Left
            {
                Cell rightCell = cell[currentCell.Row + 1, currentCell.Col];
                if (rightCell.GetColor() == (int)Cell.Type.empty)
                {
                    t.Add(FillArea(rightCell, type));
                }
                else
                {
                    Console.WriteLine("Stop Right");
                }
            }

            if (currentCell.Col > 0) // Top
            {
                Cell topCell = cell[currentCell.Row, currentCell.Col - 1];
                if (topCell.GetColor() == (int)Cell.Type.empty)
                {
                    t.Add(FillArea(topCell, type));

                }
                else
                {
                    Console.WriteLine("Stop Top");
                }
            }

            if (currentCell.Col < NUM_CELLS - 1) // Bottom
            {
                Cell bottomCell = cell[currentCell.Row, currentCell.Col + 1];
                if (bottomCell.GetColor() == (int)Cell.Type.empty)
                {
                    t.Add(FillArea(bottomCell, type));
                }
                else
                {
                    Console.WriteLine("Stop Bottom");
                }
            }
            await Task.WhenAll(t);
            await Task.Yield();
        }

        public double GetPColor(Cell.Type type)
        {
            int count = 0;
            for (int i = 1; i < 20; i++)
            {
                for (int j = 1; j < 20; j++)
                {
                    if (GetCells()[i, j].GetColor() == (int)type)
                    {
                        count++;
                    }
                }
            }
            return Math.Round(((double)count / (double)emptyCounter * 100));
        }
        public void UseHint()
        {
            int[] xy = new int[2];
            xy[0] = 9;
            xy[1] = 10;
            int[,] xy2 = new int[2, 2];
            xy2[0, 0] = xy[0];
            xy2[1, 0] = xy[1];
            xy2[0, 1] = 4;
            xy2[1, 1] = 5;
            bool color = false;
            bool MoreThenOne = false;
            int count = 0;
            for (int i = 1; i < 19; i++)
            {
                for (int j = 1; j < 19; j++)
                {
                    if (Hint[i, j] == (int)Cell.Type.userGreen)
                    {
                        if (count == 0)
                        {
                            xy[0] = i;
                            xy[1] = j;
                            count++;
                        }
                        else
                        {
                            MoreThenOne = true;
                            xy2[0, 0] = xy[0];
                            xy2[1, 0] = xy[1];
                            xy2[0, 1] = i;
                            xy2[1, 1] = j;
                        }

                    }
                }
            }
            if (!MoreThenOne)
            {
                hint = new Thread(new ParameterizedThreadStart(Blub))
                {
                    IsBackground = true
                };
                hint.Start(xy);
            }
            else
            {
                hint = new Thread(new ParameterizedThreadStart(Blub2))
                {
                    IsBackground = true
                };
                hint.Start(xy2);

            }
        }
        public void Blub(object parameter)
        {
            bool color = true;
            int[] xy = (int[])parameter;
            while (!Changed)
            {
                cell[xy[0], xy[1]].ChangeType(color ? (int)Cell.Type.userGreen : (int)Cell.Type.empty);
                color = !color;
                Invalidate();
                Thread.Sleep(90);
            }
        }
        public void Blub2(object parameter)
        {
            bool color = true;
            int[,] xy = (int[,])parameter;
            while (!Changed)
            {
                cell[xy[0, 0], xy[0, 1]].ChangeType(color ? (int)Cell.Type.userGreen : (int)Cell.Type.empty);
                cell[xy[1, 0], xy[1, 1]].ChangeType(color ? (int)Cell.Type.userGreen : (int)Cell.Type.empty);
                color = !color;
                Invalidate();
                Thread.Sleep(90);
            }
        }
    }
}
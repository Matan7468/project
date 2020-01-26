using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Graphics;
using Android.Views;
using Android.Content;
using System;

namespace experience
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        AllLevels AllLevels;
        LinearLayout game, turnsLayout;
        Button hintbtn;
        Button restartbtn;
        Button skinsbtn;
        Button LeaderBoard;
        int counter;
        public TextView tvturns { get; set; }
        Dialog EndingDialog;
        public Button NextLvl { get; set; }
        public Button RetryLvl { get; set; }
        ISharedPreferences sp;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.game);
            hintbtn = FindViewById<Button>(Resource.Id.hintbtn);
            tvturns = FindViewById<TextView>(Resource.Id.tvTurnsLeft);
            restartbtn = FindViewById<Button>(Resource.Id.rstbtn);
            skinsbtn = FindViewById<Button>(Resource.Id.skinsbtn);
            turnsLayout = FindViewById<LinearLayout>(Resource.Id.turns);
            game = FindViewById<LinearLayout>(Resource.Id.Game);
            LeaderBoard = FindViewById<Button>(Resource.Id.LeaderBoardbtn);

            sp = GetSharedPreferences("details", FileCreationMode.Private);

            Point screenSize = new Point();
            this.WindowManager.DefaultDisplay.GetSize(screenSize);

            Cell.CellWidth = (screenSize.X - 100) / GameBoard.NUM_CELLS;
            Cell.CellHeight = game.LayoutParameters.Height / GameBoard.NUM_CELLS;

            hintbtn.Click += Hintbtn_Click;
            restartbtn.Click += Restartbtn_Click;
            skinsbtn.Click += Skinsbtn_Click;
            LeaderBoard.Click += LeaderBoard_Click;

            AllLevels = new AllLevels(this, tvturns);
            sp.Edit().PutInt("Levels", AllLevels.GetLevels().Count).Apply();
            tvturns.Text = AllLevels.GetLevels()[counter].GetTurns().ToString();

            View v = AllLevels.GetLevels()[counter];
            LinearLayout.LayoutParams parameters = new LinearLayout.LayoutParams(screenSize.X - 100, LinearLayout.LayoutParams.MatchParent);
            parameters.Gravity = GravityFlags.Center;
            v.LayoutParameters = parameters;
            game.AddView(v);
        }

        private void LeaderBoard_Click(object sender, EventArgs e)
        {
            
        }


        public void ShowScoreDialog()
        {
            double green = AllLevels.GetLevels()[counter].GetPColor(Cell.Type.userGreen);
            double red = AllLevels.GetLevels()[counter].GetPColor(Cell.Type.botRed);
            double blue = AllLevels.GetLevels()[counter].GetPColor(Cell.Type.botBlue);
            Point screenSize = new Point();
            this.WindowManager.DefaultDisplay.GetSize(screenSize);
            int height = (int)(screenSize.Y * 0.3);
            int width = (int)(screenSize.X * 0.75);
            EndingDialog = new Dialog(this);
            LinearLayout.LayoutParams layoutParamsColor = new LinearLayout.LayoutParams(80, 80);
            LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(0, ViewGroup.LayoutParams.MatchParent);
            layoutParams.Gravity = GravityFlags.CenterHorizontal;
            layoutParams.Weight = 0.3f;
            if (green > red && green > blue)
            {
                EndingDialog.SetContentView(Resource.Layout.WonDialog);
                LinearLayout lScore = EndingDialog.FindViewById<LinearLayout>(Resource.Id.LayoutScoreWinning);
                LinearLayout ScoreGreen = new LinearLayout(this);
                ScoreGreen.SetGravity(GravityFlags.Center);
                ScoreGreen.LayoutParameters = layoutParams;
                ScoreGreen.Orientation = Orientation.Vertical;
                View ivgreen = new View(this);
                ivgreen.SetBackgroundColor(Color.Green);
                ivgreen.LayoutParameters = layoutParamsColor;
                TextView tvGreenScore = new TextView(this);
                tvGreenScore.Text = green.ToString() + "%";
                tvGreenScore.Gravity = GravityFlags.CenterHorizontal;
                ScoreGreen.AddView(ivgreen);
                ScoreGreen.AddView(tvGreenScore);
                lScore.AddView(ScoreGreen);
                if (red > 0)
                {
                    LinearLayout ScoreRed = new LinearLayout(this);
                    ScoreRed.SetGravity(GravityFlags.Center);
                    ScoreRed.LayoutParameters = layoutParams;
                    ScoreRed.Orientation = Orientation.Vertical;
                    View ivRed = new View(this);
                    ivRed.SetBackgroundColor(Color.Red);
                    ivRed.LayoutParameters = layoutParamsColor;
                    TextView tvRedScore = new TextView(this);
                    tvRedScore.Text = red.ToString() + "%";
                    tvRedScore.Gravity = GravityFlags.CenterHorizontal;
                    ScoreRed.AddView(ivRed);
                    ScoreRed.AddView(tvRedScore);
                    lScore.AddView(ScoreRed);
                }
                if (blue > 0)
                {
                    LinearLayout ScoreBlue = new LinearLayout(this);
                    ScoreBlue.SetGravity(GravityFlags.Center);
                    ScoreBlue.LayoutParameters = layoutParams;
                    ScoreBlue.Orientation = Orientation.Vertical;
                    View ivBlue = new View(this);
                    ivBlue.SetBackgroundColor(Color.Blue);
                    ivBlue.LayoutParameters = layoutParamsColor;
                    TextView tvBlueScore = new TextView(this);
                    tvBlueScore.Text = blue.ToString() + "%";
                    tvBlueScore.Gravity = GravityFlags.CenterHorizontal;
                    ScoreBlue.AddView(ivBlue);
                    ScoreBlue.AddView(tvBlueScore);
                    lScore.AddView(ScoreBlue);
                }

                EndingDialog.SetCancelable(false);
                NextLvl = EndingDialog.FindViewById<Button>(Resource.Id.NextLvl);
                NextLvl.Click += (senderNext, eNext) =>
                {
                    game.RemoveView(AllLevels.GetLevels()[counter]);
                    AllLevels.GetLevels()[counter].Restart();
                    AllLevels.GetLevels()[counter].t.Abort();
                    counter++;

                    try
                    {
                        tvturns.Text = AllLevels.GetLevels()[counter].GetTurns().ToString();
                        game.AddView(AllLevels.GetLevels()[counter]);
                        EndingDialog.Dismiss();
                    }
                    catch
                    {
                        counter = 0;
                    }
                };
                EndingDialog.Show();
                EndingDialog.Window.SetLayout(width, height);
                EndingDialog.Window.SetGravity(GravityFlags.Center);
            }
            else
            {
                EndingDialog.SetContentView(Resource.Layout.LosingDialog);
                LinearLayout lScore = EndingDialog.FindViewById<LinearLayout>(Resource.Id.LayoutScoreLosing);
                LinearLayout ScoreGreen = new LinearLayout(this);
                ScoreGreen.SetGravity(GravityFlags.Center);
                ScoreGreen.LayoutParameters = layoutParams;
                ScoreGreen.Orientation = Orientation.Vertical;
                ImageView ivgreen = new ImageView(this);
                ivgreen.SetBackgroundColor(Color.Green);
                ivgreen.LayoutParameters = layoutParamsColor;
                TextView tvGreenScore = new TextView(this);
                tvGreenScore.Text = green.ToString() + "%";
                tvGreenScore.Gravity = GravityFlags.CenterHorizontal;
                ScoreGreen.AddView(ivgreen);
                ScoreGreen.AddView(tvGreenScore);
                lScore.AddView(ScoreGreen);
                if (red > 0)
                {
                    LinearLayout ScoreRed = new LinearLayout(this);
                    ScoreRed.SetGravity(GravityFlags.Center);
                    ScoreRed.LayoutParameters = layoutParams;
                    ScoreGreen.Orientation = Orientation.Vertical;
                    ImageView ivRed = new ImageView(this);
                    ivRed.SetBackgroundColor(Color.Red);
                    ivRed.LayoutParameters = layoutParamsColor;
                    TextView tvRedScore = new TextView(this);
                    tvRedScore.Text = red.ToString() + "%";
                    tvRedScore.Gravity = GravityFlags.CenterHorizontal;
                    ScoreRed.AddView(ivRed);
                    ScoreRed.AddView(tvRedScore);
                    lScore.AddView(ScoreRed);
                }
                if (blue > 0)
                {
                    LinearLayout ScoreBlue = new LinearLayout(this);
                    ScoreBlue.SetGravity(GravityFlags.Center);
                    ScoreBlue.LayoutParameters = layoutParams;
                    ScoreBlue.Orientation = Orientation.Vertical;
                    ImageView ivBlue = new ImageView(this);
                    ivBlue.SetBackgroundColor(Color.Blue);
                    ivBlue.LayoutParameters = layoutParamsColor;
                    TextView tvBlueScore = new TextView(this);
                    tvBlueScore.Text = blue.ToString() + "%";
                    tvBlueScore.Gravity = GravityFlags.CenterHorizontal;
                    ScoreBlue.AddView(ivBlue);
                    ScoreBlue.AddView(tvBlueScore);
                    lScore.AddView(ScoreBlue);
                }
                RetryLvl = EndingDialog.FindViewById<Button>(Resource.Id.Restartbtn);
                RetryLvl.Click += (senderNext, eNext) =>
                {
                    AllLevels.GetLevels()[counter].Restart();
                    tvturns.Text = AllLevels.GetLevels()[counter].GetTurns().ToString();
                    EndingDialog.Dismiss();
                };
                EndingDialog.Show();
                EndingDialog.Window.SetLayout(width, height);
            }


        }
        private void Skinsbtn_Click(object sender, EventArgs e)
        {
        }

        private void Restartbtn_Click(object sender, EventArgs e)
        {
            if (!AllLevels.GetLevels()[counter].playing)
            {
                AllLevels.GetLevels()[counter].Restart();
                tvturns.Text = AllLevels.GetLevels()[counter].GetTurns().ToString();
            }

        }

        private void Hintbtn_Click(object sender, EventArgs e)
        {
            //if (!AllLevels.GetLevels()[counter].playing)
            //{
            //    AllLevels.GetLevels()[counter].UseHint();
            //}
        }


    }
}

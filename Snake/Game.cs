using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using GameLogic;
using GameLogic.Ai;

namespace Snake
{
    public partial class FrmGame : Form
    {
        public Game Game { get; private set; }
        public FrmGame()
        {
            InitializeComponent();
            StartGame();
        }

        private void StartGame()
        {
            // disable the controls.
            lblGameOver.Visible = false;
            newGameToolStripMenuItem.Enabled = false;
            // initialize with default Game.Settings.
            Game = new Game(new ScoreSetup(), new Settings());
            Game.Tick += UpdateScreen;
            Game.Play();

            // add the head to the body.
            lblScore.Text = Game.Settings.Score.ToString();
        }

        private void UpdateScreen()
        {
            if (Game.Settings.GameOver)
                return;

            Invoke(new MethodInvoker(() =>
            {
                lblTimer.Text = TimeSpan.FromSeconds(Game.Settings.Duration / Game.Settings.Speed).ToString();
                lblScore.Text = Game.Settings.Score.ToString();

                picCanvas.Refresh();
            }));
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void PicCanvas_Paint(object sender, PaintEventArgs e)
        {
            var canvas = e.Graphics;

            if (Game.Settings.GameOver)
                return;

            
            var ratio = e.ClipRectangle.Width / (double)e.ClipRectangle.Height;
            var vScale = e.ClipRectangle.Height / Game.Settings.Height;
            var hScale = e.ClipRectangle.Width / Game.Settings.Width;



            // draw the snake.
            for (var i = 0; i < Game.Snake.Body.Count; ++i)
            {
                // draw and colour head black.
                Brush snakeColour;
                if (i == 0)
                    snakeColour = Brushes.Black;
                // draw and colour rest of body green.
                else
                    snakeColour = Brushes.Green;
                // draw the whole snake.
                canvas.FillEllipse(snakeColour,
                    new Rectangle(Game.Snake.Body[i].X * (int)hScale, Game.Snake.Body[i].Y * (int)vScale,
                        (int)(Game.Settings.Width * ratio), (int)(Game.Settings.Height * ratio)));
                // draw food.
                canvas.FillEllipse(Brushes.Red,
                    new Rectangle(Game.Food.X * (int)hScale,
                        Game.Food.Y * (int)vScale, (int)(Game.Settings.Width * ratio),
                        (int)(Game.Settings.Height * ratio)));

            }
        }

        private void FrmGame_KeyDown(object sender, KeyEventArgs e)
        {
            // button is being pressed.
            Input.ChangeState(e.KeyCode, true);
        }

        private void FrmGame_KeyUp(object sender, KeyEventArgs e)
        {
            // button is no longer being pressed.
            Input.ChangeState(e.KeyCode, false);
        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            Focus();
            StartGame();
        }

        private void FrmGame_Load(object sender, EventArgs e)
        {
            tmrTimer.Enabled = false;
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var msg = "This C# implementation of Snake was developed by Jeffrey Li, ";
            msg += "from December 29, 2016 to Feburary 11, 2017.";
            msg += "\nCoded in C# in Visual Studio 2015.";
            MessageBox.Show(msg, "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void NewGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BtnPlay_Click(sender, e);
        }

        private void picCanvas_Click(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Threading;

namespace Flappy_Bird_Game_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();

        double score;
        int gravity = 8;
        bool gameOver;
        Rect flappyBirdHitBox;
        public MainWindow()
        {
            InitializeComponent();

            gameTimer.Tick += MainEventTimer;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            StartGame();
        }

        private void MainEventTimer(object sender, EventArgs e)
        {
            txtScore.Content = "Score: " + score;

            // The flappyBirdHitBox rectangle is updated to represent the position and dimensions of the flappy bird's hitbox.
            flappyBirdHitBox = new Rect(Canvas.GetLeft(flappyBird), Canvas.GetTop(flappyBird), flappyBird.Width - 10, flappyBird.Height - 10);

            // The vertical position of the flappy bird is adjusted downwards
            // by adding the gravity value to its current position.
            // This simulates the effect of gravity pulling the bird downwards.
            Canvas.SetTop(flappyBird, Canvas.GetTop(flappyBird) + gravity);


            // Make sure the bird cannot fly over or below the window
            if (Canvas.GetTop(flappyBird) < -10 || Canvas.GetTop(flappyBird) > 450)
            {
                EndGame();
            }
            

            foreach (var x in MyCanvas.Children.OfType<Image>())
            {
                if ((string)x.Tag == "obs1" || (string)x.Tag == "obs2" || (string)x.Tag == "obs3")
                {
                    // The moving speed of the pipes
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 5);

                    if (Canvas.GetLeft(x) < -100)
                    {
                        Canvas.SetLeft(x, 800);

                        score += .5;
                    }

                    Rect pipeHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);


                    if (flappyBirdHitBox.IntersectsWith(pipeHitBox))
                    {
                        EndGame();
                    }

                }


                if ((string)x.Tag == "cloud")
                {
                    // The moving speed of the clouds, slower than the pipes
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 2);

                    if (Canvas.GetLeft(x) < -250)
                    {
                        Canvas.SetLeft(x, 550);
                    }
                }
            }

        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            // if space key is pressed, the bird will rotate slightly upwards,
            // and the gravity value is set to a negative value, simulating a slight upward movement of the bird.

            if (e.Key == Key.Space)
            {
                flappyBird.RenderTransform = new RotateTransform(-20, flappyBird.Width / 2, flappyBird.Height / 2);
                gravity = -8;
            }

            if (e.Key == Key.R && gameOver == true)
            {
                StartGame();
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            // if space key is released, If the Space key is released, the flappy bird is rotated slightly downwards,
            // and the gravity value is set back to its original positive value, simulating a slight downward movement of the bird.
            flappyBird.RenderTransform = new RotateTransform(5, flappyBird.Width / 2, flappyBird.Height / 2);
            gravity = 8;
        }

        private void StartGame()
        {
            MyCanvas.Focus();

            int temp = 300;
            score = 0;
            gameOver = false;
            Canvas.SetTop(flappyBird, 146);

            foreach(var x in MyCanvas.Children.OfType<Image>())
            {
                if((string)x.Tag == "obs1")
                {
                    Canvas.SetLeft(x, 500);
                }
                if ((string)x.Tag == "obs2")
                {
                    Canvas.SetLeft(x, 800);
                }
                if ((string)x.Tag == "obs3")
                {
                    Canvas.SetLeft(x, 1100);
                }
                if((string)x.Tag == "cloud")
                {
                    Canvas.SetLeft(x, 100 + temp);
                    temp = 800;
                }
            }

            gameTimer.Start();
        }

        private void EndGame()
        {
            gameTimer.Stop();
            gameOver = true;
            txtScore.Content += " Game Over !! Press R to try again";
        }
    }
}

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

//<Ellipse Name="Breakr" Fill="Red" Height="20" Canvas.Left="390" Stroke="Black" Canvas.Top="350" Width="20" Opacity ="0.8"></Ellipse>

namespace Brick_Breakr
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        double move_paddle_by = 30;
        bool left_key_down = false;
        bool right_key_down = false;

        double breaker_speed_x;
        double breaker_speed_y;

        List<Rectangle> bricks;
        Rectangle brick;
        int num_bricks_left = 0;

        Ellipse Breakr;

        /*Ellipse ball_1;
        Ellipse ball_2;
        Ellipse ball_3;*/
        int lives_left = 2;

        bool life_lost;

        static DispatcherTimer clock;

        System.Timers.Timer life_lost_timer;

        public MainWindow()
        {
            InitializeComponent();

            breaker_speed_x = 0;
            breaker_speed_y = 5;

            bricks = new List<Rectangle>(30);

            clock = new DispatcherTimer();
            clock.Tick += new EventHandler(clock_tick);
            clock.Interval = new TimeSpan(0, 0, 0, 0, 1);
            clock.Start();

            life_lost_timer = new System.Timers.Timer(3000);
            life_lost_timer.Elapsed += new System.Timers.ElapsedEventHandler(life_lost_timer_elapsed);

            //init_breakrs();
            place_bricks();

            Breakr = new Ellipse();
            Breakr.Width = 20;
            Breakr.Height = 20;
            Breakr.Stroke = Brushes.Black;
            Breakr.Fill = Brushes.Red;
            Breakr.Opacity = 0.8;

            MyGameCanvas.Children.Insert(0, Breakr);
            Canvas.SetTop(Breakr, Canvas.GetTop(Breakr_Start));
            Canvas.SetLeft(Breakr, Canvas.GetLeft(Breakr_Start));
            Breakr.Visibility = System.Windows.Visibility.Visible;

            life_lost = false;

        }

        /*private void init_breakrs()
        {
            ball_1 = new Ellipse();
            ball_1.Width = 20;
            ball_1.Height = 20;
            ball_1.Stroke = Brushes.Black;
            ball_1.Fill = Brushes.Red;
            ball_1.Opacity = 0.8;

            MyGameCanvas.Children.Insert(0, ball_1);
            Canvas.SetTop(ball_1, Canvas.GetTop(Breakr_Start));
            Canvas.SetLeft(ball_1, Canvas.GetLeft(Breakr_Start));
            ball_1.Visibility = System.Windows.Visibility.Hidden;

            ball_2 = new Ellipse();
            ball_2.Width = 20;
            ball_2.Height = 20;
            ball_2.Stroke = Brushes.Black;
            ball_2.Fill = Brushes.Red;
            ball_2.Opacity = 0.8;

            MyGameCanvas.Children.Insert(0, ball_2);
            Canvas.SetTop(ball_2, Canvas.GetTop(Breakr_Start));
            Canvas.SetLeft(ball_2, Canvas.GetLeft(Breakr_Start));
            ball_2.Visibility = System.Windows.Visibility.Hidden;

            ball_3 = new Ellipse();
            ball_3.Width = 20;
            ball_3.Height = 20;
            ball_3.Stroke = Brushes.Black;
            ball_3.Fill = Brushes.Red;
            ball_3.Opacity = 0.8;

            MyGameCanvas.Children.Insert(0, ball_3);
            Canvas.SetTop(ball_3, Canvas.GetTop(Breakr_Start));
            Canvas.SetLeft(ball_3, Canvas.GetLeft(Breakr_Start));
            ball_3.Visibility = System.Windows.Visibility.Hidden;

        }*/

        private void place_bricks()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    brick = new Rectangle();
                    brick.Width = 60;
                    brick.Height = 30; 

                    SolidColorBrush fill_color = new SolidColorBrush();
                    fill_color.Color = Color.FromRgb(255, 255, 255);
                    brick.Fill = fill_color;


                    brick.Stroke = Brushes.Black;
                    brick.RadiusX = 1;
                    brick.RadiusY = 1;
                    brick.StrokeThickness = 1;

                    brick.Visibility = System.Windows.Visibility.Visible;

                    MyGameCanvas.Children.Insert(0, brick);

                    Canvas.SetLeft(brick, 100 + j*62);
                    Canvas.SetTop(brick, 100 + i*32);

                    bricks.Add(brick);
                    num_bricks_left++;
                }
            }
        }

        private void clock_tick(object sender, EventArgs e)
        {


            breakr_boundary_check();

            if (life_lost)
            {
                reset_breakr();
            }

            does_breakr_hit_paddle(Paddle);
            for (int i = 0; i < bricks.Count; i++)
            {
                does_breakr_hit_brick(bricks[i]);
            }

            double breakr_top = Canvas.GetTop(Breakr);
            double breakr_left = Canvas.GetLeft(Breakr);

            Canvas.SetTop(Breakr, breakr_top + breaker_speed_y);
            Canvas.SetLeft(Breakr, breakr_left + breaker_speed_x);

        }

        private void breakr_boundary_check()
        {
            double breakr_top = Canvas.GetTop(Breakr);
            double breakr_left = Canvas.GetLeft(Breakr);

            if (breakr_top < 1)
            {
                breaker_speed_y *= -1;
            }
            if (breakr_left < 1 || breakr_left + Breakr.Width > 790)
            {
                breaker_speed_x *= -1;
            }
            if (breakr_top > Canvas.GetTop(Paddle))
            {

                life_lost = true;
                lives_left--;

                clock.Stop();

                if (Life_1.IsVisible)
                {
                    Life_1.Visibility = System.Windows.Visibility.Hidden;
                    life_lost_timer.Start();
                }
                else if (Life_2.IsVisible)
                {
                    Life_2.Visibility = System.Windows.Visibility.Hidden;
                    life_lost_timer.Start();
                }
                else if (Life_3.IsVisible)
                {
                    Life_3.Visibility = System.Windows.Visibility.Hidden;
                    life_lost_timer.Start();
                }
                else
                {
                    GAME_OVER.Visibility = System.Windows.Visibility.Visible;
                }

            }

        }

        private void reset_breakr()
        {
            Breakr.Visibility = System.Windows.Visibility.Hidden;
            /*MyGameCanvas.Children.Remove(Breakr);

            if (lives_left == 1)
            {
                Breakr = ball_2;
            } 
            else if (lives_left == 0) {
                Breakr = ball_3;
            }*/

            Canvas.SetTop(Breakr, Canvas.GetTop(Breakr_Start));
            Canvas.SetLeft(Breakr, Canvas.GetLeft(Breakr_Start));

            Breakr.Visibility = System.Windows.Visibility.Visible;

            breaker_speed_x = 0;
            breaker_speed_y = 5;

            life_lost = false;

        }

        private void life_lost_timer_elapsed(Object sender, EventArgs e)
        {
            life_lost_timer.Stop();
            clock.Start();
        }

        private void does_breakr_hit_brick(Rectangle r)
        {
            double breakr_middle = Canvas.GetLeft(Breakr) + 10;
            double breakr_top = Canvas.GetTop(Breakr);
            double breakr_bottom = Canvas.GetTop(Breakr) + 20;

            double r_left = Canvas.GetLeft(r);
            double r_right = r_left + r.Width;
            double r_top = Canvas.GetTop(r);
            double r_division = r.Width / 5;

            if (breakr_middle < r_right && breakr_middle > r_left && 
                ((breakr_bottom < r_top + 1 && breakr_bottom > r_top - 1)  ||
                   (breakr_top > r_top && breakr_top < r_top + r.Height)) && 
                   r.Visibility == System.Windows.Visibility.Visible){
                   
                breaker_speed_y *= -1;
                
                if (breakr_middle >= r_left && breakr_middle < r_left + r_division)
                {
                    breaker_speed_x -= 1;
                }
                else if (breakr_middle >= r_left + 2 * r_division && breakr_middle < r_left + r.Width)
                {
                    breaker_speed_x += 1;
                }

                r.Visibility = System.Windows.Visibility.Hidden;
                num_bricks_left--;
                check_win();

            }

        }

        private void does_breakr_hit_paddle(Rectangle r)
        {
            double breakr_middle = Canvas.GetLeft(Breakr) + 10;
            double breakr_top = Canvas.GetTop(Breakr);
            double breakr_bottom = Canvas.GetTop(Breakr) + 20;

            double r_left = Canvas.GetLeft(r);
            double r_right = r_left + r.Width;
            double r_top = Canvas.GetTop(r);
            double r_division = r.Width / 5;

            if (breakr_middle < r_right && breakr_middle > r_left &&
                ((breakr_bottom < r_top + 1 && breakr_bottom > r_top - 1)))
            {

                breaker_speed_y *= -1;

                if (breakr_middle >= r_left && breakr_middle < r_left + r_division)
                {
                    breaker_speed_x = -3;
                }
                else if (breakr_middle >= r_left + r_division && breakr_middle < r_left + 2 * r_division)
                {
                    breaker_speed_x = -2;
                }
                else if (breakr_middle >= r_left + 3 * r_division && breakr_middle < r_left - 2 * r_division)
                {
                    breaker_speed_x = 2;
                }
                else if (breakr_middle >= r_left + 4 * r_division && breakr_middle < r_left + r.Width)
                {
                    breaker_speed_x = 3;
                }

            }

        }

        private void check_win()
        {
            if (num_bricks_left == 0)
            {
                clock.Stop();
                YOU_WIN.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
                double paddle_left = Canvas.GetLeft(Paddle);

                if (e.Key == Key.Left && right_key_down == false && paddle_left > 0)
                {
                    left_key_down = true;
                    paddle_left -= move_paddle_by;
                }
                else if (e.Key == Key.Right && left_key_down == false && paddle_left + Paddle.Width < 780)
                {
                    right_key_down = true;
                    paddle_left += move_paddle_by;
                }

                Canvas.SetLeft(Paddle, paddle_left);
        }

        private void KeyUpHandler(object sender, KeyEventArgs e)
        {
            if (right_key_down == true && e.Key == Key.Right)
            {
                right_key_down = false;
            }

            if (left_key_down == true && e.Key == Key.Left)
            {
                left_key_down = false;
            }

        }
 
    }
}

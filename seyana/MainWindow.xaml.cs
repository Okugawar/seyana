using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace seyana
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SerifuWindow sw;
        ebifry ebi;

        private int x, y;
        private int width = 300, height = 300;
        public Point toPoint() { return new Point(x, y); }
        public Util.rect toRect() { return new Util.rect(x, y, width, height); }

        public MainWindow()
        {
            InitializeComponent();

            sw = new SerifuWindow();
            ebi = new ebifry();

            x = 100;
            y = 200;
            width = (int)Width;
            height = (int)Height;
            setPosition();
        }

        private void setPosition()
        {
            Canvas.SetLeft(this, x);
            Canvas.SetTop(this, y);
        }

        /// <summary>
        /// speak something
        /// </summary>
        /// <param name="message">something to say</param>
        private void say(string message)
        {
            sw.say(message);
            Canvas.SetLeft(sw, x);
            Canvas.SetTop(sw, y - 60);
            sw.Show();
        }

        /// <summary>
        /// summon ebifry
        /// </summary>
        private void createEbi()
        {
            // 前のがまだ生きてたら召喚しない
            if (ebi.live) return;

            do
            {
                double ang = Util.rnd.NextDouble() * 2 * Math.PI;
                double x0 = (x + width / 2) + 100 * Math.Cos(ang) - ebi.w / 2;
                double y0 = (y + height / 2) + 100 * Math.Sin(ang) - ebi.h / 2;
                ebi.x = (int)x0;
                ebi.y = (int)y0;
                Canvas.SetLeft(ebi, x0);
                Canvas.SetTop(ebi, y0);
            } while (!Util.isInScreen(ebi.toRect()));
            ebi.Show();
            ebi.live = true;
            Task.Factory.StartNew(gotoEbi);
        }

        /// <summary>
        /// ebi massigura
        /// </summary>
        private void gotoEbi()
        {
            try {
                double speed = 2;

                Thread.Sleep(3000);

                while (ebi.live)
                {
                    int eatcount = 0;
                    while (true)
                    {
                        int dx = (ebi.x + ebi.w / 2) - (x + width / 2);
                        int dy = (ebi.y + ebi.h / 2) - (y + height / 2);
                        double dst = Math.Sqrt(dx * dx + dy * dy);
                        if (dst < 10) break;

                        double dir = Math.Atan2(dy, dx);
                        x = (int)(x + speed * Math.Cos(dir));
                        y = (int)(y + speed * Math.Sin(dir));

                        Dispatcher.Invoke(() => setPosition());
                        Thread.Sleep(20);
                    }
                    Dispatcher.Invoke(() => setPosition());

                    while (eatcount < 500)
                    {
                        int dx = (ebi.x + ebi.w / 2) - (x + width / 2);
                        int dy = (ebi.y + ebi.h / 2) - (y + height / 2);
                        double dst = Math.Sqrt(dx * dx + dy * dy);
                        if (dst < 100) eatcount++;
                        else break;

                        dst = Util.rnd.NextDouble() * 60;
                        double dir = Util.rnd.NextDouble() * 2 * Math.PI;
                        x = (int)((ebi.x + ebi.w / 2) + dst * Math.Cos(dir) - width / 2);
                        y = (int)((ebi.y + ebi.h / 2) + dst * Math.Sin(dir) - height / 2);
                        Dispatcher.Invoke(() => setPosition());
                        Thread.Sleep(5);
                    }

                    if (eatcount >= 50)
                    {
                        Dispatcher.Invoke(() => ebi.eaten());
                    }
                }
            }catch(TaskCanceledException e) { }
        }

        /// <summary>
        /// action something(to test)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Action_Clicked(object sender, RoutedEventArgs args)
        {
            createEbi();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            say("ｾﾔﾅｰ");
        }

        /// <summary>
        /// menu: Quit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Quit_Clicked(object sender, RoutedEventArgs args)
        {
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            sw.Close();
            ebi.Close();
        }
    }
}

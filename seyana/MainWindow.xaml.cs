﻿using System;
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
        SeyanaBrain brain;
        SerifuWindow sw;
        ebifry ebi;

        public static int x { get; private set; }
        public static int y { get; private set; }
        public static int width { get; private set; }
        public static int height { get; private set; }
        public Point toPoint() { return new Point(x, y); }
        public Util.rect toRect() { return new Util.rect(x, y, width, height); }

        public MainWindow()
        {
            InitializeComponent();

            Show();

            brain = SeyanaBrain.SeyanaBrainFactory;
            sw = new SerifuWindow();
            ebi = new ebifry();

            brain.init(this, sw, ebi);
            sw.Owner = this;
            ebi.Owner = this;

            x = 100;
            y = 200;

            var p0 = PointToScreen(new Point(0, 0));
            var p1 = PointToScreen(new Point(Width, Height));
            width = (int)(p1.X - p0.X);
            height = (int)(p1.Y - p0.Y);

            setPosition();
        }

        private void setPosition()
        {
            setPosition(this, x, y);
        }
        private void setPosition(int x, int y)
        {
            setPosition(this, x, y);
        }
        private void setPosition(UIElement e, int x, int y)
        {
            var p0 = PointToScreen(new Point(0, 0));
            var p1 = PointFromScreen(new Point(p0.X - x, p0.Y - y));
            Canvas.SetLeft(e, Math.Abs(p1.X));
            Canvas.SetTop(e, Math.Abs(p1.Y));
            if(e == this)
            {
                var p2 = PointToScreen(new Point(0, 0));
                MainWindow.x = (int)p2.X;
                MainWindow.y = (int)p2.Y;
            }
        }
        public void setPositionInvoke(UIElement e, int x, int y)
        {
            Dispatcher.Invoke(() => setPosition(e, x, y));
        }

        /// <summary>
        /// speak something
        /// </summary>
        /// <param name="message">something to say</param>
        private void say(string message)
        {
            sw.say(message);
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
                setPosition(ebi, (int)x0, (int)y0);
            } while (!Util.isInScreen(ebi.toRect()));
            ebi.spawn(50);
        }

        public void faceLeft() { Dispatcher.Invoke(() => invert.ScaleX = 1); }
        public void faceRight() { Dispatcher.Invoke(() => invert.ScaleX = -1); }

        /// <summary>
        /// action something(to test)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Action_Clicked(object sender, RoutedEventArgs args)
        {
            createEbi();
        }

        /// <summary>
        /// menu: summon ebifry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Summon_Clicked(object sender, RoutedEventArgs args)
        {
            createEbi();
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

        public void hideInvoke()
        {
            try
            {
                Dispatcher.Invoke(() => Hide());
            }
            catch (Exception e) { Console.WriteLine(e.StackTrace); }
        }
        public void showInvoke()
        {
            try
            {
                Dispatcher.Invoke(() => Show());
            }
            catch (Exception e) { Console.WriteLine(e.StackTrace); }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            say("ｾﾔﾅｰ");
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            sw.Close();
            ebi.Close();
        }
    }
}

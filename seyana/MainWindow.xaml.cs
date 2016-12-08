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

namespace seyana
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SerifuWindow sw;
        private int x, y;
        public MainWindow()
        {
            InitializeComponent();

            sw = new SerifuWindow();
            x = 600;
            y = 500;
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
            Canvas.SetLeft(sw, x + 100);
            Canvas.SetTop(sw, y - 60);
            sw.Show();
        }

        /// <summary>
        /// action something(to test)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Action_Clicked(object sender, RoutedEventArgs args)
        {
            say("ｾﾔﾅｰ");
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
        }
    }
}

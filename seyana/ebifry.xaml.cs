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
using System.Windows.Shapes;

namespace seyana
{
    /// <summary>
    /// Interaction logic for ebifry.xaml
    /// </summary>
    public partial class ebifry : Window
    {
        public int w { get; private set; }
        public int h { get; private set; }
        public int x, y;

        public bool live;

        public Util.rect toRect() { return new Util.rect(x, y, w, h); }
        public Point toPoint() { return new Point(x, y); }

        public ebifry()
        {
            InitializeComponent();
            w = (int)Width;
            h = (int)Height;

            live = false;
        }

        public void eaten()
        {
            live = false;
            Hide();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            var pt = PointFromScreen(new Point(0, 0));
            x = -(int)pt.X;
            y = -(int)pt.Y;
        }
    }
}

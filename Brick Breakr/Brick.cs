using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Brick_Breakr
{
    class Brick
    {
        Rectangle brick;

        Brick()
        {
            brick = new Rectangle();

            brick.Visibility = System.Windows.Visibility.Visible;
            brick.Height = 40;
            brick.Width = 53;
            
            SolidColorBrush fill_color = new SolidColorBrush();
            fill_color.Color = Color.FromRgb(0, 0, 0);
            brick.Fill = fill_color;

        }
    }
}

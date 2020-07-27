using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;

namespace MahAppsMetro.Demo.Model
{
    public class EllipseInfo
    {
        public Ellipse Ellipse { get; set; }

        public double VelocityY { get; set; }

        public EllipseInfo(Ellipse ellipse, double velocityY)
        {
            Ellipse = ellipse;
            VelocityY = velocityY;
        }
    }
}

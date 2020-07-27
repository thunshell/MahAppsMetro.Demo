using MahAppsMetro.Demo.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace MahAppsMetro.Demo.Windows
{
    /// <summary>
    /// Interaction logic for WaveGridWindow.xaml
    /// </summary>
    public partial class WaveGridWindow : Window
    {
        private Vector3D zoomDelta;

        private WaveGrid _grid;
        private bool _rendering;
        private double _lastTimeRendered;
        private double _firstPeak = 6.5;

        // Values to try:
        //   GridSize=20, RenderPeriod=125
        //   GridSize=50, RenderPeriod=50
        private const int GridSize = 50;
        private const double RenderPeriodInMS = 50;

        public WaveGridWindow()
        {
            InitializeComponent();

            _grid = new WaveGrid(GridSize);        // 10x10 grid
            slidPeakHeight.Value = _firstPeak;
            _grid.SetCenterPeak(_firstPeak);
            meshMain.Positions = _grid.Points;
            meshMain.TriangleIndices = _grid.TriangleIndices;

            // On each WheelMouse change, we zoom in/out a particular % of the original distance
            const double ZoomPctEachWheelChange = 0.02;
            zoomDelta = Vector3D.Multiply(ZoomPctEachWheelChange, camMain.LookDirection);
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
                // Zoom in
                camMain.Position = Point3D.Add(camMain.Position, zoomDelta);
            else
                // Zoom out
                camMain.Position = Point3D.Subtract(camMain.Position, zoomDelta);
            Trace.WriteLine(camMain.Position.ToString());
        }

        // Start/stop animation
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (!_rendering)
            {
                _grid = new WaveGrid(GridSize);        // 10x10 grid
                _grid.SetCenterPeak(_firstPeak);
                meshMain.Positions = _grid.Points;

                _lastTimeRendered = 0.0;
                CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
                btnStart.Content = "Stop";
                slidPeakHeight.IsEnabled = false;
                _rendering = true;
            }
            else
            {
                CompositionTarget.Rendering -= new EventHandler(CompositionTarget_Rendering);
                btnStart.Content = "Start";
                slidPeakHeight.IsEnabled = true;
                _rendering = false;
            }
        }

        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            RenderingEventArgs rargs = (RenderingEventArgs)e;
            if ((rargs.RenderingTime.TotalMilliseconds - _lastTimeRendered) > RenderPeriodInMS)
            {
                // Unhook Positions collection from our mesh, for performance
                // (see http://blogs.msdn.com/timothyc/archive/2006/08/31/734308.aspx)
                meshMain.Positions = null;

                // Do the next iteration on the water grid, propagating waves
                _grid.ProcessWater();

                // Then update our mesh to use new Z values
                meshMain.Positions = _grid.Points;

                _lastTimeRendered = rargs.RenderingTime.TotalMilliseconds;
            }
        }

        private void slidPeakHeight_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _firstPeak = slidPeakHeight.Value;
            _grid.SetCenterPeak(_firstPeak);
        }
    }
}

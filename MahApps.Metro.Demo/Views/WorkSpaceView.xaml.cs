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

namespace MahAppsMetro.Demo.Views
{
    /// <summary>
    /// Interaction logic for WorkSpaceView.xaml
    ///  放大缩小
    ///  左右平移
    ///  
    /// </summary>
    public partial class WorkSpaceView : UserControl
    {

        public WorkSpaceView()
        {
            InitializeComponent();
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            // 判断左键是否按下
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //声明DataObject,并打包圆控件的图像绘制方式(包含颜色)、高度及其副本。
                DataObject data = new DataObject();
                data.SetData(DataFormats.StringFormat, "rectangle");

                //使用DragDrop的DoDragDrop方法开启拖动功能。拖动方式为拖动复制或移动
                DragDrop.DoDragDrop(this, data, DragDropEffects.Copy | DragDropEffects.Move);
            }
        }

        private void VisualCanvas_Drop(object sender, DragEventArgs e)
        {
            Point p = e.GetPosition(canvas);
            //检查数据是否包含制定字符串类型数据
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                //获取字符串
                string dataString = (string)e.Data.GetData(DataFormats.StringFormat);
                switch (dataString)
                {
                    case "rectangle":
                        canvas.CreateRectangle(p);
                        break;
                }
            }
        }

        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
        }

        private void Canvas_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy | DragDropEffects.Move;
        }

        private void Border_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            e.UseDefaultCursors = false;
            FrameworkElement v = (FrameworkElement)(e.OriginalSource);
            var b = Helper.WpfHelper.RenderVisaulToBitmap(v, (int)v.ActualWidth, (int)v.ActualHeight);
            var c = Helper.WpfHelper.BitmapSourceToBitmap(b);
            var d = Helper.BitmapCursor.CreateBmpCursor(c);
            Cursor = d;
        }

    }

    public class DragDropAdorner : Adorner

    {

        public DragDropAdorner(UIElement parent)

            : base(parent)

        {

            IsHitTestVisible = false; // Seems Adorner is hit test visible?

            mDraggedElement = parent as FrameworkElement;

        }



        protected override void OnRender(DrawingContext drawingContext)

        {

            base.OnRender(drawingContext);



            if (mDraggedElement != null)

            {

                Win32.POINT screenPos = new Win32.POINT();

                if (Win32.GetCursorPos(ref screenPos))

                {

                    Point pos = PointFromScreen(new Point(screenPos.X, screenPos.Y));

                    Rect rect = new Rect(pos.X, pos.Y, mDraggedElement.ActualWidth, mDraggedElement.ActualHeight);

                    drawingContext.PushOpacity(1.0);

                    Brush highlight = mDraggedElement.TryFindResource(SystemColors.HighlightBrushKey) as Brush;

                    if (highlight != null)

                        drawingContext.DrawRectangle(highlight, new Pen(Brushes.Transparent, 0), rect);

                    drawingContext.DrawRectangle(new VisualBrush(mDraggedElement),

                        new Pen(Brushes.Transparent, 0), rect);

                    drawingContext.Pop();

                }
            }
        }
        
        FrameworkElement mDraggedElement = null;

    }
    public static class Win32

    {

        public struct POINT { public Int32 X; public Int32 Y; }
        
        // During drag-and-drop operations, the position of the mouse cannot be

        // reliably determined through GetPosition. This is because control of

        // the mouse (possibly including capture) is held by the originating

        // element of the drag until the drop is completed, with much of the

        // behavior controlled by underlying Win32 calls. As a workaround, you

        // might need to use Win32 externals such as GetCursorPos.

        [System.Runtime.InteropServices.DllImport("user32.dll")]

        public static extern bool GetCursorPos(ref POINT point);

    }
}

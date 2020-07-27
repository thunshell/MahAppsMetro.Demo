using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace MahAppsMetro.Demo.Views
{
    /// <summary>
    /// 双圆
    /// 全圆面板、左半圆面板、上半圆面板、右半圆、下半圆
    /// 顺时针、逆时针
    /// 可收扇开扇
    /// 可拖拽
    /// 可隐藏
    /// </summary>
    public class RadialPanel : Panel
    {
        public static readonly DependencyProperty OrientationProperty;

        bool showPieLines;
        double angleEach;       // 角度
        Size sizeLargest;       // 最大孩子的尺寸  
        double radius;

        // 圆的半径
        double outerEdgeFromCenter;
        double innerEdgeFromCenter;

        static RadialPanel()
        {
            OrientationProperty = DependencyProperty.Register("Orientation", typeof(RadialPanelOrientation), typeof(RadialPanel),
                    new FrameworkPropertyMetadata(RadialPanelOrientation.ByWidth, FrameworkPropertyMetadataOptions.AffectsMeasure));
        }

        public RadialPanelOrientation Orientation
        {
            set { SetValue(OrientationProperty, value); }
            get { return (RadialPanelOrientation)GetValue(OrientationProperty); }
        }

        public bool ShowPieLines
        {
            set
            {
                if (value != showPieLines)
                    InvalidateVisual();

                showPieLines = value;
            }
            get
            {
                return showPieLines;
            }
        }

        protected override Size MeasureOverride(Size sizeAvailable)
        {
            if (InternalChildren.Count == 0)
                return new Size(0, 0);

            angleEach = 360.0 / InternalChildren.Count;
            sizeLargest = new Size(0, 0);

            foreach (UIElement child in InternalChildren)
            {
                child.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));

                sizeLargest.Width = Math.Max(sizeLargest.Width, child.DesiredSize.Width);
                sizeLargest.Height = Math.Max(sizeLargest.Height, child.DesiredSize.Height);
            }
            if (Orientation == RadialPanelOrientation.ByWidth)
            {
                // 计算中心到element边缘的距离
                innerEdgeFromCenter = sizeLargest.Width / 2 / Math.Tan(Math.PI * angleEach / 360);
                outerEdgeFromCenter = innerEdgeFromCenter + sizeLargest.Height;
                // 以最大的孩子为基准,计算圆的半径
                radius = Math.Sqrt(Math.Pow(sizeLargest.Width / 2, 2) + Math.Pow(outerEdgeFromCenter, 2));
            }
            else
            {
                // 计算中心到element边缘的距离
                innerEdgeFromCenter = sizeLargest.Height / 2 / Math.Tan(Math.PI * angleEach / 360);
                outerEdgeFromCenter = innerEdgeFromCenter + sizeLargest.Width;

                // 以最大的孩子为基准,计算圆的半径
                radius = Math.Sqrt(Math.Pow(sizeLargest.Height / 2, 2) + Math.Pow(outerEdgeFromCenter, 2));
            }
            // 返回该圆的尺寸
            return new Size(2 * radius, 2 * radius);
        }

        protected override Size ArrangeOverride(Size sizeFinal)
        {
            double angleChild = 0;
            Point ptCenter = new Point(sizeFinal.Width / 2, sizeFinal.Height / 2);
            double multiplier = Math.Min(sizeFinal.Width / (2 * radius), sizeFinal.Height / (2 * radius));

            foreach (UIElement child in InternalChildren)
            {
                // 重置孩子呈现位置的转换信息
                child.RenderTransform = Transform.Identity;

                if (Orientation == RadialPanelOrientation.ByWidth)
                {
                    // 将孩子放在上边
                    child.Arrange(
                        new Rect(ptCenter.X - multiplier * sizeLargest.Width / 2,
                                 ptCenter.Y - multiplier * outerEdgeFromCenter,
                                 multiplier * sizeLargest.Width,
                                 multiplier * sizeLargest.Height));
                }
                else
                {
                    // 将孩子放在右边
                    child.Arrange(
                        new Rect(ptCenter.X + multiplier * innerEdgeFromCenter,
                                 ptCenter.Y - multiplier * sizeLargest.Height / 2,
                                 multiplier * sizeLargest.Width,
                                 multiplier * sizeLargest.Height));
                }

                // 旋转孩子
                Point pt = TranslatePoint(ptCenter, child);
                child.RenderTransform = new RotateTransform(angleChild, pt.X, pt.Y);

                // 增加角度,准备安置下一个孩子
                angleChild += angleEach;
            }
            return sizeFinal;
        }

        // 显示饼图切线
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (ShowPieLines)
            {
                Point ptCenter = new Point(RenderSize.Width / 2, RenderSize.Height / 2);
                double multiplier = Math.Min(RenderSize.Width / (2 * radius), RenderSize.Height / (2 * radius));
                Pen pen = new Pen(SystemColors.WindowTextBrush, 1);
                pen.DashStyle = DashStyles.Dash;

                // 显示圆
                dc.DrawEllipse(null, pen, ptCenter, multiplier * radius, multiplier * radius);
                // 初始化角度
                double angleChild = angleEach / 2;
                if (Orientation == RadialPanelOrientation.ByHeight) angleChild += 90;

                // 循环走过孩子,从中心绘制放射线
                foreach (UIElement child in InternalChildren)
                {
                    dc.DrawLine(pen, ptCenter,
                        new Point(ptCenter.X + multiplier * radius * Math.Sin(2 * Math.PI * angleChild / 360),
                                  ptCenter.Y - multiplier * radius * Math.Cos(2 * Math.PI * angleChild / 360)));
                    angleChild += angleEach;
                }
            }
        }
    }

    public enum RadialPanelOrientation
    {
        ByWidth,
        ByHeight
    }
}
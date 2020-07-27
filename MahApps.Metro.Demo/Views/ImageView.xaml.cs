using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for ImageView.xaml
    /// </summary>
    public partial class ImageView : UserControl
    {
        public ImageView()
        {
            InitializeComponent();
        }


        bool mouseDown = false;
        Point prePo;
        private void Img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mouseDown = true;
            Cursor = Cursors.Hand;
            prePo = e.GetPosition(canvas);
        }


        /// <summary>
        /// 移动图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Img_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point currentPoint = e.GetPosition(canvas);
            if (mouseDown)
            {
                double left = Canvas.GetLeft(img);
                double top = Canvas.GetTop(img);
                if (currentPoint.X != prePo.X)
                    Canvas.SetLeft(img, left + currentPoint.X - prePo.X);
                if (currentPoint.Y != prePo.Y)
                    Canvas.SetTop(img, top + currentPoint.Y - prePo.Y);
                prePo = currentPoint;
            }
        }

        private void Img_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mouseDown = false;
            Cursor = Cursors.Arrow;
        }
        
        double scale = 1;
        /// <summary>
        /// 放大、缩小图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Img_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            double left = Canvas.GetLeft(img);
            double top = Canvas.GetTop(img);
            double preW = img.ActualWidth;
            double preH = img.ActualHeight;

            if (e.Delta > 0)
                scale = scale + 0.01;
            else
                scale = scale - 0.01;
            if (scale <= 0.01)
                scale = 0.01;
            img.RenderTransform = new ScaleTransform(scale, scale);
        }

        /// <summary>
        /// 图片自适应显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDefault_Click(object sender, RoutedEventArgs e)
        {
            scale = 1;
            img.RenderTransform = new ScaleTransform(1, 1);
            InitImage();
        }

        /// <summary>
        /// 还原图片居中缩放适合尺寸显示
        /// 当图片宽（高）大于高（宽）时，如果图片宽（高）超过显示区域宽（高）则进行缩放
        /// 然后垂直水平居中
        /// </summary>
        void InitImage()
        {
            double areaH = canvas.ActualHeight;
            double areaW = canvas.ActualWidth;
            if (img.Width == 0) return;
            if(img.Width > img.Height)
            {
                if (img.Width > areaW)
                {
                    double scale = areaW / img.Width;
                    //img.RenderTransform = new ScaleTransform(scale, scale);
                    double scaleH = img.Height * scale;
                    Canvas.SetTop(img, (areaH - scaleH) / 2);
                    Canvas.SetLeft(img, 0);
                    img.Width = img.Width * scale;
                    img.Height = scaleH;
                    return;
                }
                Canvas.SetTop(img, (areaH - img.Height) / 2);
                Canvas.SetLeft(img, (areaW - img.Width) / 2);
            }
            else
            {
                if (img.Height > areaH)
                {
                    double scale = areaH / img.Height;
                    //img.RenderTransform = new ScaleTransform(scale, scale);
                    double scaleW = img.Width * scale;
                    Canvas.SetTop(img, 0);
                    Canvas.SetLeft(img, (areaW - scaleW) / 2);
                    img.Width = scaleW;
                    img.Height = img.Height * scale;
                    return;
                }
                Canvas.SetTop(img, (areaH - img.Height) / 2);
                Canvas.SetLeft(img, (areaW - img.Width) / 2);
            }
        }

        /// <summary>
        /// 上一张
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPreview_Click(object sender, RoutedEventArgs e)
        {
            if (images.Count <= 0) return;
            index = --index < 0 ? images.Count - 1 : index;
            ChangeSelect(index);
        }

        /// <summary>
        /// 下一张
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (images.Count <= 0) return;
            index = ++index >= images.Count ? 0 : index;
            ChangeSelect(index);
        }

        /// <summary>
        /// 打开图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnImage_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Title = "请选择图片文件";
            fileDialog.Filter = filter;
            fileDialog.RestoreDirectory = true;
            if(fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbNotice.Visibility = Visibility.Collapsed;
                if (fileDialog.FileNames.Length > 1)
                    LoadImages(fileDialog.FileNames);
                else
                    LoadFolder(System.IO.Path.GetDirectoryName(fileDialog.FileName), fileDialog.FileName);
            }
        }

        int index = 0;
        string folder = null;
        List<string> images = new List<string>();
        string filter = "图像文件|*.png;*.jpg;*.jpeg;*.bmp";
        /// <summary>
        /// 打开文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowser.Description = "请选择图片目录";
            if (folderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbNotice.Visibility = Visibility.Collapsed;
                folder = folderBrowser.SelectedPath;
                LoadFolder(folder);
            }
        }

        /// <summary>
        /// 切换当前图片
        /// </summary>
        /// <param name="select"></param>
        void ChangeSelect(int select)
        {
            if (select < 0 || images.Count < 1)
                return;
            if (select >= images.Count)
                select = images.Count - 1;

            string file = images[select];
            ChangeSelect(file);
        }

        /// <summary>
        /// 切换当前图片
        /// </summary>
        /// <param name="select"></param>
        void ChangeSelect(string select)
        {
            tbName.Text = System.IO.Path.GetFileName(select);
            if (!File.Exists(select))
            {
                int i = images.IndexOf(select);
                images.Remove(select);
                MessageBox.Show("图片丢失");
                if (images.Count <= 0)
                    tbNotice.Visibility = Visibility.Visible;
                else
                    ChangeSelect(i);
                return;
            }
            BitmapImage bitmap = new BitmapImage(new Uri(select));
            img.Source = bitmap;
            img.Width = bitmap.PixelWidth;
            img.Height = bitmap.Height;
            InitImage();
        }

        /// <summary>
        /// 加载文件到集合中
        /// </summary>
        /// <param name="files"></param>
        void LoadImages(string[] files)
        {
            images.Clear();
            images.AddRange(files);
            index = 0;
            ChangeSelect(index);
        }

        /// <summary>
        /// 加载目录图片到集合中
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="select"></param>
        void LoadFolder(string folder, string select = null)
        {
            images.Clear();
            string[] files = Directory.GetFiles(folder);
            if (files == null)
                return;
            foreach (var item in files)
            {
                if (filter.Contains(System.IO.Path.GetExtension(item.ToLowerInvariant())))
                    images.Add(item);
            }
            index = 0;
            ChangeSelect(select ?? images[0]);
        }
    }
}

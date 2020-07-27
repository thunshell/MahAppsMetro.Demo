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
using System.Xml.Linq;

namespace MahAppsMetro.Demo.Views
{
    public class XmlHelper
    {
    }
    /// <summary>
    /// ImageEditor.xaml 的交互逻辑
    /// </summary>
    public partial class ImageEditor : UserControl
    {
        public ImageEditor()
        {
            InitializeComponent();
            DataObject.AddPastingHandler(tbReply, OnPasting);
            Paragraph p = new Paragraph();
            p.Inlines.Add(new Run("tslff"));
            BitmapImage image = new BitmapImage(new Uri(@"C:\Users\ycx\Pictures\1.jpg"));
            p.Inlines.Add(new Image { Source = image, Width = image.PixelWidth });
            this.tbReply.Document.Blocks.Clear();
            this.tbReply.Document.Blocks.Add(p);
        }

        private void OnPasting(object sender, DataObjectPastingEventArgs e)
        {
            if(!this.tbReply.Selection.IsEmpty)
                EditingCommands.Delete.Execute(null, this.tbReply);
            string[] type = e.SourceDataObject.GetFormats(true);
            if (type.Contains("QQ_Unicode_RichEdit_Format"))
            {
                var success = e.SourceDataObject.GetData("QQ_Unicode_RichEdit_Format");
                if (success is MemoryStream)
                {
                    using (StreamReader sr = new StreamReader((MemoryStream)success))
                    {
                        string content = sr.ReadToEnd();
                        PasteQQ(content.Substring(0, content.IndexOf("</QQRichEditFormat>") + "</QQRichEditFormat>".Length));
                        e.CancelCommand();
                    }
                }
            }
            return;
        }

        public void PasteQQ(string qqPastString)
        {
            XDocument doc = XDocument.Parse(qqPastString);
            string version = doc.Root.Element("Info").FirstAttribute.Value;
            var items = doc.Root.Elements("EditElement");


            for (int i = 0; i <= items.Count() - 1; i++)
                //for (int i = items.Count() - 1; i >= 0; i--)
            {
                var item = items.ElementAt(i);
                switch (item.Attribute("type").Value)
                {
                    //文本
                    case "0":
                        InsertInline(new Run(item.Value)); break;
                    case "1": //图片
                        string path = item.Attribute("filepath").Value;
                        BitmapImage image = new BitmapImage(new Uri(path));
                        InsertImage(new Image { Source = image, Width = image.Width});
                        break;
                    default: break;
                }
            }
        }

        void InsertImage(Image image)
        {
            InlineUIContainer inlineUIContainer = new InlineUIContainer(image, this.tbReply.CaretPosition);
            this.tbReply.Selection.Select(inlineUIContainer.ElementEnd, inlineUIContainer.ElementEnd);
        }

        void InsertInline(Inline inline)
        {
            using(MemoryStream ms = new MemoryStream())
            {
                TextRange textRange = new TextRange(inline.ContentStart, inline.ContentEnd);
                textRange.Save(ms, DataFormats.XamlPackage);
                ms.Position = 0;

                TextRange tr = new TextRange(this.tbReply.CaretPosition, this.tbReply.CaretPosition.GetInsertionPosition(LogicalDirection.Backward));
                tr.Load(ms, DataFormats.XamlPackage);
                this.tbReply.Selection.Select(tr.End,tr.End);
            }
        }
    }
}

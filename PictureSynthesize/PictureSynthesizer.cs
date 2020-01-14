using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace PictureSynthesize
{
    /// <summary>
    /// 合成
    /// </summary>
    public class PictureSynthesizer
    {
        /// <summary>
        /// 底图
        /// </summary>
        private Bitmap _basemap;

        private Graphics _graphics;

        /// <summary>
        /// 支持的图片类型
        /// </summary>
        private static string[] _supportImageFormat = new[] { ".jpg", ".png" };

        /// <summary>
        /// 支持的文本类型
        /// </summary>
        private static string[] _supportTextFormat = new[] { ".txt" };

        /// <summary>
        /// 支持的类型
        /// </summary>
        private static string[] _supportFormat
        {
            get => _supportImageFormat.Concat(_supportTextFormat).ToArray();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pixelW">图片的宽度</param>
        /// <param name="pixelH">图片的高度</param>
        /// <param name="background">图片的底色</param>
        public PictureSynthesizer(int pixelW, int pixelH, Color background)
        {
            _basemap = new Bitmap(pixelW, pixelH);
            _graphics = Graphics.FromImage(_basemap);
            _graphics.Clear(background);
        }

        /// <summary>
        /// 添加一个合成的对象
        /// </summary>
        /// <param name="text">可以是一段文字描述，或文件路径（）</param>
        /// <param name="area"></param>
        /// <returns></returns>
        public bool Add(string text, RectangleF area)
        {
            RectangleF baseArea = new RectangleF(0, 0, _basemap.Width, _basemap.Height);
            //if (_basemap.Width < area.X + area.Width || _basemap.Height < area.Y + area.Height)
            //    return false;

            if (!baseArea.Contains(area))
                return false;
            string ext = string.Empty;
            if (File.Exists(text))
            {
                ext = Path.GetExtension(text);
            }
            if (string.IsNullOrWhiteSpace(ext) || !_supportFormat.Contains(ext.ToLower()))
            {
                return AddText(text, area);
            }
            else if (_supportTextFormat.Contains(ext))
            {
                string fileText;
                using (var fs = new FileStream(text, FileMode.Open, FileAccess.Read))
                {
                    using (var sr = new StreamReader(fs))
                    {
                        fileText = sr.ReadToEnd();
                    }
                }

                return AddText(fileText, area);
            }
            else
            {
                var img = new Bitmap(text);
                return AddImage(img, area);
            }
        }

        /// <summary>
        /// 添加文本
        /// </summary>
        /// <param name="text"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        private bool AddText(string text, RectangleF area)
        {
            int fontHeight = 12;
            //FontFamily ff = SystemFonts.DefaultFont.FontFamily;
            FontFamily ff = new FontFamily("黑体");
            Font font = new Font(ff, fontHeight, GraphicsUnit.Pixel);
            StringFormat sf = new StringFormat()
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Near,
                FormatFlags = StringFormatFlags.MeasureTrailingSpaces,
            };
            do
            {
                var ms = _graphics.MeasureString(text, font, area.Size, sf);
                if (ms.Height == area.Height)
                {
                    font = new Font(ff, --fontHeight, GraphicsUnit.Pixel);
                    break;
                }
                else
                {
                    font = new Font(ff, ++fontHeight, GraphicsUnit.Pixel);
                }
            } while (fontHeight != 0);

            if (fontHeight == 0)
                return false;

            _graphics.DrawString(text, font, Brushes.Black, area);
            _graphics.Save();
            return true;

        }

        /// <summary>
        /// public
        /// </summary>
        /// <param name="img"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        private bool AddImage(Bitmap img, RectangleF area)
        {
            var imgRatio = (float)img.Width / img.Height;
            var areaRatio = (float)area.Width / area.Height;

            float x;
            float y;
            float width;
            float height;

            if (imgRatio > areaRatio)
            {
                width = area.Width;
                height = img.Height * (area.Width / img.Width);
                x = area.X;
                y = (area.Height - height) / 2 + area.Y;
            }
            else
            {
                height = area.Height;
                width = img.Width * (area.Height / img.Height);
                y = area.Y;
                x = (area.Width - width) / 2 + area.X;
            }

            var actualArea = new RectangleF(x, y, width, height);

            _graphics.DrawImage(img, actualArea);
            _graphics.Save();
            img.Dispose();

            return true;
        }

        /// <summary>
        /// 合成
        /// </summary>
        /// <param name="path">合成图存放的路径</param>
        public void Synthesize(string path)
        {
            try
            {
                var folderPath = path.Replace(Path.GetFileName(path), string.Empty);
                if (!string.IsNullOrWhiteSpace(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                _basemap.Save(path);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _graphics.Dispose();
                _basemap.Dispose();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace CompressedImageDemo
{
    public partial class Form1 : Form
    {
        private string fileName;
        public Form1()
        {
            InitializeComponent();
        }

        private void CompressedImage(string fileName, long quality)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            Byte[] bytePic = new Byte[fs.Length];
            fs.Read(bytePic, 0, bytePic.Length);
            MemoryStream stream = new MemoryStream(bytePic);
            Bitmap bmp = (Bitmap)Image.FromStream(stream);
            ImageCodecInfo myImageCodecInfo = ImageCodecInfo.GetImageEncoders()[1];  //默认为jpeg
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            for (int j = 0; j < encoders.Length; j++)
            {
                if (encoders[j].MimeType == "image/jpeg")
                {
                    myImageCodecInfo = encoders[j];
                    break;
                }
            }
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;  //要操作的是质量
            EncoderParameters myEncoderParameters = new EncoderParameters(1);      //一个成员
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, quality);    //0L为最差质量
            myEncoderParameters.Param[0] = myEncoderParameter;
            Size s = new Size(bmp.Width, bmp.Height);
            Bitmap newBmp = new Bitmap(bmp, s);
            MemoryStream ms = new MemoryStream();
            newBmp.Save(ms, myImageCodecInfo, myEncoderParameters);    //压缩后的流保存到ms
            //从流中还原图片
            Image image = Image.FromStream(ms);
            string curDirectory = Path.GetDirectoryName(Assembly.GetCallingAssembly().GetModules()[0].FullyQualifiedName) + "\\";
            //保存图片
            image.Save(curDirectory + "pic.jpg");
            fs.Dispose();
            stream.Dispose();
            newBmp.Dispose();
            ms.Dispose();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialog1.FileName;

            }
            else
            {
                MessageBox.Show("请选择图片");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(textBox1.Text))
            {
                long quality = long.Parse(textBox1.Text);
                CompressedImage(fileName, quality);
            }
        }
    }
}

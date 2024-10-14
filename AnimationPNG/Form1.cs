using AnimationPNG.Decoder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnimationPNG
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Timer.Tick += Timer_Tick;
        }
        Bitmap After = null;
        private void Timer_Tick(object sender, EventArgs e)
        {
            Bitmap D = (Bitmap)pictureBox1.Image;
            if (After != null)
            {
                D = After;
                After = null;
            }
            Bitmap bm = new Bitmap(D);


            AFrame af = Dc.aFrames[Frameindex];
            switch (af.DisposeType)
            {
                case 0:
                    break;
                case 1:
                    After = new Bitmap((int)Dc.Image.Width, (int)Dc.Image.Height);
                    Graphics graphics1 = Graphics.FromImage(After);
                    graphics1.Clear(Color.Transparent);
                    break;
                case 2:
                    After = new Bitmap(D);
                    break;
            }

            Graphics graphics = Graphics.FromImage(bm);
            PixelFormat pixel;
            Timer.Interval = af.time;

            if (Dc.Image.PixelSize == 4)
            {
                pixel = PixelFormat.Format32bppArgb;
            }
            else
            {
                pixel = PixelFormat.Format24bppRgb;
            }
            byte[] data = af.ChengeDotNet(af.LineDatas());
            graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            Bitmap bitmap1 = new Bitmap((int)af.FWidth,(int)af.FHeight, pixel);
            {
                BitmapData bd = bitmap1.LockBits(new Rectangle(0, 0, (int)af.FWidth, (int)af.FHeight), ImageLockMode.WriteOnly, pixel);
                Marshal.Copy(data, 0, bd.Scan0, data.Length);

                bitmap1.UnlockBits(bd);


            }
            graphics.DrawImage(bitmap1,(int)af.x,(int) af.y,af.FWidth,af.FHeight);

            graphics.Dispose();
            bitmap1.Dispose();

            pictureBox1.Image = bm;
            
            Frameindex++;
            if (Dc.aFrames.Count == Frameindex)
            {
                Frameindex = 0;
                //Timer.Stop();
            }
            //if (Dc.aFrames.Count == Frameindex)
            //{
            //    Frameindex = 0;
            //    PixelFormat pixel1;
            //    AFrame af1 = Dc.aFrames[Frameindex];
            //    if (Dc.Image.PixelSize == 4)
            //    {
            //        pixel1 = PixelFormat.Format32bppArgb;
            //    }
            //    else
            //    {
            //        pixel1 = PixelFormat.Format24bppRgb;
            //    }
            //    byte[] data1 = af1.ChengeDotNet(af1.LineDatas());
            //    Bitmap bitmap12 = new Bitmap((int)af1.FWidth, (int)af1.FHeight, pixel1);
            //    {
            //        BitmapData bd = bitmap12.LockBits(new Rectangle(0, 0, (int)af1.FWidth, (int)af1.FHeight), ImageLockMode.WriteOnly, pixel1);
            //        Marshal.Copy(data1, 0, bd.Scan0, data1.Length);

            //        bitmap12.UnlockBits(bd);
            //    }
            //    System.Drawing.Image im = pictureBox1.Image;
            //    pictureBox1.Image = bitmap12;

            //    if (im != null)
            //    {
            //        im.Dispose();
            //    }
            //    Frameindex = 1;
            //}
            
        }
        Decoder.Decode Dc;
        int Frameindex = 0;
        Timer Timer = new Timer();
        private void button1_Click(object sender, EventArgs e)
        {
            Timer.Stop();
            var d = new Decoder.Decode(File.ReadAllBytes(textBox1.Text));
            byte[] data = d.Image.LineDatas();
            data=d.Image.ChangeDotNet(data);
            PixelFormat pixel;

            if (d.Image.PixelSize == 4)
            {
                pixel = PixelFormat.Format32bppArgb;
            }
            else
            {
                pixel = PixelFormat.Format24bppRgb;
            }
            Bitmap bitmap1 = new Bitmap((int)d.Image.Width, (int)d.Image.Height, pixel);
            {
                BitmapData bd = bitmap1.LockBits(new Rectangle(0, 0, (int)d.Image.Width, (int)d.Image.Height), ImageLockMode.WriteOnly, pixel);
                Marshal.Copy(data, 0, bd.Scan0, data.Length);

                bitmap1.UnlockBits(bd);
            }
            System.Drawing.Image im = pictureBox1.Image;
            pictureBox1.Image = bitmap1;
                
            if (im != null)
            {
                im.Dispose();
            }

            Dc = d;
            
        }
        int findex = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            var d = new Decoder.Decode(File.ReadAllBytes(textBox1.Text));
            Dc = d;
            Bitmap bitmap = new Bitmap((int)d.Image.Height, (int)d.Image.Width);
            pictureBox1.Image = bitmap;
            Frameindex = 0;
            Timer.Start();
        }

        Bitmap bitmap2;
        private void button3_Click(object sender, EventArgs e)
        {
            Timer.Stop();
            bitmap2 = new Bitmap(textBox1.Text);
            pictureBox1.Image = bitmap2;


            // pictureBoxのPaintイベントハンドラ
            pictureBox1.Paint += PictureBox1_Paint ;


            // アニメーション開始
            ImageAnimator.Animate(bitmap2, new EventHandler(Image_FrameChanged));
            MessageBox.Show(ImageAnimator.CanAnimate(bitmap2).ToString());
        }
        private void Image_FrameChanged(object o, EventArgs e)
        {
            // Paintイベントハンドラを呼び出す
            pictureBox1.Invalidate();
        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            ImageAnimator.UpdateFrames(bitmap2);
            // 画像の描画
            e.Graphics.DrawImage(bitmap2, 0, 0);
        }
    }
    
}

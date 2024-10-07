using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimationPNG.Decoder
{
    class AFrame
    {
        byte[,] Data;

        internal int PixelSize;
        internal uint FHeight;
        internal uint FWidth;
        internal int time;
        internal int DisposeType;
        internal int Blend;
        internal uint x, y;
        internal AFrame(IHDR iHRD, fcTL fctl, IDAT[] fdats)
        {
            x = fctl.X_Offset;
            y = fctl.Y_Offset;
            DisposeType = fctl.Dispose_Op;
            Blend = fctl.Blend_Op;
            time = (int)((float)(fctl.Delay_Num) / (float)(fctl.Delay_Den) * 1000);
            FHeight = fctl.Height;
            FWidth = fctl.Width;
            int colorCount = 0;
            if (iHRD.BitDepth != 8)
            {
                return;
            }
            switch (iHRD.ColorType)
            {
                case 0:
                    colorCount = 1;

                    break;
                case 2:
                    colorCount = 3;
                    break;
                case 3:
                    colorCount = 1;
                    break;
                case 4:
                    colorCount = 2;
                    break;
                case 6:
                    colorCount = 4;
                    break;
            }
            PixelSize = colorCount;
            Data = new byte[FWidth * colorCount, FHeight];
            int wi = 0, hi = -1;
            int type = 0;
            List<byte> tdata = new List<byte>();
            for (int i = 0; i < fdats.Length; i++)
            {
                tdata.AddRange(fdats[i].Data);

            }
            byte[] decomp = Decompress(tdata.ToArray());
            for (int ii = 0; ii < decomp.Length; ii++)
            {
                int l = Data.GetLength(0);
                if (ii == 0 || wi == l)
                {
                    type = decomp[ii];
                    wi = 0;
                    hi++;
                }
                else
                {
                    byte lef = 0;
                    byte tlef = 0;
                    byte up = 0;
                    if (wi < colorCount == false)
                    {
                        lef = Data[wi - colorCount, hi];
                        if (hi != 0)
                        {
                            tlef = Data[wi - colorCount, hi - 1];

                        }
                        else
                        {

                        }
                    }
                    if (hi != 0)
                    {
                        up = Data[wi, hi - 1];
                    }
                    switch (type)
                    {
                        case 0:

                            Data[wi, hi] = decomp[ii];
                            break;
                        case 1:

                            Data[wi, hi] = (byte)(lef + decomp[ii]);

                            break;
                        case 2:
                            Data[wi, hi] = (byte)(up + decomp[ii]);
                            break;
                        case 3:
                            Data[wi, hi] = (byte)(((lef + up) / 2) + decomp[ii]);
                            break;
                        case 4:
                            Data[wi, hi] = (byte)((PaethPredictor(lef, up, tlef) % 256) + decomp[ii]);
                            break;
                    }
                    wi++;
                }
            }
        }
        byte PaethPredictor(byte a, byte b, byte c)
        {
            int p = a + b - c;

            // pa = |b - c|　　　横向きの値の変わり具合
            // pb = |a - c|　　　縦向きの値の変わり具合
            // pc = |b-c + a-c|　↑ふたつの合計
            int pa = Math.Abs(p - a);
            int pb = Math.Abs(p - b);
            int pc = Math.Abs(p - c);

            // 横向きのほうがなだらかな値の変化 → 左
            if (pa <= pb && pa <= pc)
                return a;

            // 縦向きのほうがなだらかな値の変化 → 上
            if (pb <= pc)
                return b;

            // 縦横それぞれ正反対に値が変化するため中間色を選択 → 左上        
            return c;
        }
        protected static byte[] Decompress(byte[] src)
        {

            byte[] tmp = new byte[src.Length - 6];
            Array.Copy(src, 2, tmp, 0, tmp.Length);
            using (var ms = new MemoryStream(tmp))
            using (var ds = new DeflateStream(ms, CompressionMode.Decompress))
            {

                using (var dest = new MemoryStream())
                {
                    ds.CopyTo(dest);

                    dest.Position = 0;
                    byte[] decomp = new byte[dest.Length];
                    dest.Read(decomp, 0, decomp.Length);
                    ds.Dispose();
                    ms.Dispose();
                    return decomp;
                }
            }
            


        }
        internal byte[] LineDatas()
        {
            byte[] rdata = new byte[Data.GetLength(0) * Data.GetLength(1)];
            int index = 0;
            for (int i = 0; i < Data.GetLength(1); i++)
            {
                for (int ii = 0; ii < Data.GetLength(0); ii++)
                {
                    rdata[index] = (byte)(Data[ii, i]);
                    index++;
                }
            }
            return rdata;
        }
        internal byte[] ChengeDotNet(byte[] data)
        {
            int next = PixelSize;
            byte t1;
            for (int i = 0; i < data.Length; i += next)
            {

                t1 = data[i];
                data[i] = data[i + 2];
                data[i + 2] = t1;
            }
            return data;
        }
    }
}

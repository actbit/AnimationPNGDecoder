using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimationPNG.Decoder
{
    class IHDR:ChunckData
    {
        internal IHDR(byte[] data)
        {
            byte[] byte4 = new byte[4];
            Array.Copy(data, 0, byte4, 0, 4);
            Array.Reverse(byte4);
            Width=BitConverter.ToUInt32(byte4,0);
            Array.Copy(data, 4, byte4, 0, 4);
            Array.Reverse(byte4);
            Height = BitConverter.ToUInt32(byte4, 0);
            BitDepth = data[8];
            ColorType = data[9];
            Compression= data[10];
            Filter = data[11];
            Interlace = data[12];

        }
        internal uint Height;
        internal uint Width;
        internal byte BitDepth;
        internal byte ColorType;
        internal byte Compression;
        internal byte Filter;
        internal byte Interlace;
    }
}

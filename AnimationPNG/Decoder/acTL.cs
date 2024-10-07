using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimationPNG.Decoder
{
    class acTL:ChunckData
    {
        internal acTL(byte[] data)
        {
            byte[] tmp = new byte[4];
            Array.Copy(data, 0, tmp, 0, 4);
            Array.Reverse(tmp);
            Num_Frames = BitConverter.ToUInt32(tmp,0);
            Array.Copy(data, 4, tmp, 0, 4);
            Array.Reverse(tmp);
            NumPlays=BitConverter.ToUInt32(tmp, 0);
        }
        internal uint Num_Frames;
        internal uint NumPlays;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimationPNG.Decoder
{
    class fdAT:IDAT
    {
        internal fdAT(byte[] data)
        {
            byte[] tmp = new byte[4];
            Array.Copy(data, 0, tmp, 0, 4);
            Array.Reverse(tmp); 
            Sequence_Number=BitConverter.ToUInt32(tmp, 0);
            byte[] tmp1 = new byte[data.Length - 4];
            Array.Copy(data, 4, tmp1, 0, tmp1.Length);
            Data = (tmp1);
        }
        internal uint Sequence_Number;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimationPNG.Decoder
{
    class fcTL:ChunckData
    {
        internal fcTL(byte[] data) :base(data)
        {
            byte[] tmp=new byte[4];
            Array.Copy(data, 0, tmp, 0, 4);
            Array.Reverse(tmp);
            Sequence_Number=BitConverter.ToUInt32(tmp, 0);
            Array.Copy(data, 4, tmp, 0, 4);
            Array.Reverse(tmp);
            Width = BitConverter.ToUInt32(tmp,0);
            Array.Copy(data, 8, tmp,0, 4);
            Array.Reverse(tmp);
            Height = BitConverter.ToUInt32(tmp, 0);
            Array.Copy(data, 12, tmp, 0, 4);
            Array.Reverse(tmp);
            X_Offset = BitConverter.ToUInt32(tmp,0);
            Array.Copy(data, 16, tmp, 0, 4);
            Array.Reverse(tmp);
            Y_Offset = BitConverter.ToUInt32(tmp, 0);
            byte[] tmp2 = new byte[2];
            Array.Copy(data, 20, tmp2, 0,2);
            Array.Reverse(tmp2);
            Delay_Num = BitConverter.ToUInt16(tmp2,0);
            Array.Copy(data, 22, tmp2, 0, 2);
            Array.Reverse(tmp2);
            Delay_Den = BitConverter.ToUInt16(tmp2, 0);
            Dispose_Op = data[24];
            Blend_Op = data[25];
        }
        internal uint Sequence_Number;
        internal uint Width;
        internal uint Height;
        internal uint X_Offset;
        internal uint Y_Offset;
        internal ushort Delay_Num;
        internal ushort Delay_Den;
        internal byte Dispose_Op;
        internal byte Blend_Op;
    }
}

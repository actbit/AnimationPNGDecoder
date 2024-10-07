using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimationPNG.Decoder
{
    class IDAT:ChunckData
    {
        protected IDAT()
        {

        }
        internal IDAT(byte[] data)
        {
            Data = data;
        }

        internal byte[] Data;
        
        //internal void AddData(byte[] data)
        //{
        //    byte[] tmp= new byte[Data.Length];
        //    Data.CopyTo(tmp, 0);
        //    //tmp = Decompress(tmp);
        //    Data = new byte[tmp.Length + data.Length];
        //    Array.Copy(tmp, 0, Data, 0, tmp.Length);
        //    Array.Copy(data, 0, Data, tmp.Length, data.Length);

        //}
        
        //protected static byte[] Decompress (byte[] src)
        //{
        //    using (var ms = new MemoryStream(src))
        //    using (var ds = new DeflateStream(ms, CompressionMode.Decompress))
        //    {
        //        using (var dest = new MemoryStream())
        //        {
        //            ds.CopyTo(dest);

        //            dest.Position = 0;
        //            byte[] decomp = new byte[dest.Length];
        //            dest.Read(decomp, 0, decomp.Length);
        //            return decomp;
        //        }
        //    }
        //}
    }
}

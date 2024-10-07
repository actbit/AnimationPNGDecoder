using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimationPNG.Decoder
{
    class Chunck
    {
        internal Chunck(uint length, string type,byte[] data,byte[] crc)
        {
            Length = length;
            Type = type;
            Data = data;
            CRC = crc;
            switch (type)
            {
                case "IDAT":
                    ChunckData = new IDAT(data);
                    break;
                case "IHDR":
                    ChunckData = new IHDR(data);
                    break;
                case "fcTL":
                    ChunckData = new fcTL(data);
                    break;
                case "fdAT":
                    ChunckData = new fdAT(data);
                    break;
                case "acTL":
                    ChunckData = new acTL(data);
                    break;
            }
        }
        internal ChunckData ChunckData;
        internal uint Length;
        internal string Type;
        internal byte[] Data;
        internal byte[] CRC;
    }
}

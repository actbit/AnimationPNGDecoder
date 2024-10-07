using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimationPNG.Decoder
{
    class Decode
    {
        Color[][] colors;
        internal Image Image;
        internal List<AFrame> aFrames = new List<AFrame>();
        internal acTL acTL;
        internal Decode(byte[] data)
        {

            List<Chunck> datas = new List<Chunck>();
            byte[] name=new byte[8];
            Array.Copy(data, 0, name, 0, 8);
            uint size = 0;            
            if (System.Linq.Enumerable.SequenceEqual(name, new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A })) 
            {
                for(long i = 8; i < data.Length; )
                {
                    byte[] sizeb = new byte[4];
                    byte[] crc = new byte[4];
                    
                    byte[] typeb = new byte[4];
                    Array.Copy(data, i, sizeb, 0, 4);
                    Array.Reverse(sizeb);
                    size =BitConverter.ToUInt32(sizeb,0);
                    Array.Copy(data, i + 4, typeb, 0, 4);
                    byte[] tmpdata = new byte[size];
                    Array.Copy(data, i + 8, tmpdata, 0, size);
                    Array.Copy(data, i + 8+size,crc , 0, 4);
                    Chunck chunck = new Chunck(size, System.Text.Encoding.ASCII.GetString(typeb),tmpdata,crc);
                    datas.Add(chunck);
                    i = i + 12 + size;
                }
            }
            IHDR iHRD=null;
            bool afterIDAT=false;
            List<IDAT> IDATs = new List<IDAT>();
            List<IDAT> IDATs1 = new List<IDAT>();

            fcTL fcTL1 = null;
            for (int i = 0; i < datas.Count; i++)
            {

                if (afterIDAT == false && datas[i].Type == "IHDR")
                {
                    iHRD = (IHDR)datas[i].ChunckData;
                }
                else if (datas[i].Type == "IDAT")
                {
                    afterIDAT = true;
                    IDATs.Add((IDAT)datas[i].ChunckData);
                }
                if(datas[i].Type == "acTL")
                {
                    acTL = (acTL)datas[i].ChunckData;
                }
                if (datas[i].Type == "fcTL")
                {
                    if (fcTL1 != null)
                    {
                        aFrames.Add(new AFrame(iHRD, fcTL1, IDATs1.ToArray()));
                        IDATs1 = new List<IDAT>();
                    }
                    fcTL1 = (fcTL)datas[i].ChunckData;
                }
                else if (fcTL1!=null)
                {
                    if (datas[i].Type == "fdAT")
                    {
                        IDATs1.Add((fdAT)datas[i].ChunckData);
                    }
                    else if (datas[i].Type == "IDAT")
                    {
                        IDATs1.Add((IDAT)datas[i].ChunckData);
                    }
                    else
                    {
                        aFrames.Add(new AFrame(iHRD, fcTL1, IDATs1.ToArray()));
                        fcTL1 = null;
                        IDATs1 = new List<IDAT>();
                    }
                }

            }
            Image = new Image(iHRD, IDATs.ToArray());
        }
    }
}

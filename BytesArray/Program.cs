using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesArray
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] data1 = { 0x12, 0x13, 0x14 };
            byte[] data2 = { 0x14, 0x15, 0x16 };
            byte[] data3 = new byte[data1.Length + data2.Length];

            Stream s = new MemoryStream();
            s.Write(data1, 0, data1.Length);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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

            Buffer.BlockCopy(data1, 0, data3, 0, data1.Length);
            Buffer.BlockCopy(data2, 0, data3, data1.Length, data2.Length);
            
            //合并数据性能比较。http://q.cnblogs.com/q/30534/
            //Array.Copy(data1, 0, data3, 0, data1.Length);
            //Array.Copy(data2, 0, data3, data1.Length, data2.Length);

            for (int i = 0; i < data3.Length; i++)
            {
                Console.WriteLine("{0:X}", data3[i]);
            }
            Console.ReadLine();

            Stream stream = new MemoryStream();
            stream.Write(data1, 0, data1.Length);
            stream.Write(data2, 0, data2.Length);
            stream.Position = 0;

            int count = stream.Read(data3, 0, data3.Length);

            for (int i = 0; i < count; i++)
            {
                Console.WriteLine("{0:X}",data3[i]);    
            }

            Console.WriteLine(Convert.ToString(12, 2));
            Console.WriteLine(Convert.ToString(12, 8));
            //16进制12转化为二进制10010  十进制12转化为二进制01100 二进制B 八进制O 十六进制0x
            Console.WriteLine(Convert.ToInt32("FF", 16)); //16进制FF转化为十进制255
            Console.WriteLine(Convert.ToInt32("01100", 2)); //二进制01100转化为十进制12
            Console.ReadLine();

            Player player = new Player() { Id = 12138, Name = "LiuSmile", Life = 8888 };

            BinaryFormatter bf = new BinaryFormatter();

            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, player);

                ms.Seek(0, SeekOrigin.Begin);

                Player obj = (Player)bf.Deserialize(ms);

                Console.WriteLine(" \n" + obj.Id + " \n" + obj.Name + " \n" + obj.Life);
            }
        }
    }

    [System.Serializable]
    public class Player
    {
        public int Id;
        public string Name;
        public int Life;
    }

}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace UnityNetwork
{
    /// <summary>
    /// 完成内置类型和Byte数组的转换。
    /// </summary>
    public class NetBitStream
    {
        public Socket SocketObj = null;

        public const int HeaderLength = 4;

        public const int MaxBodyLength = 512;

        public const int INT32_LEN = 4;

        /************************************************/

        public int ReadLength
        { get; set; }

        public int BodyLength
        { get; set; }

        public int Length
        {
            get { return HeaderLength + BodyLength; }
        }

        public byte[] Bytes
        { get; set; }

        public NetBitStream()
        {
            ReadLength = 0;

            BodyLength = 0;

            Bytes = new Byte[HeaderLength + MaxBodyLength];
        }

        public void Reset()
        {
            ReadLength = 0;

            BodyLength = 0;
        }

        /************************************************/

        public void BeginWrite(string msg)
        {
            BodyLength = 0;

            this.WriteString(msg);
        }

        public void WriteInt(int number)
        {
            if (BodyLength + INT32_LEN > MaxBodyLength) return;

            byte[] bs = System.BitConverter.GetBytes(number);

            bs.CopyTo(Bytes, HeaderLength + BodyLength);

            BodyLength += INT32_LEN;
        }

        public void WriteString(string str)
        {
            int len = System.Text.Encoding.UTF8.GetByteCount(str);

            this.WriteInt(len);

            if (BodyLength + len > MaxBodyLength) return;

            System.Text.Encoding.UTF8.GetBytes(str, 0, str.Length, Bytes, HeaderLength + BodyLength);

            BodyLength += len;
        }

        public void WriteStream(byte[] bs)
        {
            this.WriteInt(bs.Length);

            if (BodyLength + bs.Length > MaxBodyLength) return;

            bs.CopyTo(bs, HeaderLength + BodyLength);

            BodyLength += bs.Length;
        }

        public void WriteObject<T>(T obj)
        {
            byte[] bs = Serialize<T>(obj);

            this.WriteStream(bs);
        }

        public void EncodeHeader()
        {
            byte[] bs = System.BitConverter.GetBytes(BodyLength);

            bs.CopyTo(Bytes, 0);
        }

        /************************************************/
        public void BeginRead(out string msg)
        {
            BodyLength = 0;

            ReadString(out msg);
        }

        public void ReadInt(out int number)
        {
            number = 0;

            if (BodyLength + INT32_LEN > MaxBodyLength) return;

            number = System.BitConverter.ToInt32(Bytes, HeaderLength + BodyLength);

            BodyLength += INT32_LEN;

        }

        public void ReadString(out string str)
        {
            str = "";

            int len = 0;

            ReadInt(out len);

            if (BodyLength + len > MaxBodyLength) return;

            str = System.Text.Encoding.UTF8.GetString(Bytes, HeaderLength + BodyLength, len);

            BodyLength += len;

        }

        public byte[] ReadStream()
        {
            int len = 0;

            this.ReadInt(out len);

            if (BodyLength + len > MaxBodyLength) return null;

            byte[] bs = new byte[len];

            for (int index = 0; index < len; index++)
            {
                bs[index] = Bytes[HeaderLength + BodyLength + index];
            }
            BodyLength += len;

            return bs;
        }

        public T ReadObject<T>()
        {
            byte[] bs = this.ReadStream();

            if (bs == null) return default(T);

            return this.Deserialize<T>(bs);
        }

        public void DecodeHeader()
        {
            BodyLength = System.BitConverter.ToInt32(Bytes, 0);
        }
        /************************************************/
        public byte[] Serialize<T>(T t)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();

                    bf.Serialize(stream, t);

                    stream.Seek(0, SeekOrigin.Begin);

                    return stream.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                    return null;
                }
            }
        }

        public T Deserialize<T>(byte[] bs)
        {
            using (MemoryStream ms = new MemoryStream(bs))
            {
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();

                    T t = (T)bf.Deserialize(ms);

                    return t;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                    return default(T);
                }
            }
        }
    }
}


#region RetainContent
        //public int Length 
        //{
        //    get { return HeaderLength + BodyLength; }
        //}


//public void WriteByte(byte bt)
//        {
//            if (BodyLength + BYTE_LEN > MaxBodyLength) return;

//            Bytes[HeaderLength + BodyLength] = bt;

//            BodyLength += BYTE_LEN;
//        }
        //public const int BYTE_LEN = 1;

        //public const int SHORT16_LEN = 2;

        //public const int FLOAT_LEN = 4;

//public void WriteBool(bool flag)
//{
//    if (BodyLength + BYTE_LEN > MaxBodyLength) return;

//    byte bt = flag ? (byte)'1' : (byte)'0';

//    Bytes[HeaderLength + BodyLength] = bt;

//    BodyLength += BYTE_LEN;
//}

//public void WriteUInt(uint number)
//{
//    if (BodyLength + INT32_LEN > MaxBodyLength) return;

//    byte[] bs = System.BitConverter.GetBytes(number);

//    bs.CopyTo(Bytes, HeaderLength + BodyLength);

//    BodyLength += INT32_LEN;
//}

//public void WriteShort(short number)
//{
//    if (BodyLength + SHORT16_LEN > MaxBodyLength) return;

//    byte[] bs = System.BitConverter.GetBytes(number);

//    bs.CopyTo(Bytes, HeaderLength + BodyLength);

//    BodyLength += SHORT16_LEN;
//}

//public void WriteUShort(ushort number)
//{
//    if (BodyLength + SHORT16_LEN > MaxBodyLength) return;

//    byte[] bs = System.BitConverter.GetBytes(number);

//    bs.CopyTo(Bytes, HeaderLength + BodyLength);

//    BodyLength += SHORT16_LEN;
//}

//public void WriteFloat(float number)
//{
//    if (BodyLength + FLOAT_LEN > MaxBodyLength) return;

//    byte[] bs = System.BitConverter.GetBytes(number);

//    bs.CopyTo(Bytes, HeaderLength + BodyLength);

//    BodyLength += FLOAT_LEN;
//} 
#endregion
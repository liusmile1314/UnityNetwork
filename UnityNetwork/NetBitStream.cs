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
        public const int header_length = 4;

        public const int max_body_length = 512;

        public const int BYTE_LEN = 1;

        public const int INT32_LEN = 4;

        public const int SHORT16_LEN = 2;

        public const int FLOAT_LEN = 4;

        private byte[] _bytes = null;

        public byte[] BYTES {
            get {
                return _bytes;
            }
            set {
                _bytes = value;
            }
        }

        private int _bodyLength = 0;

        public int BodyLength {
            get { return _bodyLength; }
        }

        public int Length 
        {
            get { return header_length + _bodyLength; }
        }

        public Socket _socket = null;

        public int readLength { get; set; }

        public NetBitStream()
        {
            readLength = 0;

            _bodyLength = 0;

            _bytes = new Byte[header_length + max_body_length];
        }

        public void Reset()
        {
            readLength = 0;

            _bodyLength = 0;
        }

        public void WriteByte(byte bt)
        {
            if (_bodyLength + BYTE_LEN > max_body_length) return;

            _bytes[header_length + _bodyLength] = bt;

            _bodyLength += BYTE_LEN;
        }

        public void WriteBool(bool flag)
        {
            if (_bodyLength + BYTE_LEN > max_body_length) return;

            byte bt = flag ? (byte)'1' : (byte)'0';

            _bytes[header_length + _bodyLength] = bt;

            _bodyLength += BYTE_LEN;
        }

        public void WriteInt(int number)
        {
            if (_bodyLength + INT32_LEN > max_body_length) return;

            byte[] bs = System.BitConverter.GetBytes(number);

            bs.CopyTo(_bytes, header_length + _bodyLength);

            _bodyLength += INT32_LEN;
        }

        public void WriteUInt(uint number)
        {
            if (_bodyLength + INT32_LEN > max_body_length) return;

            byte[] bs = System.BitConverter.GetBytes(number);

            bs.CopyTo(_bytes, header_length + _bodyLength);

            _bodyLength += INT32_LEN;
        }

        public void WriteShort(short number)
        {
            if (_bodyLength + SHORT16_LEN > max_body_length) return;

            byte[] bs = System.BitConverter.GetBytes(number);

            bs.CopyTo(_bytes, header_length + _bodyLength);

            _bodyLength += SHORT16_LEN;
        }

        public void WriteUShort(ushort number)
        {
            if (_bodyLength + SHORT16_LEN > max_body_length) return;

            byte[] bs = System.BitConverter.GetBytes(number);

            bs.CopyTo(_bytes, header_length + _bodyLength);

            _bodyLength += SHORT16_LEN;
        }

        public void WriteFloat(float number)
        {
            if (_bodyLength + FLOAT_LEN > max_body_length) return;

            byte[] bs = System.BitConverter.GetBytes(number);

            bs.CopyTo(_bytes, header_length + _bodyLength);

            _bodyLength += FLOAT_LEN;
        }

        public void WriteString(string str)
        {
            int len = System.Text.Encoding.UTF8.GetByteCount(str);

            this.WriteInt(len);

            if (_bodyLength + len > max_body_length) return;

            System.Text.Encoding.UTF8.GetBytes(str, 0, str.Length, _bytes, header_length + _bodyLength);

            _bodyLength += len;
        }

        public void WriteStream(byte[] bs)
        {
            this.WriteInt(bs.Length);

            if (_bodyLength + bs.Length > max_body_length) return;

            bs.CopyTo(bs, header_length + _bodyLength);

            _bodyLength += bs.Length;
        }

        public void WriteObject<T>(T obj)
        {
            byte[] bs = Serialize<T>(obj);

            this.WriteStream(bs);
        }
        public void BeginWrite(string msg)
        {
            _bodyLength = 0;

            this.WriteString(msg);
        }

        public void EncodeHeader()
        {
            byte[] bs = System.BitConverter.GetBytes(_bodyLength);

            bs.CopyTo(_bytes, 0);
        }


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

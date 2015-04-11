using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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

        public NetBitStream()
        {
            _bodyLength = 0;

            _bytes = new Byte[header_length + max_body_length];
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
    }
}

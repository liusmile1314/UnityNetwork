using System;
using System.Collections.Generic;
using System.Text;


namespace ProtoBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            ProtoBuf.Meta.RuntimeTypeModel model = ProtoBuf.Meta.RuntimeTypeModel.Create();

            model.Add(typeof(ChatAPP.Chat), true);

            model.Add(typeof(ChatAPP.User), true);

            model.Compile("ChatSerializer", "ChatSerializer.dll");
        }
    }
}

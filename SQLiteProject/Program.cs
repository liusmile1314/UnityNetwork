﻿using System;
using System.Collections.Generic;

using System.Text;

using System.Data.Common;
using System.Data.SQLite;
using System.IO;

using ProtoBuf;
using ChatAPP;


namespace SQLiteProject
{
    class Program
    {
        /*SQLite是一款非常轻量级的关系数据库系统。 SQLite的主要应用场景有作为手机应用的数据库以及小型桌面软件的数据库。
         * 
         *  Reference:http://www.cnblogs.com/teroy/p/3960760.html
         */

        //static readonly string DB_PATH = @"Data Source = G:\20150414\SQLiteSpy_1.9.8\Information.db3";

        //static readonly string DB_PATH = "Data Source = G:/20150414/SQLiteSpy_1.9.8/Information.db3";

        static readonly string DB_PATH = "Data Source = " +Environment.CurrentDirectory + "\\Information.db3";
        
        static void Insert()
        {
            using (SQLiteConnection con = new SQLiteConnection(DB_PATH))
            {
                con.Open();

                string sqlStr = @"INSERT INTO hero VALUES(4,'End')";
                
                using (SQLiteCommand cmd = new SQLiteCommand(sqlStr,con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        static void Select()
        {
            using (SQLiteConnection con = new SQLiteConnection(DB_PATH))
            {
                con.Open();

                string sqlStr = @"SELECT * FROM hero";

                using (SQLiteCommand cmd = new SQLiteCommand(sqlStr, con))
                {
                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Console.WriteLine("\t{0}\t{1}",dr["hero_id"].ToString(), dr["hero_name"]);
                        }
                    }

                    Console.WriteLine("----------------------------------");
                }
            }
        }

        static void Update()
        {
            using (SQLiteConnection con = new SQLiteConnection(DB_PATH))
            {
                con.Open();

                string sqlStr = @"UPDATE hero SET Hero_name = 'HappyEnd' WHERE hero_id = 4";

                using(SQLiteCommand cmd = new SQLiteCommand(sqlStr,con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        static void Delete()
        {
            using (SQLiteConnection con = new SQLiteConnection(DB_PATH))
            {
                con.Open();

                string sqlStr = "DELETE FROM hero WHERE hero_id = 4";

                using (SQLiteCommand cmd = new SQLiteCommand(sqlStr, con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            
        }
        static void Main(string[] args)
        {

            ChatAPP.User user = new ChatAPP.User();

            user.name = "Liusmile";

            user.email = "Forever Love";


            using (MemoryStream msUser = new MemoryStream())
            {
                ProtoBuf.Meta.RuntimeTypeModel model = ChatSerializer.Create();

                //ProtoBuf.Serializer.Serialize<ChatAPP.User>(msUser, user);

                model.Serialize(msUser, user);
                
                Byte[] bs = msUser.ToArray();

                File.WriteAllBytes("F:/ChatAPP.txt", bs);
            }

            using (MemoryStream msUser = new MemoryStream(File.ReadAllBytes("F:/ChatAPP.txt")))
            {
                ProtoBuf.Meta.RuntimeTypeModel model = ChatSerializer.Create();

                //ChatAPP.User userRead = ProtoBuf.Serializer.Deserialize<ChatAPP.User>(msUser);

                ChatAPP.User userRead = (ChatAPP.User)model.Deserialize(msUser, null, typeof(ChatAPP.User));

                Console.WriteLine(userRead.name + " " + userRead.email);
            }

            Console.ReadLine();
            
            /*
            ChatAPP.Chat chat = new ChatAPP.Chat();

            ChatAPP.User user = new ChatAPP.User();

            user.name = "Liusmile";

            user.email = "Forever Love";

            ChatAPP.User user1 = new ChatAPP.User();

            user1.name = "Turing";

            user1.email = "Computer Sciences";

            chat.user.Add(user);

            chat.user.Add(user1);

            //Byte[] bs = null;

            using (MemoryStream msUser = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize<ChatAPP.User>(msUser, user);

                Byte[] bs = msUser.ToArray();

                File.WriteAllBytes("F:/ChatAPP.txt", bs);
            }

            using (MemoryStream msChat = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize<ChatAPP.Chat>(msChat, chat);

                File.WriteAllBytes("F:/All.txt", msChat.ToArray());
            }

            using (MemoryStream msRead = new MemoryStream(File.ReadAllBytes("F:/ChatAPP.txt")))
            {
                ChatAPP.User userRead = ProtoBuf.Serializer.Deserialize<ChatAPP.User>(msRead);

                Console.WriteLine( userRead.name + " " + userRead.email);
            }

            Console.WriteLine("_________________________");

            using (MemoryStream mschatRead = new MemoryStream(File.ReadAllBytes("F:/All.txt")))
            {
                ChatAPP.Chat chatRead = ProtoBuf.Serializer.Deserialize<ChatAPP.Chat>(mschatRead);

                for (int i = 0; i < chatRead.user.Count; i++)
                {
                    Console.WriteLine(chatRead.user[i].name + " " + chatRead.user[i].email);
                }
            }
            */
          
            //控制台根目录
            Console.WriteLine(Environment.CurrentDirectory);

            Console.WriteLine(Directory.GetCurrentDirectory());

            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);

            Console.WriteLine(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase);

            //Console.WriteLine(Application.StartupPath);

            //Console.WriteLine(Application.ExecutablePath);

            //MessageBox.Show("Title", "Hello", MessageBoxButtons.OK, MessageBoxIcon.None);

            //Directory.GetParent()

            Delete();
                Select();

            Insert();
                Select();

            Update();
                Select();

            Console.ReadLine();
        }
    }
}

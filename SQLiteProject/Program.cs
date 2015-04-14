using System;
using System.Collections.Generic;

using System.Text;

using System.Data.Common;
using System.Data.SQLite;

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
            Console.WriteLine("Path : " + Environment.CurrentDirectory);

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

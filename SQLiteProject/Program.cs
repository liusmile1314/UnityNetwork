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
         */

        //static readonly string DB_PATH = @"Data Source = G:\20150414\SQLiteSpy_1.9.8\Information.db3";

        //static readonly string DB_PATH = "Data Source = G:/20150414/SQLiteSpy_1.9.8/Information.db3";

        static readonly string DB_PATH = "Data Source = " +Environment.CurrentDirectory + "\\Information.db3";
        
        static void Insert()
        {
            using (SQLiteConnection con = new SQLiteConnection(DB_PATH))
            {
                con.Open();

                string sqlStr = @"INSERT INTO hero VALUES(3,'Love')";
                
                using (SQLiteCommand cmd = new SQLiteCommand(sqlStr,con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Path : " + Environment.CurrentDirectory);

            Insert();

            Console.ReadLine();
        }
    }
}

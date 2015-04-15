using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exercise
{
    class Program
    {
        static void Main(string[] args)
        {

            int number = 10;

            Console.WriteLine(number >> 1);

            Console.WriteLine(number << 1);

            Console.WriteLine(number >> 2);

            Console.WriteLine(number << 2);

            Console.WriteLine(number >> 3);

            Console.WriteLine(number << 3);

            Console.WriteLine(number >> 4);

            Console.WriteLine(number << 4);

            Console.ReadLine();

            /*******************************************/

            Thread[] threads = new Thread[10];

            Account acObj = new Account(1000);

            for (int i = 0; i < 10; i++)
            {
                Thread tempThread = new Thread(acObj.ExecuteWithdraw);

                threads[i] = tempThread;
            }


            for (int i = 0; i < 10; i++)
            {
                threads[i].Start();
            }
            Console.ReadLine();

            int type = 2;

            int length = 131330;//65793;//257;//1075;//563;

            Console.WriteLine(Convert.ToByte(type));

            Console.WriteLine(Convert.ToByte(length >> 16 & 0xFF));

            Console.WriteLine(Convert.ToByte(length >> 8 & 0xFF));

            Console.WriteLine(Convert.ToByte(length & 0xFF));

            Console.ReadLine();
            //buf[index++] = Convert.ToByte(body.Length >> 16 & 0xFF);
            //buf[index++] = Convert.ToByte(body.Length >> 8 & 0xFF);
            //buf[index++] = Convert.ToByte(body.Length & 0xFF);
        }
    }

    class Account
    {
        private Object thisLock = new Object();

        int balance;

        Random random = new Random();

        public Account(int initial)
        {
            balance = initial;
        }

        int withdraw(int amount)
        {
            if (balance < 0)
            {
                //Console.WriteLine("Exception : " + balance);
            }

            lock (thisLock)
            {
                if (amount < balance)
                {
                    Console.WriteLine("BalanceBefore : " + balance);

                    Console.WriteLine("amount : " + amount);

                    balance = balance - amount;

                    Console.WriteLine("BalanceAfter : " + balance);

                    return amount;
                }
                else
                {
                    //Console.WriteLine("End : 0");
                    return 0;
                }
            }
        }

        public void ExecuteWithdraw()
        {
            for (int i = 0; i < 40; i++)
            {
                withdraw(random.Next(1, 100));
            }
        }
    }
}

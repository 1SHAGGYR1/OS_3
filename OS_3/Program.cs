using System;
using System.IO.Pipes;
using System.Threading;
using System.IO;
namespace OS_3
{
    class Program
    {
        static NamedPipeClientStream Client = new NamedPipeClientStream("Pipe");
        static NamedPipeServerStream Server = new NamedPipeServerStream("Pipe");
        static bool thread1 = true, thread2 = true;
        static void One()
        {
            Server.WaitForConnection();
            Random rand = new Random();
            try
            {
                using (StreamWriter writer = new StreamWriter(Server))
                {
                    while (thread1)
                    {
                        writer.WriteLine("Your byte is {0}", rand.Next(byte.MaxValue));
                        Thread.Sleep(100);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Client " + e.Message); 
            }
            Server.Close();
        }
        static void Two()
        {
            Client.Connect();
            try
            {
                using (StreamReader reader = new StreamReader(Client))
                {
                    while (thread2)
                    {
                        Console.WriteLine(reader.ReadLine());
                        Thread.Sleep(150);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Server" + e.Message);
            }
            Client.Close();
        }
        static void Main(string[] args)
        {
            Thread firstthread = new Thread(One);
            firstthread.Start();
            Thread secondthread = new Thread(Two);
            secondthread.Start();
            Console.ReadKey();
            thread1 = thread2 = false;
            Console.WriteLine("Threads stopped.");
            Console.ReadKey();
        }
    }
}

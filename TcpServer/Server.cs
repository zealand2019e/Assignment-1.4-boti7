using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BookLibrary;

namespace TcpServer
{
    class Server
    {
        private static List<Book> books = new List<Book>();

        public void Start()
        {
            TcpListener server = null;
            try
            {
                Int32 port = 4646;
                IPAddress localAddr = IPAddress.Any;

                server = new TcpListener(localAddr, port);

                server.Start();

                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    TcpClient client = server.AcceptTcpClient();

                    Console.WriteLine("connected");

                    Task.Run(() => HandleStream(client));
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server.Stop();
            }
            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }

        private void HandleStream(TcpClient client)
        {
            Byte[] bytes = new Byte[256];
            String data = null;

            NetworkStream stream = client.GetStream();

            int i;

            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                string[] lines = data.Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.None);

                Console.WriteLine("Command: {0} Param: {1}", lines[0], lines[1]);

                string response = ProcessData(lines[0], lines[1]);

                byte[] msg = System.Text.Encoding.ASCII.GetBytes(response);

                stream.Write(msg, 0, msg.Length);
            }

            client.Close();
        }

        private string ProcessData(string command, string param)
        {
            string returnData = "";

            switch(command)
            {
                case "GetAll":
                    returnData = JsonConvert.SerializeObject(books);
                    break;
                case "Get":
                    returnData = JsonConvert.SerializeObject(books.Find(e => e.Isbn13 == param));
                    break;
                case "Save":
                    books.Add(JsonConvert.DeserializeObject<Book>(param));
                    break;
            }

            return returnData;
        }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PingPongClientTcp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddr = ipHost.AddressList[0];

                TcpClient client = new TcpClient();
                try
                {
                    client.Connect(ipAddr, 8888);

                    bool endConnection = false;
                    while (!endConnection)
                    {
                        string personJson = GetUserData();

                        if (personJson == null)
                        {
                            endConnection = true;
                        }
                        else
                        {
                            SendAndReciveMessageFromServer(client, personJson);
                        }

                    }
                }
                catch (ArgumentNullException ane)
                {

                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }

                catch (SocketException se)
                {

                    Console.WriteLine("SocketException : {0}", se.ToString());
                }

                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
            }

            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("End");

        }
        public static string GetUserData()
        {
            Console.WriteLine("enter name and age , if you wand to end connection enter: 0");
            string name = Console.ReadLine();
            int age;

            while (!int.TryParse(Console.ReadLine(), out age))
            {
                Console.WriteLine("enter age- age is number!");
            }

            if (name == "0" || age == 0)
            {
                return null;
            }
            else
            {
                Person person = new Person(name, age);

                return JsonConvert.SerializeObject(person);
            }
        }
        public static void SendAndReciveMessageFromServer(TcpClient client, string personJson)
        {
            NetworkStream nwStream = client.GetStream();
            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(personJson);

            nwStream.Write(bytesToSend, 0, bytesToSend.Length);

            byte[] bytesToRead = new byte[client.ReceiveBufferSize];
            int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
            string personRead = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);

            Person newPerson = JsonConvert.DeserializeObject<Person>(personRead);
            Console.WriteLine("Received : " + newPerson.ToString());
        }


    } 
}

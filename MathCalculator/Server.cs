using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MathCalculator
{
    class Server
    {
        private double _numberX;
        private double _numberY;
        private double _serverResult;
        private string _selectedMethod;
        public void Start()
        {
            TcpListener server = null;
            try
            {
                Int32 port = 3001;
                IPAddress localAddress = IPAddress.Loopback;
                server = new TcpListener(localAddress, port);
                server.Start();
                Console.WriteLine("Server started");

                while (true)
                {
                    TcpClient connectionSocket = server.AcceptTcpClient();
                    Task.Run(() =>
                    {
                        TcpClient tempSocket = connectionSocket;
                        DoClient(tempSocket); 

                    });
                }
                
                server.Stop();
            }
            catch (SocketException ex)
            {
                Console.WriteLine("SocketException: {0}", ex);
            }
        }

        private void DoClient(TcpClient connectionSocket)
        {

            Console.WriteLine("Server activated");
            Stream ns = connectionSocket.GetStream();

            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);
            sw.AutoFlush = true;

            // Client communication
            sw.WriteLine("Type: Add/Sub/Mul/Div (NumberX) (NumberY), to Add/Sub/Mul/Div two numbers");
            string[] array = sr.ReadLine().Split(' ');
            _selectedMethod = (array[0]);
            _numberX = Double.Parse(array[1]);
            _numberY = Double.Parse(array[2]);
            AddNumbers();
            switch (_selectedMethod)
            {
                case "Add":
                    Console.WriteLine("Selected method: Add");
                    AddNumbers();
                    break;
                case "Sub":
                    Console.WriteLine("Selected method: Subtraction");
                    SubNumbers();
                    break;
                case "Mul":
                    Console.WriteLine("Selected method: Multiply");
                    MulNumbers();
                    break;
                case "Div":
                    Console.WriteLine("Selected method: Divide");
                    DivNumbers();
                    break;

            }
            sw.WriteLine($"Result of added numbers is: {_serverResult}");

            ns.Close();
            connectionSocket.Close();


        }

        private void AddNumbers()
        {
            _serverResult = _numberX + _numberY;
        }

        private void SubNumbers()
        {
            _serverResult = _numberX - _numberY;
        }

        private void MulNumbers()
        {
            _serverResult = _numberX * _numberY;
        }

        private void DivNumbers()
        {
            _serverResult = _numberX / _numberY;
        }
    }
}

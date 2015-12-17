using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tanks_Client.DataType;

namespace TankGame
{
    class ClientClass
    {

        //will store the received msgs
        private String msg = null;

        //this object is created to add objects to the MsgParser Class Queue
        MsgParser msgParser;

        private TcpClient client;
        private Thread thread;
        private TcpListener tcpListener;

        Int32 sendPort = 6000;
        Int32 receivePort = 7000;

        IPAddress localAddr = IPAddress.Parse("127.0.0.1");

        public ClientClass(MsgParser parser)
        {
            this.msgParser = parser;
            //constructor will initiate thread which will execute the receive method
            thread = new Thread(new ThreadStart(Receiver));
        }

        //will be used to send msgs to the server
        public void Sender(String msg)
        {
            client = new TcpClient();
            client.Connect(localAddr, sendPort);
            Stream stream = client.GetStream();

            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] temp = ascii.GetBytes(msg);

            stream.Write(temp, 0, temp.Length);
            stream.Close();
            client.Close();
            //thread will initiate only if the JOIN# msgs is passed
            if (msg.Equals("JOIN#") && !thread.IsAlive)
            {
                thread.Start();
            }

        }

        //is used to receive msgs sent by the server
        public void Receiver()
        {
            this.tcpListener = new TcpListener(IPAddress.Any, receivePort);

            //this loop will keep on listening
            while (true)
            {
                // Always use a Sleep call in a while(true) loop 
                // to avoid locking up your CPU. --Shanika
                //Thread.Sleep(10);
                tcpListener.Start();
                TcpClient client = this.tcpListener.AcceptTcpClient();
                Stream streamReceiver = client.GetStream();
                Byte[] bytes = new Byte[256];

                int i;
                String data = null;

                while ((i = streamReceiver.Read(bytes, 0, bytes.Length)) != 0)
                {

                    //receive msg sent by server
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                }
                //write to console- for testing purposes
                Console.WriteLine(data);
                msg = data;
                //creates a MsgObject object and stores the respectivr values
                MsgObject msgObject = new MsgObject(msg, DateTime.Now);
                msgParser.addMsg(msgObject);

                streamReceiver.Close();
                tcpListener.Stop();
                client.Close();

            }



        }

        //this is used to retrieve the msg only
        public String getMessage()
        {
            return msg;
        }




    }
}

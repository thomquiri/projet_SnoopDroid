using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationRobot
{
    internal class ControlPacketSender
    {
        private TcpClient? client;
        private NetworkStream? stream;

        public ControlPacketSender()
        {

        }

        public void Connect(string ipAddress, int port, string mot_de_passe)
        {
            try
            {
                client = new TcpClient(ipAddress, port);
                stream = client.GetStream();
                byte[] passwordBytes = Encoding.ASCII.GetBytes(mot_de_passe);
                stream.Write(passwordBytes, 0, passwordBytes.Length);
            }
            catch 
            {
                MessageBox.Show("La connexion n'a pas pu être établie");
            }
        }
        public long GetCurrentUnixTimestamp()
        {
            DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = DateTime.UtcNow - epochStart;
            // Directly use ticks for nanoseconds since each tick represents 100 nanoseconds
            long unixTimeInNanoseconds = timeSpan.Ticks * 100; // Incorrect multiplication here, the correct line should not multiply by 100.
            return unixTimeInNanoseconds;
        }

        public void SendJoystickUpdate(float x, float y)
        {
            if (!IsConnected()) 
            { 
                this.Close();
                return;
            }
                byte[] packet = new byte[16];
                Buffer.BlockCopy(BitConverter.GetBytes(x), 0, packet, 0, 4);
                Buffer.BlockCopy(BitConverter.GetBytes(y), 0, packet, 4, 4);
                Buffer.BlockCopy(BitConverter.GetBytes(this.GetCurrentUnixTimestamp()), 0, packet, 8, 8);
                stream.Write(packet, 0, packet.Length);
        }

        public void SendButtonUpdate(byte keyId, bool pressed)
        {
            byte[] packet = new byte[10];
            packet[0] = keyId;
            packet[1] = (byte)(pressed ? 1 : 0);
            Buffer.BlockCopy(BitConverter.GetBytes(this.GetCurrentUnixTimestamp()), 0, packet, 2, 8);
            stream.Write(packet, 0, packet.Length);
        }

        public void Close()
        {
            if (stream != null) { stream.Close(); }
            if(client != null) { client.Close(); }
        }
        public bool IsConnected()
        {
            try
            {
                if (client != null && client.Client != null && client.Client.Connected)
                {
                    // Detect if socket is disconnected
                    if (client.Client.Poll(0, SelectMode.SelectRead))
                    {
                        byte[] checkBuff = new byte[1];
                        if (client.Client.Receive(checkBuff, SocketFlags.Peek) == 0)
                        {
                            // Connection is closed
                            return false;
                        }
                    }

                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }
    }
}

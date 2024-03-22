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
        private Dictionary<int, long> pingSentTimestamps = new Dictionary<int, long>();

        public ControlPacketSender()
        {

        }

        public void Connect(string ipAddress, int port, string mot_de_passe, Label labelPing)
        {
            try
            {
                client = new TcpClient(ipAddress, port);
                stream = client.GetStream();
                byte[] passwordBytes = Encoding.ASCII.GetBytes(mot_de_passe);
                stream.Write(passwordBytes, 0, passwordBytes.Length);
                pingLoop(1000);
                ListenForPingCallback(labelPing);
                Task.Run(async () => await pingLoop(1000)); // Exécute pingLoop dans une tâche séparée
                Task.Run(async () => await ListenForPingCallback(labelPing)); // Idem pour ListenForPingCallback
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

        public async Task pingLoop(int delayMs)
        {
            int id = 0;
            while (true) 
            { 
                SendPing(id);
                MessageBox.Show("feur");
                id++;
                await Task.Delay(delayMs);
            }
        }

        public void SendPing(int identifier)
        {
            if (!IsConnected())
            {
                this.Close();
                return;
            }
            byte[] packet = new byte[5];
            Buffer.BlockCopy(Encoding.ASCII.GetBytes("P"), 0, packet, 0, 1);
            Buffer.BlockCopy(BitConverter.GetBytes(identifier), 0, packet, 1, 4);
            // Convert the packet to a hex string for display
            string packetHexString = BitConverter.ToString(packet).Replace("-", " ");
            pingSentTimestamps[identifier] = GetCurrentUnixTimestamp(); // Store the send timestamp
            stream.Write(packet, 0, packet.Length);
        }

        public async Task ListenForPingCallback(Label labelPing)
        {
            try
            {
                if (stream == null || !IsConnected()) return;

                byte[] buffer = new byte[5]; // Buffer for the current message
                int bytesReadTotal = 0; // Total bytes read for the current message

                while (true)
                {
                    int bytesRead = await stream.ReadAsync(buffer, bytesReadTotal, buffer.Length - bytesReadTotal);
                    bytesReadTotal += bytesRead;

                    if (bytesReadTotal == buffer.Length) // Check if we've got the complete message
                    {
                        if (Encoding.ASCII.GetString(buffer, 0, 1) == "P")
                        {
                            int identifier = BitConverter.ToInt32(buffer, 1);
                            long receivedTimestamp = GetCurrentUnixTimestamp();

                            if (pingSentTimestamps.TryGetValue(identifier, out long sentTimestamp))
                            {
                                long pingTime = receivedTimestamp - sentTimestamp; // Calculate ping time
                                long pingTimeMs = pingTime / 1000000;
                                // Make sure to update UI elements on the UI thread
                                labelPing.Invoke((MethodInvoker)(() => labelPing.Text = $"Ping: {pingTimeMs} ms"));
                                pingSentTimestamps.Remove(identifier); // Clean up the entry
                            }
                        }
                        bytesReadTotal = 0; // Reset for the next message
                    }
                }
            }
            catch  (Exception ex)
            {
                MessageBox.Show($"La connexion est terminé.");
                this.Close();
            }
        }

        public void SendJoystickUpdate(float x, float y)
        {
            if (!IsConnected()) 
            { 
                this.Close();
                return;
            }
            byte[] packet = new byte[17];
            Buffer.BlockCopy(Encoding.ASCII.GetBytes("J"), 0, packet, 0, 1);
            Buffer.BlockCopy(BitConverter.GetBytes(x), 0, packet, 1, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(y), 0, packet, 5, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(this.GetCurrentUnixTimestamp()), 0, packet, 9, 8);
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

using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PRI_lab5_Client
{
    public partial class Form1 : Form
    {
        TcpClient tcpClient;
        NetworkStream stream;
        public Form1()
        {
            InitializeComponent();
            tcpClient = new TcpClient();
            //Start();
            tcpClient.Connect("192.168.43.11", 8888);
            stream = tcpClient.GetStream();
            StartListening();
        }
        void Start()
        {
            tcpClient.ConnectAsync("192.168.43.11", 8888);
            stream = tcpClient.GetStream();
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            string message = textBox2.Text;
            //отправляем длину сообщения
            await stream.WriteAsync(BitConverter.GetBytes(message.Length));
            //отправляемм сообщение
            await stream.WriteAsync(Encoding.UTF8.GetBytes(message));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //stream.Write(Encoding.UTF8.GetBytes("!@#END#@!" + '\0'));
            //Thread.Sleep(5000);
            // Закрыть NetworkStream
            //stream.Close();
            // Освободить ресурсы NetworkStream
            //stream.Dispose();

            // Закрыть TcpClient
            //tcpClient.Close();
            // Освободить ресурсы TcpClient
            //tcpClient.Dispose();
        }
        //private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    Close();
        //}

        private async void StartListening()
        {
            while (true)
            {
                await ListenMessages();
            }
        }
        private async Task ListenMessages()
        {
            byte[] messageLengthBuffer = new byte[4];
            await stream.ReadAsync(messageLengthBuffer);
            int messageLength = BitConverter.ToInt32(messageLengthBuffer, 0);

            //we have received messageLength; Now we are going to receive message
            byte[] buffer = new byte[messageLength];
            await stream.ReadAsync(buffer, 0, messageLength);
            var message = Encoding.UTF8.GetString(buffer, 0, messageLength);
            textBox1.Text += message + "\r\n";
        }
    }
}
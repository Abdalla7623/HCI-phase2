using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WinFormsApp1
{ 
    public partial class Form1 : Form
    {

        List<img> imgList = new List<img>();
        List<img2> img2 = new List<img2>();
       
        Bitmap off = new Bitmap(2000, 1000);
        int x = 50;
        int y = 50;
        float w = 30;
        float h = 30;

        class ClientC
        {
            int byteCount;
            NetworkStream stream;
            byte[] sendData;
            TcpClient tcpClient;

            public bool connectToSocket(String host, int portNumber)
            {
                try
                {
                    tcpClient = new TcpClient(host, portNumber);
                    stream = tcpClient.GetStream();
                    Console.WriteLine("Connection Made ! with " + host);
                    return true;
                }
                catch (System.Net.Sockets.SocketException e)
                {
                    Console.WriteLine("Connection Failed: " + e);
                    return false;
                }
            }

            public bool recieveMessage()
            {
                try
                {
                    // Receive some data from the peer.
                    byte[] receiveBuffer = new byte[1024];
                    int bytesReceived = stream.Read(receiveBuffer);
                    string data = Encoding.UTF8.GetString(receiveBuffer.AsSpan(0, bytesReceived));

                    if (data == "exit")
                    {
                        return false;
                    }

                    string[] points = data.Split(",");

                    for (int i = 0; i < points.Length; i++)
                    {
                        Console.WriteLine(points[i]);
                    }

                    // Check if the received data is "mado" and open a new form if it is
                    if (data == "'center'")
                    {
                        // Open a new form or perform the desired action when "mado" is received
                        OpenNewForm();
                    }
                    else
                    {
                        Console.WriteLine("failed");
                    }
                    

                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Connection not initialized : " + e);
                    return false;
                }
            }

            private void OpenNewForm()
            {
                Form1.ActiveForm.Close();  
                Form2 form2 = new Form2();
                form2.Show();

            }



            public bool closeConnection()
            {

                stream.Close();
                tcpClient.Close();
                Console.WriteLine("Connection terminated ");
                return true;

            }

            
        }

        private BackgroundWorker backgroundWorker;

        public Form1()
        {
            InitializeComponent();
            MouseDown += Form1_MouseDown;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Form1_MouseDown(object? sender, MouseEventArgs e)
        {
            // ... (unchanged)
        }

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            ClientC c = new ClientC();
            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            DrawDubb(e.Graphics, c);
        }

        void DrawScene(Graphics g, ClientC c)
        {
            g.Clear(Color.White);
            for (int i = 0; i < imgList.Count; i++)
            {
                img img = imgList[i];
                g.DrawImage(img.bmp, img.y, img.x, GraphicsUnit.Pixel);
            }

            for (int i = 0; i < img2.Count; i++)
            {
                img2 img_report = img2[i];
                g.DrawImage(img_report.bmp, img_report.x, img_report.y, img_report.w, img_report.h);
            }
            //Font font = new Font("system", 20);
            //Brush b2 = new SolidBrush(Color.White);
            //g.DrawString("face login", font, b2, this.Width / 10, 30);
        }

        void DrawDubb(Graphics g, ClientC c)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2, c);
            g.DrawImage(off, 0, 0);
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            create();
            createreport();

            this.Paint += Form1_Paint;
            this.WindowState = FormWindowState.Normal;
            ClientC c = new ClientC();
            DrawDubb(this.CreateGraphics(), c);
            c.connectToSocket("127.0.0.1", 6000);
            while (true)
            {
                bool resp = c.recieveMessage();
                if (!resp)
                {
                    break;
                }
                
            }

            c.closeConnection();
        }

        void create()
        {
            img img = new img();
            img.bmp = new Bitmap("museum.jpg");
            img.y = new Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height);
            img.x = new Rectangle(0, 0, img.bmp.Width, img.bmp.Height);
            imgList.Add(img);
        }

        void createreport()
        {
            img2 img = new img2();
            img.bmp = new Bitmap("login.png");
            img.x = imgList[0].x.X + 450;
            img.y = imgList[0].y.Y + 250;
            img.w = img.bmp.Width;
            img.h = img.bmp.Height;
            img2.Add(img);
        }
    }

    class img
    {
        public Rectangle y;
        public Rectangle x;
        public Bitmap bmp;
    }

    class img2
    {
        public int x, y, w, h;
        public Bitmap bmp;
    }

}

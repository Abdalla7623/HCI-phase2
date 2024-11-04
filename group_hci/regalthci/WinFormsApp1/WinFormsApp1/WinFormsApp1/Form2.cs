using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form2 : Form
    {
        List<img3> imgList = new List<img3>();
        List<img4> img2 = new List<img4>();

        Bitmap off = new Bitmap(2000, 1000);
        private TcpClient tcpClient;
        private NetworkStream stream;
       
        class ClientC
        {
            private Form currentForm;
            int byteCount;
            NetworkStream stream;
            byte[] sendData;
            TcpClient tcpClient;
            public ClientC(Form form)
            {
                currentForm = form;  // Initialize the form instance
            }
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

                    if (data != "Unknown")
                    {
                        OpenNewForm();
                    }
                    else
                    {
                        Console.WriteLine("login failed");
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
                currentForm.Close();
                //tuio exe
                Form3 form3 = new Form3();
                form3.Show();
            }

            //private void openadmin()
            //{
            //    currentForm.Close();
            //    //tuio exe
            //    Form4 form4 = new Form4();
            //    form4.Show();
            //}
            //private void openadmin1()
            //{
            //    currentForm.Close();
            //    //tuio exe
            //    Form7 form7 = new Form7();
            //    form7.Show();
            //}

            //private void OpenNurseForm()
            //{
            //    currentForm.Close();
            //    //tuio exe
            //    Form7 form7 = new Form7();
            //    form7.Show();
            //}


            public bool closeConnection()
            {

                stream.Close();
                tcpClient.Close();
                Console.WriteLine("Connection terminated ");
                return true;

            }

           
        }
        public Form2()
        {
            InitializeComponent();
            // Initialize the clientC instance
            this.StartPosition = FormStartPosition.CenterScreen;

        }
        private void Form2_Load(object sender, EventArgs e)
        {
            create();
            createreport();
            this.Paint += Form2_Paint;

            ClientC c = new ClientC(this);  // Pass the form instance to the constructor
            DrawDubb(this.CreateGraphics(), c);
            c.connectToSocket("127.0.0.1", 5000);

            while (true)
            {
                bool mido = c.recieveMessage();
                if (!mido)
                {
                    break;
                }
            }

            // Close the form after the loop completes
            this.Close();

            // Close the connection after closing the form
            c.closeConnection();
        }

        private void Form2_Paint(object? sender, PaintEventArgs e)
        {

            ClientC c = new ClientC(this);
            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            DrawDubb(e.Graphics, c);
        }

        void DrawDubb(Graphics g, ClientC c)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2, c);
            g.DrawImage(off, 0, 0);
        }
        void DrawScene(Graphics g, ClientC c)
        {
            g.Clear(Color.White);
            for (int i = 0; i < imgList.Count; i++)
            {
                img3 img = imgList[i];
                g.DrawImage(img.bmp, img.y, img.x, GraphicsUnit.Pixel);
            }

            for (int i = 0; i < img2.Count; i++)
            {
                img4 img_report = img2[i];
                g.DrawImage(img_report.bmp, img_report.x, img_report.y, img_report.w, img_report.h);
            }
            Font font = new Font("system", 20);
            Brush b2 = new SolidBrush(Color.White);
            g.DrawString("face login", font, b2, this.Width / 10, 30);
        }

        void create()
        {
            img3 img = new img3();
            img.bmp = new Bitmap("loginn-transformed.jpeg");
            img.y = new Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height);
            img.x = new Rectangle(0, 0, img.bmp.Width, img.bmp.Height);
            imgList.Add(img);
        }

        void createreport()
        {
            img4 img = new img4();
            img.bmp = new Bitmap("login.png");
            img.x = imgList[0].x.X + 250;
            img.y = imgList[0].y.Y + 250;
            img.w = img.bmp.Width;
            img.h = img.bmp.Height;
            img2.Add(img);
        }
    }
    class img3
    {
        public Rectangle y;
        public Rectangle x;
        public Bitmap bmp;
    }

    class img4
    {
        public int x, y, w, h;
        public Bitmap bmp;
    }
}

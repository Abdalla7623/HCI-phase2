using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form4 : Form
    {

        List<img20> imgList = new List<img20>();
        List<img21> img2 = new List<img21>();
        List<img22> img22 = new List<img22>();
        List<img23> img23 = new List<img23>();
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
            bool applicationOpened = false;


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
                    if (data == "'prev'")
                    {
                        // Open a new form or perform the desired action when "'prev'" is received
                        OpenNewForm();
                    }
                    //else if (data == "'next'")
                    //{
                    //    opentnextform();

                    //}
                    else if (data == "'center'")
                    {
                        // Do something for "center"
                        openthisform();
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
                Form4.ActiveForm.Close();
                Form1 form1 = new Form1();
                form1.Show();

            }
            private void opentnextform()
            {
                Form4.ActiveForm.Close();
                Form5 form5 = new Form5();
                form5.Show();

            }
            private void openthisform()
            {

                string pathToFile1 = "D:\\semester1\\HCI\\project\\editd\\test_bol7i\\WinFormsApp1\\WinFormsApp1\\WinFormsApp1\\bin\\Debug\\net6.0-windows\\cleo.txt";

                try
                {
                    // Check if Notepad is already running
                    Process[] processes = Process.GetProcessesByName("notepad");
                    if (processes.Length == 0)
                    {
                        // Notepad is not running, so start a new instance
                        Process.Start("notepad.exe", pathToFile1);
                    }
                    else
                    {
                        // Notepad is already running
                        MessageBox.Show("Notepad is already open.");
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions
                    MessageBox.Show("Error: " + ex.Message);
                }
            }


            private bool IsProcessRunning(string processName)
            {
                Process[] processes = Process.GetProcessesByName(processName);
                return processes.Length > 0;
            }
            public bool closeConnection()
            {

                stream.Close();
                tcpClient.Close();
                Console.WriteLine("Connection terminated ");
                return true;

            }


        }
        public Form4()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

        }

        private void Form4_Load(object sender, EventArgs e)
        {
            create();
            createreport();
            //createnext();
            createlog();
            this.Paint += Form4_Paint;
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

        private void Form4_Paint(object? sender, PaintEventArgs e)
        {
            ClientC c = new ClientC();
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
                img20 img = imgList[i];
                g.DrawImage(img.bmp, img.y, img.x, GraphicsUnit.Pixel);
            }

            for (int i = 0; i < img2.Count; i++)
            {
                img21 img_report = img2[i];
                g.DrawImage(img_report.bmp, img_report.x, img_report.y, img_report.w, img_report.h);
                string text = "cleopatra Information";
                Font font = new Font("Arial", 10, FontStyle.Bold);
                Brush brush = Brushes.White;
                g.DrawString(text, font, brush, img_report.x, img_report.y - 30);

            }
            for (int i = 0; i < img22.Count; i++)
            {
                img22 img_log = img22[i];
                g.DrawImage(img_log.bmp, img_log.x, img_log.y, img_log.w, img_log.h);
            }
            for (int i = 0; i < img23.Count; i++)
            {
                img23 img_log = img23[i];
                g.DrawImage(img_log.bmp, img_log.x, img_log.y, img_log.w, img_log.h);
            }
        }

        void create()
        {
            img20 img = new img20();
            img.bmp = new Bitmap("cleo.jpg");
            img.y = new Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height);
            img.x = new Rectangle(0, 0, img.bmp.Width, img.bmp.Height);
            imgList.Add(img);
        }
        //void createnext()
        //{
        //    img23 img = new img23();
        //    img.bmp = new Bitmap("next.jpg");
        //    img.x = 580;
        //    img.y = 20;
        //    img.w = img.bmp.Width;
        //    img.h = img.bmp.Height;
        //    img23.Add(img);
        //}
        void createlog()
        {
            img22 img = new img22();
            img.bmp = new Bitmap("close.png");
            img.x = 30;
            img.y = 20;
            img.w = 186;
            img.h = 51;
            img22.Add(img);
        }
        void createreport()
        {
            img21 img = new img21();
            img.bmp = new Bitmap("enter.png");
            img.x = 200;
            img.y = 200;
            img.w = img.bmp.Width;
            img.h = img.bmp.Height;
            img2.Add(img);
        }
    }
    class img20
    {
        public Rectangle y;
        public Rectangle x;
        public Bitmap bmp;
    }

    class img21
    {
        public int x, y, w, h;
        public Bitmap bmp;
    }
    class img22
    {
        public int x, y, w, h;
        public Bitmap bmp;

    }
    class img23
    {
        public int x, y, w, h;
        public Bitmap bmp;

    }
}

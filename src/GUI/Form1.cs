using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ActionHook.ConsoleApp;



namespace GUI
{
    public partial class Form1 : Form
    {
        private Semaphore semaphore;
        private Thread thread;
        private string tmp_str;

        public Form1()
        {
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            semaphore = new Semaphore(1, 1); // 一开始有1个资源，最多有1个资源
            semaphore.WaitOne();             // 我自己先拿走一个
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = tb_url.Text;

            //try
            //{
            //    test_if_our(url)
            //    connect();
            //}
            //catch (Exception)
            //{
            //    MessageBox message = new MessageBox("invalid url for peeper server!\n");
            //    message.show()
            //    throw;
            //}

            ConsoleApp2 app = new ConsoleApp2(this.tb_keyboard, this.tb_mouse);
            this.thread = new Thread(() => this.tmp_str = app.run(this.semaphore, url));
            Console.WriteLine("start thread!");
            this.thread.Start();
            
            //btn_start.disable();
            //btn_stop.enable();

        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            this.semaphore.Release();
            Console.WriteLine("in btn_stop_Click, release semaphore");
            this.thread.Join();
            Console.WriteLine("in btn_stop_Click, Join!");
            //btn_start.enable();
            //btn_stop.disable();
            Console.WriteLine(tmp_str);
        }

        public void set_tb_keyboard(string s)
        {
            this.tb_keyboard.Text = s;
        }

        public void set_tb_mouse(string s)
        {
            this.tb_mouse.Text = s;
        }
    }

}

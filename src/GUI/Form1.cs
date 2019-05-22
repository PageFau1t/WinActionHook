using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace GUI
{
    public partial class Form1 : Form
    {
        private Semaphore semaphore;
        private Thread thread;
        private Task task_Reporter;
        private CancellationTokenSource reporterCancellation = new CancellationTokenSource();

        public Form1()
        {
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
            this.thread = new Thread(() => app.run(this.semaphore));
            this.thread.Start();

            task_Reporter = Task.Run(() => { ScheduledReporter.Run(reporterCancellation.Token); }, reporterCancellation.Token);



            //btn_start.disable();

        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            
            try
            {
                reporterCancellation.Cancel();
            }catch (OperationCanceledException)
            {

            }
            
            this.semaphore.Release();
            Console.WriteLine("in btn_stop_Click, release semaphore");
            this.thread.Join();
            Console.WriteLine("in btn_stop_Click, Join!");
            //btn_start.enable();
        }
    }
}

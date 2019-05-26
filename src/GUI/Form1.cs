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



namespace GUI
{
    public partial class Form1 : Form
    {
        private Semaphore semaphore;
        private Thread thread;
        private string tmp_str;
        private Task task_Reporter;
        private CancellationTokenSource reporterCancellation;
        private string logpath = "../../../log.txt";
        
        public Form1()
        {
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            semaphore = new Semaphore(1, 1); // 一开始有1个资源，最多有1个资源
            semaphore.WaitOne();             // 我自己先拿走一个 
            init_form();

            btn_start.Enabled = true;
            btn_stop.Enabled = false;
            button1.Enabled = false;
            btn_read.Enabled = true;

        }

        private void init_form()
        {
            //表格每列宽度
            int w = 30;
            this.listView1.Columns.Add("1", w, HorizontalAlignment.Left);
            this.listView1.Columns.Add("2",w, HorizontalAlignment.Left);
            this.listView1.Columns.Add("3", w, HorizontalAlignment.Left);
            this.listView1.Columns.Add("4", w, HorizontalAlignment.Left);
            this.listView1.Columns.Add("5", w, HorizontalAlignment.Left);
            this.listView1.Columns.Add("6", w, HorizontalAlignment.Left);
            this.listView1.Columns.Add("7", w, HorizontalAlignment.Left);
            this.listView1.Columns.Add("8", w, HorizontalAlignment.Left);
            this.listView1.Columns.Add("9", w, HorizontalAlignment.Left);
            this.listView1.Columns.Add("10", w, HorizontalAlignment.Left);
            this.listView1.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度

            //添加第一行
            ListViewItem lvi = new ListViewItem();
            lvi.Text = "1";
            lvi.SubItems.Add("2");
            lvi.SubItems.Add("3");
            lvi.SubItems.Add("4");
            lvi.SubItems.Add("5");
            lvi.SubItems.Add("6");
            lvi.SubItems.Add("7");
            lvi.SubItems.Add("8");
            lvi.SubItems.Add("9");
            lvi.SubItems.Add("0");
            this.listView1.Items.Add(lvi);
            //添加第2行
            lvi = new ListViewItem();
            lvi.Text = "q";
            lvi.SubItems.Add("w");
            lvi.SubItems.Add("e");
            lvi.SubItems.Add("r");
            lvi.SubItems.Add("t");
            lvi.SubItems.Add("y");
            lvi.SubItems.Add("u");
            lvi.SubItems.Add("i");
            lvi.SubItems.Add("o");
            lvi.SubItems.Add("p");
            this.listView1.Items.Add(lvi);
            //添加第3行
            lvi= new ListViewItem();
            lvi.Text = "a";
            lvi.SubItems.Add("s");
            lvi.SubItems.Add("d");
            lvi.SubItems.Add("f");
            lvi.SubItems.Add("g");
            lvi.SubItems.Add("h");
            lvi.SubItems.Add("j");
            lvi.SubItems.Add("k");
            lvi.SubItems.Add("l");
            lvi.SubItems.Add(" ");
            this.listView1.Items.Add(lvi);
            //添加第4行
            lvi = new ListViewItem();
            lvi.Text = " ";
            lvi.SubItems.Add("z");
            lvi.SubItems.Add("x");
            lvi.SubItems.Add("c");
            lvi.SubItems.Add("v");
            lvi.SubItems.Add("b");
            lvi.SubItems.Add("n");
            lvi.SubItems.Add("m");
            lvi.SubItems.Add(" ");
            lvi.SubItems.Add(" ");
            this.listView1.Items.Add(lvi);


            //更新表格
            this.listView1.View = System.Windows.Forms.View.Details;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = tb_url.Text;

            ConsoleApp2 app = new ConsoleApp2(this.tb_keyboard, this.tb_mouse,this.listView1);
            this.thread = new Thread(() => this.tmp_str = app.run(this.semaphore, url));
            Console.WriteLine("start thread!");
            this.thread.Start();

            reporterCancellation = new CancellationTokenSource();
            task_Reporter = Task.Run(() => { ScheduledReporter.Run(reporterCancellation.Token, url, app); }, reporterCancellation.Token);

            btn_start.Enabled = false;
            btn_stop.Enabled = true;

        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            tmp_str = "";
            reporterCancellation.Cancel();
            task_Reporter.Dispose();
            this.semaphore.Release();
            Console.WriteLine("in btn_stop_Click, release semaphore");
            this.thread.Join();
            Console.WriteLine("in btn_stop_Click, Join!");
            btn_start.Enabled = true;
            btn_stop.Enabled = false;
            button1.Enabled = true;
        }



        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btn_read_Click(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader(logpath, Encoding.Default);
            string content;
            while ((content = sr.ReadLine()) != null)
            {
                this.textBox1.AppendText(content.ToString()+"\n");
            }
            sr.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            // "保存为"对话框
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "文本文件|*.txt";
            // 显示对话框
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // 文件名
                string fileName = dialog.FileName;
                // 创建文件，准备写入
                FileStream fs = File.Open(fileName,
                        FileMode.Create,
                        FileAccess.Write);
                StreamWriter wr = new StreamWriter(fs);
                wr.Write(this.tmp_str);
                // 关闭文件
                wr.Flush();
                wr.Close();
                fs.Close();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

}

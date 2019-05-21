using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ActionHook;
using System.Threading;
using System.Windows.Forms;
using System.Management;

namespace ActionHook.ConsoleApp
{
    public class ConsoleApp
    {

        static void Main(string[] args)
        {
            var eventHookFactory = new EventHookFactory();
            //Queue<string> events = new Queue<string>();

            // 用工厂方法获得watcher对象
            var keyboardWatcher = eventHookFactory.GetKeyboardWatcher();
            // 启动钩子
            keyboardWatcher.Start();
            // 添加按键信息的回调方法
            keyboardWatcher.OnKeyInput += (s, e) =>
            {
                Console.WriteLine("Key {0} event of key {1}", e.KeyData.EventType, e.KeyData.Keyname);
                //string out1 = "";
                //events += string.Format("Key {0} event of key {1}", e.KeyData.EventType, e.KeyData.Keyname);

                //Console.WriteLine(ss);
            };

            var mouseWatcher = eventHookFactory.GetMouseWatcher();
            //mouseWatcher.Start();
            mouseWatcher.OnMouseInput += (s, e) =>
            {
                Console.WriteLine("Mouse event {0} at point {1},{2}", e.Message.ToString(), e.Point.x, e.Point.y);
            };

            // 示例发送HTTP报文
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("0", "11");
            dic.Add("a", "4");
            dic.Add("b", "2019");

            Task.Factory.StartNew(async () =>
            {
                for (; ; )
                {
                    await Task.Delay(1000);
                    string responseBody = Post("http://localhost:8086/announce", dic);
                    Console.WriteLine(responseBody);
                }

            });

            // 主线程阻塞在这里 输入一行退出
            Console.Read();

            keyboardWatcher.Stop();
            mouseWatcher.Stop();
            eventHookFactory.Dispose();
        }

        #region HTTP-post
        // 示例HTTP-post请求
        // ref: https://www.imooc.com/article/40178
        public static string Post(string url, Dictionary<string, string> dic)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            #region 添加Post 参数  
            StringBuilder builder = new StringBuilder();
            int i = 0;
            foreach (var item in dic)
            {
                if (i > 0)
                    builder.Append("&");
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
                i++;
            }
            byte[] data = Encoding.UTF8.GetBytes(builder.ToString());
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //读出响应内容  
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            // Console.WriteLine(result); // print
            return result;
        }
        #endregion

        #region Get_CPUID
        // 获取CPUID，用以标志鶸
        // ref: https://blog.csdn.net/iilegend/article/details/75087638
        public static string Get_CPUID()
        {
            try
            {
                //需要在解决方案中引用System.Management.DLL文件  
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                string strCpuID = null;
                foreach (ManagementObject mo in moc)
                {
                    strCpuID = mo.Properties["ProcessorId"].Value.ToString();
                    mo.Dispose();
                    break;
                }
                return strCpuID;
            }
            catch
            {
                return "";
            }
        }
        #endregion
    }


    public class ConsoleApp2
    {
        private EventHookFactory eventHookFactory;
        private MouseWatcher mouseWatcher;
        private KeyboardWatcher keyboardWatcher;

        public ConsoleApp2(TextBox tb_keyboard, TextBox tb_mouse)
        {
            Console.WriteLine("New ConsoleApp2!");
            this.eventHookFactory = new EventHookFactory();

            this.keyboardWatcher = eventHookFactory.GetKeyboardWatcher();
            this.keyboardWatcher.OnKeyInput += (s, e) =>
            {
                string msg = $"Key {e.KeyData.EventType} event of key {e.KeyData.Keyname}";
                tb_keyboard.Text = msg;
                Console.WriteLine(msg);
            };

            this.mouseWatcher = eventHookFactory.GetMouseWatcher();
            this.mouseWatcher.OnMouseInput += (s, e) =>
            {
                string msg = $"Mouse event {e.Message.ToString()} at point {e.Point.x},{e.Point.y}";
                tb_mouse.Text = msg;
                Console.WriteLine(msg);
            };
        }

        public void run(Semaphore semaphore)
        {
            Console.WriteLine("in ConsoleApp2.run()");
            keyboardWatcher.Start();
            mouseWatcher.Start();


            Task.Factory.StartNew(async () =>
            {
                for (; ; )
                {
                    await Task.Delay(1000);
                    //string responseBody = Post("http://localhost:8086/announce", dic);
                    //Console.WriteLine(responseBody);
                }
            });

            // 主线程阻塞在这里
            semaphore.WaitOne();

            keyboardWatcher.Stop();
            mouseWatcher.Stop();
            eventHookFactory.Dispose();
        }

        // 示例HTTP-post请求
        // ref: https://www.imooc.com/article/40178
        public static string Post(string url, Dictionary<string, string> dic)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            #region 添加Post 参数  
            StringBuilder builder = new StringBuilder();
            int i = 0;
            foreach (var item in dic)
            {
                if (i > 0)
                    builder.Append("&");
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
                i++;
            }
            byte[] data = Encoding.UTF8.GetBytes(builder.ToString());
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //读出响应内容  
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            // Console.WriteLine(result); // print
            return result;
        }
    }
}

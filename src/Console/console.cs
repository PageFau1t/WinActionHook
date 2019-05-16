using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ActionHook;


namespace ActionHook.ConsoleApp
{
    class ConsoleApp
    {
        static void Main(string[] args)
        {
            var eventHookFactory = new EventHookFactory();

            // 用工厂方法获得watcher对象
            var keyboardWatcher = eventHookFactory.GetKeyboardWatcher();
            // 启动钩子
            keyboardWatcher.Start();
            // 添加按键信息的回调方法
            keyboardWatcher.OnKeyInput += (s, e) =>
            {
                Console.WriteLine("Key {0} event of key {1}", e.KeyData.EventType, e.KeyData.Keyname);
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

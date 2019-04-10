using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            mouseWatcher.Start();
            mouseWatcher.OnMouseInput += (s, e) =>
            {
                Console.WriteLine("Mouse event {0} at point {1},{2}", e.Message.ToString(), e.Point.x, e.Point.y);
            };

            Console.Read();

            keyboardWatcher.Stop();
            mouseWatcher.Stop();
            eventHookFactory.Dispose();
        }
    }
}

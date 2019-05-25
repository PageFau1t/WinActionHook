using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ActionHook.Helpers;
using ActionHook.Hooks;

namespace ActionHook
{
    /// <summary>
    ///     Key press data.
    ///     封装成EventArgs，与Invoke搭配使用
    /// </summary>
    public class KeyInputEventArgs 
    {
        public KeyData KeyData { get; set; }
    }

    /// <summary>
    ///     Key data.
    ///     按键记录
    /// </summary>
    public class KeyData
    {
        public KeyEvent EventType;
        public Key Key; // 添加：原始扫描码
        public string Keyname;
        public string UnicodeCharacter;
    }

    /// <summary>
    ///     Key press event type.
    ///     按下 or 释放
    /// </summary>
    public enum KeyEvent
    {
        down = 0,
        up = 1
    }

    /// <summary>
    ///     Wraps low level keyboard hook.
    ///     Uses a producer-consumer pattern to improve performance and to avoid operating system forcing unhook on delayed
    ///     user callbacks.
    ///     键盘钩子的封装，使用生产者-消费者模式以提高性能
    /// </summary>
    public class KeyboardWatcher
    {
        //互斥锁
        private readonly object accesslock = new object();
        //得到UI线程的Task Scheduler
        private readonly SyncFactory factory;

        private KeyboardHook keyboardHook;
        //异步队列
        private AsyncConcurrentQueue<object> keyQueue;
        private CancellationTokenSource taskCancellationTokenSource;

        internal KeyboardWatcher(SyncFactory factory)
        {
            this.factory = factory;
        }

        private bool isRunning { get; set; }
        //记录上层用户响应函数
        public event EventHandler<KeyInputEventArgs> OnKeyInput;

        /// <summary>
        ///     Start watching
        /// </summary>
        public void Start()
        {
            // 和Stop() 互斥
            lock (accesslock)
            {
                if (!isRunning)
                {
                    // 创建异步队列
                    taskCancellationTokenSource = new CancellationTokenSource();
                    keyQueue = new AsyncConcurrentQueue<object>(taskCancellationTokenSource.Token);

                    //This needs to run on UI thread context
                    //So use task factory with the shared UI message pump thread
                    // 在UI线程中启动Hook，生产者作为Hook的回调函数。
                    Task.Factory.StartNew(() =>
                        {
                            keyboardHook = new KeyboardHook();
                            keyboardHook.KeyDown += KListener;
                            keyboardHook.KeyUp += KListener;
                            keyboardHook.Start();
                        },
                        CancellationToken.None,
                        TaskCreationOptions.None,
                        factory.GetTaskScheduler()).Wait();

                    // 启动消费者
                    Task.Factory.StartNew(() => ConsumeKeyAsync());
                    
                    // 记录watcher状态
                    isRunning = true;
                }
            }
        }

        /// <summary>
        ///     Stop watching
        /// </summary>
        public void Stop()
        {
            lock (accesslock)
            {
                if (isRunning)
                {
                    if (keyboardHook != null)
                    {
                        //This needs to run on UI thread context
                        //So use task factory with the shared UI message pump thread
                        Task.Factory.StartNew(() =>
                            {
                                keyboardHook.KeyDown -= KListener;
                                keyboardHook.Stop();
                                keyboardHook = null;
                            },
                            CancellationToken.None,
                            TaskCreationOptions.None,
                            factory.GetTaskScheduler());
                    }

                    keyQueue.Enqueue(false);
                    isRunning = false;
                    taskCancellationTokenSource.Cancel();
                }
            }
        }

        /// <summary>
        ///     Add key event to the producer queue
        ///     Hook的回调，向异步队列写入记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KListener(object sender, RawKeyEventArgs e)
        {
            keyQueue.Enqueue(new KeyData
            {
                UnicodeCharacter = e.Character,
                Key = e.Key,
                Keyname = e.Key.ToString(),
                EventType = (KeyEvent)e.EventType
            });
        }

        /// <summary>
        ///     Consume events from the producer queue asynchronously
        ///     消费者：从异步队列取出记录，Invoke调用上层用户的响应函数
        ///     注意，这是个异步函数(async)
        /// </summary>
        /// <returns></returns>
        private async Task ConsumeKeyAsync()
        {
            while (isRunning)
            {
                //blocking here until a key is added to the queue
                var item = await keyQueue.DequeueAsync();
                if (item is bool)
                {
                    break;
                }

                //KListener_KeyDown(item);
                OnKeyInput?.Invoke(null, new KeyInputEventArgs { KeyData = (KeyData)item });
            }
        }

        ///// <summary>
        /////     Invoke user call backs
        ///// </summary>
        ///// <param name="kd"></param>
        //private void KListener_KeyDown(KeyData kd)
        //{
            
        //}
    }
}

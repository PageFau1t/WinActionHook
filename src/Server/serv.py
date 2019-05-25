import os
import tornado.ioloop
import tornado.web
import tornado.escape
import tornado.httpserver


class AnnounceHandler(tornado.web.RequestHandler):
    # post: /announce
    async def post(self):
        print(self.get_argument("cpuid"))
        self.write('ACK')

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
    app = make_app()
    server = tornado.httpserver.HTTPServer(app)
    server.bind(8086)
    server.start(1)  # 0: forks one process per cpu
    tornado.ioloop.IOLoop.current().start()

using System;
using SuperWebSocket;

namespace WebSocketBasicDemo.Server
{
    class Program
    {
        private const string wsIP = "127.0.0.1";
        private const int wsPort = 2012;

        static void Main(string[] args)
        {
            var ws = new WebSocketServer();
            if (!ws.Setup(wsIP, wsPort)) //根据IP和端口启动服务器
            {
                Console.WriteLine("设置WebSocket服务端IP Port失败");
                return;
            }

			//绑定连接事件
            ws.NewSessionConnected += ws_NewSessionConnected;
			//绑定关闭事件
            ws.SessionClosed += ws_SessionClosed;
			//绑定接受事件
            ws.NewMessageReceived += ws_NewMessageReceived;

			//启动服务器
            if (!ws.Start())
            {
                Console.WriteLine("开启WebSocket服务侦听失败");
                return;
            }

            Console.WriteLine("侦听服务以启动 按'q'键关闭服务");


            while ('q' == Console.ReadKey().KeyChar)
            {
                Console.WriteLine("关闭服务侦听");
                ws.Stop();
            }
        }

		/// <summary>
		/// 检测有一个会话session
		/// </summary>
		/// <param name="session"></param>
        static void ws_NewSessionConnected(WebSocketSession session)
        {
            Console.WriteLine("{0:HH:MM:ss} 客户端:{1} 连接服务器 ", DateTime.Now, session.RemoteEndPoint);
        }

		/// <summary>
		/// 检测有一个会话关闭
		/// </summary>
		/// <param name="session">session</param>
		/// <param name="value">关闭的原因</param>
        static void ws_SessionClosed(WebSocketSession session, SuperSocket.SocketBase.CloseReason value)
        {
            Console.WriteLine("{0:HH:MM:ss} 客户端:{1} 与服务器断开连接  原因:{2} ", DateTime.Now, session.RemoteEndPoint, value);
        }

		/// <summary>
		///	客户端接受
		/// </summary>
		/// <param name="session"></param>
		/// <param name="value"></param>
        static void ws_NewMessageReceived(WebSocketSession session, string value)
        {
            Console.WriteLine(string.Format("{0:HH:MM:ss} 接收到来自客户端{1}的消息:{2}", DateTime.Now, session.RemoteEndPoint, value));

            session.Send(string.Format("服务端已经收到消息:{0}", value));
        }
    }
}

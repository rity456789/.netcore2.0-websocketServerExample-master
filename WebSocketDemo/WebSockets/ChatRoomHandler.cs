using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WebSocketDemo.Model;

namespace WebSocketDemo.WebSockets
{
    public class ChatRoomHandler : WebSocketHandler
    {
        public ChatRoomHandler(WebSocketConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {

        }
        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            //if (message.Substring(0, 4) == "1a2b") 
            //{
            //    var sId = WebSocketConnectionManager.GetId(socket);
            //    var username = message.Substring(5);
            //    WebSocketConnectionManager.SetUserIdentification(sId, username);
            //}
            //else
            //{
            //    var data = JObject.Parse(message);
            //    var receiver = data["ReceiverId"].ToString();
            //    var receiverWSID = WebSocketConnectionManager.GetUserIdentification(receiver);
            //    await SendMessageAsync(receiverWSID ,message);
            //}
            await SendMessageToAllAsync(message);
        }
    }
}

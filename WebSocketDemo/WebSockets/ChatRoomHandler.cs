using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocketDemo.Model;

namespace WebSocketDemo.WebSockets
{
    public class ChatRoomHandler : WebSocketHandler
    {
        static HttpClient client = new HttpClient(); //read db from api
        public ChatRoomHandler(WebSocketConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {

        }
        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            if (message.Substring(0, 4) == "1a2b")
            {
                var sId = WebSocketConnectionManager.GetId(socket);
                var uId = Int32.Parse(message.Substring(5));
                WebSocketConnectionManager.SetUserIdentification(sId, uId);
            }
            else
            {
                JObject data = JObject.Parse(message);
                Invoice invoice = new Invoice(data);
                var urlString = "https://localhost:44373/api/invoice";

                var receiver = invoice.ReceiverId;
                var receiverWSID = WebSocketConnectionManager.GetUserIdentification(receiver);
                if (receiverWSID == "") //receiver doesn't connect to socket
                {   //store invoice to db without change status
                    var uri = new Uri(urlString);
                    var jsonInString = JsonConvert.SerializeObject(invoice);
                    var response = await client.PostAsync(uri, new StringContent(jsonInString, Encoding.UTF8, "application/json"));
                }
                else
                {
                    invoice.Status = 1; //sent
                    var uri = new Uri(urlString);
                    var jsonInString = JsonConvert.SerializeObject(invoice);
                    var response = await client.PostAsync(uri, new StringContent(jsonInString, Encoding.UTF8, "application/json"));
                    await SendMessageAsync(receiverWSID, message);
                }               
            }
        }
    }
}

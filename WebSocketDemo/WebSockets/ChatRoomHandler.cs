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
        private static readonly HttpClient Client = new HttpClient(); //read db from api
        private const string UrlString = "https://localhost:44373/api/invoice";

        public ChatRoomHandler(WebSocketConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {

        }
        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            if (message.Substring(0, 4) == "1a2b")
            {
                Identify(socket, message);
            }
            else
            {
                JObject data = JObject.Parse(message);
                Invoice invoice = new Invoice(data);
                if (!invoice.ReplyInvoice)
                {   //reply task
                    await StoreAndSendInvoice(invoice);
                }
                else
                {   //reply other invoice
                    //update status (3-reply) of invoice replied by this invoice
                    await UpdateInvoiceStatus(invoice);
                }
            }
        }

        private async Task UpdateInvoiceStatus(Invoice invoice)
        {
            invoice.Status = 3; //approved
            var uri = new Uri(UrlString + "/" + invoice.ReplyId.ToString());
            var jsonInString = JsonConvert.SerializeObject(invoice);
            var response = await Client.PutAsync(uri, new StringContent(jsonInString, Encoding.UTF8, "application/json"));
        }

        private async Task StoreAndSendInvoice(Invoice invoice)
        {
            var receiver = invoice.ReceiverId;
            var receiverWsid = WebSocketConnectionManager.GetUserIdentification(receiver);
            var message = "";
            if (receiverWsid == "") //receiver doesn't connect to socket
            {   //store invoice to db without change status
                var uri = new Uri(UrlString);
                var jsonInString = JsonConvert.SerializeObject(invoice);
                var response = await Client.PostAsync(uri, new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            }
            else
            {
                invoice.Status = 1; //sent
                var uri = new Uri(UrlString);
                var jsonInString = JsonConvert.SerializeObject(invoice);
                var response = await Client.PostAsync(uri, new StringContent(jsonInString, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    var rs = await response.Content.ReadAsAsync<Invoice>();
                    jsonInString = JsonConvert.SerializeObject(rs);
                    message = jsonInString;
                }
                await SendMessageAsync(receiverWsid, message);
            }
        }

        private void Identify(WebSocket socket, string message)
        {
            var sId = WebSocketConnectionManager.GetId(socket);
            var uId = Int32.Parse(message.Substring(5));
            WebSocketConnectionManager.SetUserIdentification(sId, uId);
        }
    }
}

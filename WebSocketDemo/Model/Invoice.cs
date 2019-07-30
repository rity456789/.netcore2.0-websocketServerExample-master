using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketDemo.Model
{
    public class Invoice
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public int Method { get; set; }
        public int Type { get; set; }
        public string Time { get; set; }
        public int Status { get; set; }
        public bool ReplyInvoice { get; set; }
        public int ReplyId { get; set; }

        public Invoice(JObject data)
        {
            Id = 0;
            Message = data["Message"].ToString();
            SenderId = Int32.Parse(data["SenderId"].ToString());
            ReceiverId = Int32.Parse(data["ReceiverId"].ToString());
            Method = Int32.Parse(data["Method"].ToString());
            Type = Int32.Parse(data["Type"].ToString());
            Time = data["Time"].ToString();
            Status = Int32.Parse(data["Status"].ToString());
            ReplyInvoice = bool.Parse(data["ReplyInvoice"].ToString());
            ReplyId = Int32.Parse(data["ReplyId"].ToString());
        }

        public Invoice()
        {

        }
    }
}

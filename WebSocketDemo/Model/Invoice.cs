using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketDemo.Model
{
    public enum Status
    {
        Sending = 0,
        Sent = 1,
        Approved = 2,
        Replied = 3,
        Error = -1
    }

    public enum Method
    {
        Email = 0,
        SMS = 1,
        Notification = 2,
        PushNotification = 3,
    }

    public enum InvoiceType
    {
        Seen = 0,
        Approved = 1,
        Replied = 2,
    }

    public class Invoice
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public Method Method { get; set; }
        public InvoiceType Type { get; set; }
        public string Time { get; set; }
        public Status Status { get; set; }
        public bool ReplyInvoice { get; set; }
        public int ReplyId { get; set; }

        public Invoice(JObject data)
        {
            Id = 0;
            Message = data["Message"].ToString();
            SenderId = Int32.Parse(data["SenderId"].ToString());
            ReceiverId = Int32.Parse(data["ReceiverId"].ToString());
            var temp = 1;
            Method = (Method)Int32.Parse(data["Method"].ToString());
            Type = (InvoiceType)Int32.Parse(data["Type"].ToString());
            Time = data["Time"].ToString();
            Status = (Status)Int32.Parse(data["Status"].ToString());
            ReplyInvoice = bool.Parse(data["ReplyInvoice"].ToString());
            ReplyId = Int32.Parse(data["ReplyId"].ToString());
        }

        public Invoice()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketDemo.Model
{
    public class JSON_data
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Type { get; set; }
        public string Time { get; set; }
        public int Status { get; set; }
    }
}

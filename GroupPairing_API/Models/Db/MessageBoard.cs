using System;
using System.Collections.Generic;

#nullable disable

namespace GroupPairing_API.Models.Db
{
    public partial class MessageBoard
    {
        public int MessageId { get; set; }
        public int ActivityId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public DateTime MessageDateTime { get; set; }
    }
}

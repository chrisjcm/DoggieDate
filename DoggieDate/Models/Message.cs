using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DoggieDate.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string SenderId { get; set; }
        public ApplicationUser Sender { get; set; }
        public string ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; }

        public string Content { get; set; }

        [DataType(DataType.DateTime)]
        [DefaultValue("getutcdate()")]
        public DateTime TimeStamp { get; set; }

        [DefaultValue("false")]
        public bool IsRead { get; set; }

        [DefaultValue("false")]
        public bool Reported { get; set; }
    }
}

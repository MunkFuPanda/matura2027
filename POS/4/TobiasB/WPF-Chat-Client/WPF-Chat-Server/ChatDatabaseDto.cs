using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Chat_Server
{

    public partial class MessageDto
    {
        public long? ID { get; set; } // INTEGER
        public string Titel { get; set; } // TEXT
        public string Content { get; set; } // TEXT
        public long Sender { get; set; } // INTEGER
        public long? Receiver { get; set; } // INTEGER
        public long? Room { get; set; } // INTEGER
        public long Timestamp { get; set; } // INTEGER

    }


    public partial class RoomDto
    {
        public long? ID { get; set; } // INTEGER
        public string Name { get; set; } // TEXT


    }

    public partial class RoomUserDto
    {
        public long? Room { get; set; } // INTEGER
        public long? User { get; set; } // INTEGER


    }


    public partial class UserDto
    {
        public long? ID { get; set; } // INTEGER
        public string Name { get; set; } // TEXT
        public string Password { get; set; } // TEXT
        public long? Timestamp { get; set; } // INTEGER

    }
}

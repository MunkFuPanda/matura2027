using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Chat_Client
{
    public class Message
    {
        public int Type { get; set; } // 0 = Authenticate, 1 = ChatMessage, 2 = Rooms
        public string Message_Text { get; set; }

        public int From_UserId { get; set; }

        public string From_UserName { get; set; } = string.Empty;

        public int To_RommId { get; set; }

        public DateTime date { get; set; }


        public Message(int type, string messageText, int from_UserId, int to_RommId, DateTime date)
        {
            Type = type;
            Message_Text = messageText;
            From_UserId = from_UserId;
            To_RommId = to_RommId;
            this.date = date;
        }

        public Message(int type, string messageText)
        {
            Type = type;
            Message_Text = messageText;
        }

        public Message() { }
    }

    public enum MessageType
    {
        Authenticate = 0,
        ChatMessage = 1,
        Rooms = 2
    }
}

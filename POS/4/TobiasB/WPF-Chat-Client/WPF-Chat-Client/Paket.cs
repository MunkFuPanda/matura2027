using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Chat_Client
{
    public class Paket
    {
        public MessageType type;

        public String username;
        public String password;

        public List<MessageDto> messages;
        public List<UserDto> users;
        public long? sender;
        public long? receiver;

        public Paket() { }

        public Paket(MessageType type)
        {
            this.type = type;
        }
        public Paket(MessageType type, String username, String password)
        {
            this.type = type;
            this.username = username;
            this.password = password;
        }

        public Paket(MessageType type, List<MessageDto> messages)
        {
            this.type = type;
            this.messages = messages;
        }

        public Paket(MessageType type, List<UserDto> users)
        {
            this.type = type;
            this.users = users;
        }

        public Paket(MessageType type, long? sender, long? receiver)
        {
            this.type = type;
            this.sender = sender;
            this.receiver = receiver;
        }
    }
}

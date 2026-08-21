using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3tePAUebung

{

    public enum Msgenum{
        Login,
        GetUserList,
        Send,
        Receiv
    }
    public class Msg
    {
        public Msgenum msgenum { get; set; }
        public string name { get; set; }
        public List<User> users;
        public List<Message> messages;
        public Message message;

        public Msg(Msgenum msgenum, string name, List<User> users, List<Message> messages, Message message)
        {
            this.msgenum = msgenum;
            this.name = name;
            this.users = users;
            this.messages = messages;
            this.message = message;

        }
    }
}

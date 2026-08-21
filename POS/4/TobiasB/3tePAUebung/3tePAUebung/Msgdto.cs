using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3tePAUebung
{
        public class Message
        {
           public long? ID { get; set; } // INTEGER
            public long From { get; set; } // INTEGER
            public long To { get; set; } // INTEGER
            public string MessageColumn { get; set; } // TEXT


        }

        public partial class User
        {
public string Name { get; set; } // TEXT
public long? ID { get; set; } // INTEGER

 


        }
    }


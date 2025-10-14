using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Achterbahn
{
    internal class train
    {
        private int id;
        private string status;
        public train(int id)
        {
            this.id = id;
            this.status = "waiting";
        }

        private int getid(int id)
        {
            return id;
        }
        private string getstatus(string status)
        {
            return status;
        }
    }
}

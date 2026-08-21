using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Video_Player
{
    internal class Video
    {
        public String Name { get { return name; } set { this.name = value; } }
        public Uri Path { get { return path; } set { this.path = value; } }

        private String name;
        private Uri path;

        public Video(String name, Uri path)
        {
            this.name = name;
            this.path = path;
        }

        public override String ToString()
        {
            return name;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Video_Player
{
    public class VideoList
    {
        public String videoTitle { get; set; }
        public String videoFilePath { get; set; }

        public VideoList(String title, String filePath)
        {
            this.videoTitle = title;
            this.videoFilePath = filePath;
        }
        public override string ToString()
        {
            return videoTitle;
        }
    }
}

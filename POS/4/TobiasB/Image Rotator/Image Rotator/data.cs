using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Image_Rotator
{
    internal class data
    {
        public ProgressBar progressBar;
        public int rotation;
        public Mirroring mirroring;
        public int quality;
        public string work_dir;
        public string save_dir;
        public object item;

        public data(ProgressBar progressBar, int rotation, Mirroring mirroring, int quality, string work_dir, string save_dir, object item)
        {

            this.progressBar = progressBar;
            this.rotation = rotation;
            this.mirroring = mirroring;
            this.quality = quality;
            this.work_dir = work_dir;
            this.save_dir = save_dir;
            this.item = item;
        }


    }
}

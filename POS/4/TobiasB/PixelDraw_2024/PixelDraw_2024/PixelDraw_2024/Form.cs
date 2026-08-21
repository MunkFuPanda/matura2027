using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace PixelDraw_2024
{
    public class Form
    {

        [XmlAttribute("Typ")]
        public int type = 0;
        [XmlArray("Points")]
        [XmlArrayItem("Point")]
        public List<Point> Points = new List<Point>();
        [XmlAttribute("Radius")]
        public int radius = 0;
        [XmlAttribute("Count")]
        public int count = 0;

        public Form()
        {

        }

        public Form(int type, List<Point> Points, int radius, int count)
        {
            this.type = type;
            this.Points = Points;
            this.radius = radius;
            this.count = count;
        }

    }
}

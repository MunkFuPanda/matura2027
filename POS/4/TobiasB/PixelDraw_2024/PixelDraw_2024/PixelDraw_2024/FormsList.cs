using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PixelDraw_2024
{
    [XmlRoot("FormsList")]
    public class FormsList
    {
        public FormsList() 
        {

        }

        [XmlArray("forms")]
        [XmlArrayItem("form")]
        public List<Form> forms = new List<Form>();


    }
}

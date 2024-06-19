using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AtsEx.Plugins.Extensions.Data
{
    [XmlRoot]
    public class Extension
    {
        [XmlAttribute]
        public string Path = null;

        [XmlAttribute]
        public string Class = null;

        [XmlAttribute]
        public bool IsEnabled = true;
    }
}

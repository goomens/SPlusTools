using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UvA.SPlusTools.Data.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace UvA.SPlusTools.Data
{
    [Serializable]
    [XmlInclude(typeof(CopyResourceInfoTask))]
    [XmlInclude(typeof(EditTagsTask))]
    public abstract class SPlusTask
    {
        protected TextWriter Log = Console.Out;

        public abstract void Execute();
    }

    public abstract class ImageTask : SPlusTask
    {
        public string ProgID { get; set;  }
    }

    public abstract class TwoImageTask : SPlusTask
    {
        public string SourceProgID { get; set; }
        public string DestinationProgID { get; set; }
    }
}

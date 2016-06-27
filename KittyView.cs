using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMate
{
    public class KittyViewGroup
    {
        public KittyView[] Kitties { get; set; }
    }
    public class KittyView
    {
        public int ID { get; set; }
        public string IMGURL { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Description {get; set;}
        public string Category { get; set; }
        public string FileType { get; set; }
        public string LocalIMGURL { get; set; }
    }
}

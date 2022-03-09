using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemPipes.Framework.Data
{
    public class ItemData
    {
        public string Name { get; set; }
        public string IDName { get; set; }
        public int ID { get; set; }
        public string Description { get; set; }
        public Dictionary<string, string> NameLocalization { get; set; }
        public Dictionary<string, string> DescriptionLocalization { get; set; }
    }
}

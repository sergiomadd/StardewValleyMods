using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemLogistics.Framework.Model
{
    class DataModel
    {
        public List<string> ModItems { get; set; }
        public List<string> PipeNames { get; set; }
        public List<string> IOPipeNames { get; set; }
        public List<string> Locations { get; set; }
        public List<string> Items { get; set; }
        public List<string> ExtraNames { get; set; }
        public List<string> Buildings { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemLogistics.Framework.Model
{
    class DataModel
    {
        public List<string> ValidNetworkItems { get; set; }
        public List<string> ValidPipeNames { get; set; }
        public List<string> ValidIOPipeNames { get; set; }
        public List<string> ValidLocations { get; set; }
        public List<string> ValidItems { get; set; }
        public List<string> ValidExtraNames { get; set; }
    }
}

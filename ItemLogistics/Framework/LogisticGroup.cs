using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemLogistics.Framework.Model;

namespace ItemLogistics.Framework
{
    class LogisticGroup : SGraph
    {
        public int ID { get; set; }
        public List<OutPipe> Outputs { get; set; }
        public List<InPipe> Inputs { get; set; }

        public LogisticGroup()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemLogistics.Framework.Model
{
    class SGEdge : SGUnit
    {
        public SGNode From { get; set; }
        public SGNode To { get; set; }
        public List<SGEdgeUnit> UnitList { get; set; }
        public int Length { get; set; }

        public SGEdge(SGNode from, SGNode to)
        {
            From = from;
            To = to;
            UnitList = new List<SGEdgeUnit>();
            Length = UnitList.Count;
        }
    }
}

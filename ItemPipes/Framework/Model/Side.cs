using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemPipes.Framework.Model
{
    public class Side
    {
        public string Name { get; set; }

        public Side() { }
        public Side(string name)
        {
            Name = name;
        }
    }
}

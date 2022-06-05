using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemPipes.Framework.Data
{
    public class ItemIDs
    {
        public Dictionary<string, int> ModItemsIDs { get; set; }
        public List<int> ModItems { get; set; }
        public List<int> NetworkItems { get; set; }
        public List<string> Buildings { get; set; }

    }
}

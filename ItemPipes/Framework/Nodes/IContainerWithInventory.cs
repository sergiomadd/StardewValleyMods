using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemPipes.Framework.Nodes
{
    public interface IContainerWithInventory
    {
        public bool CanSentItems();

        public bool CanRecieveItems();

        public bool IsFull();

        public bool IsEmpty();
    }
}

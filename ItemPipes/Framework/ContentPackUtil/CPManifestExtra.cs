using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;

namespace ItemPipes.Framework.ContentPackUtil
{
    public class CPManifestExtra : IManifestDependency
    {
        public string UniqueID { get; set; }
        public ISemanticVersion MinimumVersion { get; set; }

        public bool IsRequired => throw new NotImplementedException();
    }
}

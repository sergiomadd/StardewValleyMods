using System;
using System.Linq;

namespace ItemPipes.Framework.Util
{
    public static class Utilities
    {
        public static string GetIDName(string name)
        {
            string trimmed = "";
            if (name.Equals("P.P.M."))
            {
                trimmed = "PPM";
            }
            else
            {
                trimmed = String.Concat(name.Where(c => !Char.IsWhiteSpace(c)));
            }
            return trimmed;
        }
    }
}

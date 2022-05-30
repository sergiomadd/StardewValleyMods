﻿using System;
using System.Linq;
using StardewValley;
using SObject = StardewValley.Object;

namespace ItemPipes.Framework.Util
{
    public static class Utilities
    {
        public static string GetIDName(string name)
        {
            string trimmed = "";
            if (name.Equals("PIPO"))
            {
                trimmed = name;
            }
            else
            {
                trimmed = String.Concat(name.Where(c => !Char.IsWhiteSpace(c)));
            }
            return trimmed;
        }
		//Keep for 1.6
		/*
        public static Item GetItemFromIndex(string type, int index)
        {
			switch(type)
            {
				case "O":
					break;
            }
			if (item is SObject && (item as SObject).bigCraftable.Value)
			{

			}
			else if (item is Tool)
			{
				Tool tool = (Tool)item;
			}
			//Boots = standard
			else if (item is Boots)
			{

			}
			//rings = standard
			else if (item is Ring)
			{

			}
			else if (item is Hat)
			{

			}
			else if (item is Clothing)
			{

			}
			else
			{

			}
		}
		*/
    }
}

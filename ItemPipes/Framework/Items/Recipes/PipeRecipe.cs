using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemPipes.Framework.Model;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;
using StardewValley;
using SpaceCore;
using Microsoft.Xna.Framework.Graphics;

namespace ItemPipes.Framework.Items.Recipes
{
    public class PipeRecipe : CustomCraftingRecipe
    {
        public string IDName { get; set; }
        public override string Description { get; }
        public string ItemTexturePath { get; set; }
        public override Texture2D IconTexture { get; }
        public override Rectangle? IconSubrect { get; }
        public override IngredientMatcher[] Ingredients => throw new System.NotImplementedException();

        public PipeRecipe()
        {
            IconTexture = ModEntry.helper.Content.Load<Texture2D>(ItemTexturePath);
            LoadTextures();
        }

        public void LoadTextures()
        {
            ItemTexturePath = $"assets/Pipes/{IDName}/{IDName}_Item.png";
        }

        public override Item CreateResult()
        {
            throw new System.NotImplementedException();
        }
    }
}

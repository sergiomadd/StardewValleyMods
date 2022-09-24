# Chest Preview
Chest Preview provides a preview of a chest's items without opening the chest.
- [Read for translations](translations.md)

## Requirements:
- Stardew Valley 1.5.6
- [SMAPI](https://smapi.io/) 3.15.0

## Mod Authors (Compatibility):
- If you add your item's sprite to the vanilla spritesheet, it should work. If not, please create a new issue with the details.
- If you DON'T add your item's sprite to the vanilla spritesheet, and use a custom way of loading the sprite, consider the following:
  - Add your item's ParentSheetIndex to the [Chest Preview API](Framework/APIs/IChestPreviewAPI.cs), by using `void SendIDs(List<int> list)`
    - With this the preview will use the item's drawInMenu() function rather than my custom one.
  - If the item is miss aligned, then you can use the [API's](Framework/APIs/IChestPreviewAPI.cs) function  
  `void DrawInMenuPreview(int id, Action<SpriteBatch, Vector2, float, float, float, StackDrawType, Color, bool> draw) `  
  to provide the preview with your own drawInPreview() (a copy of drawInMenu()) function. Then it is up to you to change values until you align your item. 
    <details>
    <summary>DrawInMenuPreview() example:</summary>
  
    From ItemPipes compatibility (LINK)  
    Make sure your method has the same signature as the action.
    ```c#
  
    public void drawInPreview(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
    {
      //Same as drawInMenu(), but with modified values to align it
            bool shouldDrawStackNumber = ((drawStackNumber == StackDrawType.Draw && this.maximumStackSize() > 1 && this.Stack > 1)
          || drawStackNumber == StackDrawType.Draw_OneInclusive) && (double)scaleSize > 0.3 && this.Stack != int.MaxValue;
        Rectangle srcRect = new Rectangle(0, 0, 16, 16);
        spriteBatch.Draw(ItemTexture, location + new Vector2((int)(32f * scaleSize), (int)(32f * scaleSize)), srcRect, color * transparency, 0f,
          new Vector2(8f, 8f) * scaleSize, 4f * scaleSize, SpriteEffects.None, layerDepth);
        //If the stack numbers are missaligned, modify this
        if (shouldDrawStackNumber)
        {
          var loc = location + new Vector2((float)(64 - Utility.getWidthOfTinyDigitString(this.Stack, 3f * scaleSize)) + 3f * scaleSize, 64f - 18f * scaleSize + 2f);
          Utility.drawTinyDigits(this.Stack, spriteBatch, loc, 3f * scaleSize, 1f, color);
        }
    }
    ```  
  
    ```c#
    Item item = Factories.ItemFactory.CreateItemFromID(id);
    if(item is PipeItem)
    {
        try
        {
            Action<SpriteBatch, Vector2, float, float, float, StackDrawType, Color, bool> action = (item as PipeItem).drawInPreview;
            chestPreview.DrawInPreview(id, action);
        }
        catch(Exception e)
        {
            this.Monitor.Log("Error creating delegate drawInPreview method!", LogLevel.Error);
            this.Monitor.Log(e.Message, LogLevel.Error);
            this.Monitor.Log(e.StackTrace, LogLevel.Error);
        }
    }
    ```
    </details>
  

## Planned Features (in no order):


## See also
- [Release Notes](release-notes.md)
- [Nexus Mod Page]
- [Contact](https://twitter.com/madded__)

# Chest Preview
Chest Preview provides a preview of a chest's items without opening the chest.

## Requirements:
- Stardew Valley 1.5.6
- [SMAPI](https://smapi.io/) 3.15.0

## Mod Authors (Compatibility):
- If you add your item's sprite to the vanilla spritesheet, it should work. If not please create a new issue with the details.
- If you DON'T add your item's sprite to the vanilla spritesheet, and use a custom way of loading the sprite, consider using the [API](Framework.API.IChestPreview.cs)
  - Add your item's ParentSheetIndex to the ChestPreview API, by using `void SendIDs(List<int> list)`
    - With this the preview will use the item's drawInMenu function rather than my custom one.
  - If the item is miss aligned, then you can use the API's  
  `void DrawInMenuPreview(int id, Action<SpriteBatch, Vector2, float, float, float, StackDrawType, Color, bool> draw) `  
  This provides the preview with your own drawInMenuPreview (a copy of drawInMenu) function. Then it is up to you to change values to align your item. 

## Planned Features (in no order):


## See also
- [Read for translations](translations.md)
- [Release Notes](release-notes.md)
- [Nexus Mod Page]
- [Contact](https://twitter.com/madded__)

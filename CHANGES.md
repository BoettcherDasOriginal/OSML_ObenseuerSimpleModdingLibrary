# v0.1.0 (July 19th 2024)
## General
- updated to OSLoader v0.1.0-beta
## Detour
- added BuildingSystem.AddFurniture() detour
- added FurnitureShop.AddFurniture() detour
- added FurnitureShop.Restock() detour
- renamed SavableScriptableObjectDetour to FurnitureDetour
- cleaned ReplacementLoadFromPath() up
## Furniture
- added Furniture Handler -> Callback when Furniture gets loaded, which allows us to add custum scripts to the prefabs
- added Furniture Shop Restock Handler -> sell your furnitures!
- added Furniture Place Type
- changed some minor error handling
- fixed Furniture stacking issues

# v0.1.0-alpha (July 4th 2024) - Preview
## General
- added Public Vars
## Detour
- added Detour Utility
## Furniture
- added Furniture Creator
- added Furniture Config
- added AssetBundle support (Loading/Saving support)

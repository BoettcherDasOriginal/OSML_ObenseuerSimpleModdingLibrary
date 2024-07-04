using System;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using UnityEngine;

namespace OSML
{
    public class FurnitureConfig
    {
        /// <summary>
        /// The name of the furniture
        /// </summary>
        public string title;
        /// <summary>
        /// A short description of the furniture
        /// </summary>
        public string details;

        /// <summary>
        /// OC price
        /// </summary>
        public int priceOC;
        /// <summary>
        /// RM price
        /// </summary>
        public int priceRM;

        /// <summary>
        /// The location of the AssetBundle, which contains the image,prefab and preview prefab, relative to the .json path
        /// </summary>
        public string assetBundlePath;
        /// <summary>
        /// The name of the sprite in the AssetBundle defined at assetBundlePath
        /// </summary>
        public string imageName;
        /// <summary>
        /// The name of the GameObject/Prefab in the AssetBundle defined at assetBundlePath
        /// </summary>
        public string prefabName;
        /// <summary>
        /// The name of the GameObject/Prefab in the AssetBundle defined at assetBundlePath
        /// </summary>
        public string previewPrefabName;

        /// <summary>
        /// The category of the Furniture
        /// </summary>
        public FurnitureCategory category;
        /// <summary>
        /// Restricted areas to build the furniture -> Allowed to put everywhere if empty
        /// </summary>
        public FurnitureBuildingArea[] restrictedAreas;
        /// <summary>
        /// Should it be positioned like a normal furniture or like a painting or ceiling object?
        /// </summary>
        public FurnitureDisplayStyle displayStyle = FurnitureDisplayStyle.Default;
        /// <summary>
        /// Display rotation on the Y-Achses
        /// </summary>
        public int displayRotationY = 0;

        // To do:

        // public List<Furniture.ReseourceItem> dismantleItems;
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum FurnitureCategory
    {
        All,
        Chairs,
        Tables,
        Electronics,
        Paintings,
        Lights,
        Rugs,
        Items,
        Machines,
        Storage,
        Clutter,
        Beds,
        Sofas,
        Decoration,
        Plants,
        Shelves,
        Manufacturing,
        Growing,
        Letters,
        Bathroom,
        Signs,
        Magazines,
        Posters
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum FurnitureDisplayStyle
    {
        Default,
        Painting,
        Ceiling
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum FurnitureBuildingArea
    {
        None,
        Stairs,
        PlayerApartment,
        Workshop,
        Outside,
        Greenhouse
    }
}

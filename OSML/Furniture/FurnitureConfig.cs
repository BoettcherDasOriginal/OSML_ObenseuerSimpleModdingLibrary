using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using UnityEngine;

namespace OSML
{
    public class FurnitureConfig
    {
        public string title;
        public string details;

        public int priceOC;
        public int priceRM;

        public string assetBundlePath;
        public string imageName;
        public string prefabName;
        public string previewPrefabName;

        public FurnitureCategory category;
        public FurnitureBuildingArea[] restrictedAreas;
        public FurnitureDisplayStyle displayStyle = FurnitureDisplayStyle.Default;
        public int displayRotationY = 0;

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

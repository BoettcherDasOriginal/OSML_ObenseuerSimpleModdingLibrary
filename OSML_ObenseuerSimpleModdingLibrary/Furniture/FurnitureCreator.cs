using System;
using System.Collections.Generic;
using System.Text;
using OS;
using UnityEngine;

namespace OSML
{
    public class FurnitureCreator
    {
        public static Furniture NewFurniture(string title, Sprite image, string details, Furniture.Category category, int priceOC, int priceRM, Furniture.BuildingArea[] restrictedArea, List<Furniture.ReseourceItem> dismantleItems, GameObject prefab, GameObject previewPrefab, Furniture.DisplayStyle displayStyle, int displayRotationY)
        {
            Furniture furniture = ScriptableObject.CreateInstance<Furniture>();

            furniture.title = title;
            furniture.image = image;
            furniture.details = details;
            furniture.category = category;
            furniture.priceOC = priceOC;
            furniture.priceRM = priceRM;
            furniture.restrictedAreas = restrictedArea;
            furniture.dismantleItems = dismantleItems;
            furniture.prefab = prefab;
            furniture.previewPrefab = previewPrefab;
            furniture.displayStyle = displayStyle;
            furniture.displayRotationY = displayRotationY;

            return furniture;
        }
    }
}

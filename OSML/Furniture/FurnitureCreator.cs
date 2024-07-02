using System;
using System.Collections.Generic;
using System.Text;
using OS;
using UnityEngine;

namespace OSML
{
    public class FurnitureCreator
    {
        /// <summary>
        /// Creates a new Furniture (Just needs your raw furniture, no particular structure needed!)
        /// </summary>
        /// <param name="title">The name of your furniture</param>
        /// <param name="image">An image for the build menu</param>
        /// <param name="details">Description of your furniture</param>
        /// <param name="category">Is this a Table or what?</param>
        /// <param name="priceOC"></param>
        /// <param name="priceRM"></param>
        /// <param name="furniturePrefab">your raw furniture when build</param>
        /// <param name="furniturePreviewPrefab">your raw furniture when "preview"</param>
        /// <param name="restrictedArea">Allowed to put everywhere if empty</param>
        /// <param name="dismantleItems">?</param>
        /// <param name="displayStyle"></param>
        /// <param name="displayRotationY"></param>
        /// <returns></returns>
        public static Furniture NewFurniture(string title, Sprite image, string details, Furniture.Category category, int priceOC, int priceRM, GameObject furniturePrefab, GameObject furniturePreviewPrefab, Furniture.BuildingArea[] restrictedArea, List<Furniture.ReseourceItem> dismantleItems, Furniture.DisplayStyle displayStyle = Furniture.DisplayStyle.Default, int displayRotationY = 0)
        {
            Furniture furniture = ScriptableObject.CreateInstance<Furniture>();

            //Furniture Prefab Setup
            GameObject prefab = new GameObject(title);
            FurniturePlaceable furniturePlaceable = prefab.AddComponent<FurniturePlaceable>();
            furniturePlaceable.furniture = furniture;

            GameObject prefabRotate = new GameObject("rotate");
            prefabRotate.transform.parent = prefab.transform;

            furniturePrefab.transform.parent = prefabRotate.transform;

            prefab.layer = 12;

            //Furniture Preview Prefab Setup
            GameObject preview = new GameObject($"{title}-Preview");

            GameObject previewRotate = new GameObject("rotate");
            previewRotate.AddComponent<ObjectPreview>();
            previewRotate.transform.parent = preview.transform;

            //furniturePreviewPrefab.AddComponent<InsideTrigger>();
            furniturePreviewPrefab.transform.parent = previewRotate.transform;

            preview.layer = 11;

            //Furniture
            furniture.title = title;
            furniture.image = image;
            furniture.details = details;
            furniture.category = category;
            furniture.priceOC = priceOC;
            furniture.priceRM = priceRM;
            furniture.restrictedAreas = restrictedArea;
            furniture.dismantleItems = dismantleItems;
            furniture.prefab = prefab;
            furniture.previewPrefab = preview;
            furniture.displayStyle = displayStyle;
            furniture.displayRotationY = displayRotationY;

            return furniture;
        }
    }
}

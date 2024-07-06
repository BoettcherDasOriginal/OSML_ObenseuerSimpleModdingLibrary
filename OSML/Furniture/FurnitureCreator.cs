using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace OSML
{
    public class FurnitureCreator
    {
        public static Furniture NewFurniture(string title, Sprite image, string details, Furniture.Category category, int priceOC, int priceRM, GameObject furniturePrefab, GameObject furniturePreviewPrefab, Furniture.BuildingArea[] restrictedArea, List<Furniture.ReseourceItem> dismantleItems, FurniturePlaceType placeType, Furniture.DisplayStyle displayStyle = Furniture.DisplayStyle.Default, int displayRotationY = 0)
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
            ObjectPreview objectPreview =  previewRotate.AddComponent<ObjectPreview>();
            objectPreview.SetOrientationType((PlaceType)placeType);
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

        /// <summary>
        /// Uses a FurnitureConfig to create a Furniture
        /// </summary>
        /// <param name="furnitureData">Deserialized ur-furniture-name.json</param>
        /// <returns></returns>
        public static Furniture FurnitureConfigToFurniture(FurnitureConfig furnitureData)
        {
            if( furnitureData == null ) return null;

            // Convert OSML.FurnitureBuildingArea to OS.Furniture.BuildingArea
            Furniture.BuildingArea[] _resArea = new Furniture.BuildingArea[furnitureData.restrictedAreas.Length];
            for (int i = 0; i < furnitureData.restrictedAreas.Length; i++)
            {
                _resArea[i] = (Furniture.BuildingArea)furnitureData.restrictedAreas[i];
            }

            // Get the AssetBundle and loads the needed Assets
            var fileStream = new FileStream(furnitureData.assetBundlePath, FileMode.Open, FileAccess.Read);
            var assetBundle = AssetBundle.LoadFromStream(fileStream);

            Sprite image = assetBundle.LoadAsset<Sprite>(furnitureData.imageName);
            GameObject prefab = assetBundle.LoadAsset<GameObject>(furnitureData.prefabName);
            GameObject previewPrefab = assetBundle.LoadAsset<GameObject>(furnitureData.previewPrefabName);

            assetBundle.Unload(false);

            // Creates the Furniture
            Furniture furniture = NewFurniture(
                furnitureData.title,
                image,
                furnitureData.details,
                (Furniture.Category)furnitureData.category,
                furnitureData.priceOC,
                furnitureData.priceRM,
                prefab,
                previewPrefab,
                _resArea,
                new List<Furniture.ReseourceItem>(),
                furnitureData.placeType,
                (Furniture.DisplayStyle)furnitureData.displayStyle,
                furnitureData.displayRotationY
            );

            return furniture;
        }
    }
}

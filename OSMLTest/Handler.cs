using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using OSML;
using UnityEngine;
using System.IO;

namespace OSMLTest
{
    public class Handler
    {
        [FurnitureHandlerAttribute("OSML Box")]
        public static Furniture osmlBoxHandler(Furniture furniture)
        {
            Debug.Log($"[{furniture.title}] Hello!");

            return furniture;
        }

        [FurnitureShopRestockHandlerAttribute("OSMLTest")]
        public static List<BuildingSystem.FurnitureInfo> furShopRestockHandler(FurnitureShopName name)
        {
            List<BuildingSystem.FurnitureInfo> restock = new List<BuildingSystem.FurnitureInfo>();

            //Test
            string path = Path.Combine(Assembly.GetExecutingAssembly().Location.Substring(0, Assembly.GetExecutingAssembly().Location.Length - 13), "osml_box.json");

            if (File.Exists(path))
            {
                try
                {
                    string rawFurnitureConfig = File.ReadAllText(path);

                    FurnitureConfig? furnitureConfig = JsonConvert.DeserializeObject<FurnitureConfig>(rawFurnitureConfig);
                    if (furnitureConfig != null)
                    {
                        furnitureConfig.assetBundlePath = path.Substring(0, path.Length - Path.GetFileName(path).Length) + furnitureConfig.assetBundlePath;
                        Furniture f = FurnitureCreator.FurnitureConfigToFurniture(furnitureConfig);
                        f.addressableAssetPath = $"OSML_Furniture<#>{path}";

                        TaskItem taskItem = (TaskItem)ScriptableObject.CreateInstance(typeof(TaskItem));
                        taskItem.itemName = f.title;
                        taskItem.itemDetails = f.details;
                        taskItem.image = f.image;
                        taskItem.itemType = TaskItem.Type.Furnitures;

                        restock.Add(new BuildingSystem.FurnitureInfo(f, taskItem, null, 20, null));
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
            //Test

            return restock;
        }
    }
}

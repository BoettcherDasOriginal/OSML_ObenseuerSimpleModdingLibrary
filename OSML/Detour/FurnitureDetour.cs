using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;

namespace OSML.Detour
{
    public static class FurnitureDetour
    {
        #region SavableScriptableObject

        static ScriptableObject ReplacementLoadFromPath(this SavableScriptableObject obj)
        {
            if (!string.IsNullOrEmpty(obj.addressableAssetPath))
            {
                if(obj.addressableAssetPath.StartsWith("OSML_Furniture"))
                {
                    string sep = "<#>";
                    string path = obj.addressableAssetPath.Substring(obj.addressableAssetPath.IndexOf(sep) + 3);

                    try
                    {
                        if(File.Exists(path))
                        {
                            string rawFurnitureConfig = File.ReadAllText(path);

                            FurnitureConfig furnitureConfig = JsonConvert.DeserializeObject<FurnitureConfig>(rawFurnitureConfig);
                            furnitureConfig.assetBundlePath = path.Substring(0, path.Length - Path.GetFileName(path).Length) + furnitureConfig.assetBundlePath;
                            Furniture f = FurnitureCreator.FurnitureConfigToFurniture(furnitureConfig);
                            f.addressableAssetPath = $"OSML_Furniture<#>{path}";

                            if (PublicVars.furnitureHandlers.TryGetValue(f.title, out PublicVars.FurnitureHandler handler))
                            {
                                f = handler.Invoke(f);
                            }

                            return f;
                        }

                        return null;
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                        return null;
                    }
                }

                ScriptableObject result;
                try
                {
                    result = Addressables.LoadAssetAsync<ScriptableObject>(obj.addressableAssetPath).WaitForCompletion();
                }
                catch (Exception ex)
                {
                    Debug.LogError(string.Concat(new object[]
                    {
                    "Name: ",
                    obj.name,
                    " Type:",
                    obj.GetType(),
                    " Error: ",
                    ex
                    }));
                    result = null;
                }
                return result;
            }
            if (string.IsNullOrEmpty(obj.name))
            {
                return null;
            }
            if (obj.GetType() == typeof(TaskItem))
            {
                Debug.LogError(string.Concat(new object[]
                {
                "Path is empty! Name: ",
                (obj as TaskItem).itemName,
                " Type:",
                obj.GetType()
                }));
            }
            else
            {
                Debug.LogError(string.Concat(new object[]
                {
                "Path is empty! Name: ",
                obj.name,
                " Type:",
                obj.GetType()
                }));
            }
            return null;
        }

        public static void PatchSavableScriptableObject()
        {
            Debug.Log("[OSML] Trying to detour SavableScriptableObject.LoadFromPath()!");

            DetourUtility.TryDetourFromTo(
                src: DetourUtility.MethodInfoForMethodCall(() => default(SavableScriptableObject).LoadFromPath()),
                dst: DetourUtility.MethodInfoForMethodCall(() => ReplacementLoadFromPath(default))
            );
        }

        #endregion

        #region FurnitureShop

        public static void PatchFurnitureShop()
        {
            Debug.Log("[OSML] Trying to detour FurnitureShop.AddFurniture()!");

            DetourUtility.TryDetourFromTo(
                src: typeof(FurnitureShop).GetMethod("AddFurniture", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance),
                dst: typeof(FurnitureDetour).GetMethod("NewFSAddFurniture", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
            );

            /* Apprently Unity doesnt like this :(
            Debug.Log("[OSML] Trying to detour FurnitureShop.Restock()!");

            DetourUtility.TryDetourFromTo(
                src: typeof(FurnitureShop).GetMethod("Restock", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance),
                dst: typeof(FurnitureDetour).GetMethod("NewFSRestock", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
            );*/
        }

        static bool NewFSAddFurniture(this FurnitureShop fs, Furniture furniture, int amount = 1)
        {
            BuildingSystem.FurnitureInfo furnitureInfo = fs.availableFurnitures.Find((BuildingSystem.FurnitureInfo f) => f.furniture.title == furniture.title);
            if (furnitureInfo == null || furnitureInfo.furniture == null)
            {
                TaskItem taskItem = (TaskItem)ScriptableObject.CreateInstance(typeof(TaskItem));
                taskItem.itemName = furniture.title;
                taskItem.itemDetails = furniture.details;
                taskItem.image = furniture.image;
                taskItem.itemType = TaskItem.Type.Furnitures;
                fs.availableFurnitures.Add(new BuildingSystem.FurnitureInfo(furniture, taskItem, null, amount, null));
                return true;
            }
            furnitureInfo.amount += amount;
            return true;
        }

        static void NewFSRestock(this FurnitureShop fs)
        {
            fs.MoneyRestock();

            MethodInfo methodInfo = typeof(FurnitureShop).GetMethod("Restock", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            methodInfo.Invoke(fs, null);

            Debug.Log($"[OSML] Restocking '{(fs.title != "" ? fs.title : "One Stop Shop")}'");
        }

        #endregion

        #region BuildingSystem

        public static void PatchBuildingSystem()
        {
            Debug.Log("[OSML] Trying to detour BuildingSystem.AddFurniture()!");

            DetourUtility.TryDetourFromTo(
                src: typeof(BuildingSystem).GetMethod("AddFurniture", BindingFlags.Public | BindingFlags.Instance),
                dst: typeof(FurnitureDetour).GetMethod("NewBSAddFurniture", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
            );
        }

        static bool NewBSAddFurniture(this BuildingSystem bs,Furniture furniture, GameObject gameObject, out GameObject savedGameObject, int amount = 1)
        {
            BuildingSystem.FurnitureInfo furnitureInfo = bs.availableFurnitures.Find((BuildingSystem.FurnitureInfo f) => f.furniture.title == furniture.title && f.gameObject == null);
            if (gameObject != null)
            {
                gameObject.transform.SetParent((bs.inventoryLocation != null) ? bs.inventoryLocation : bs.transform);
                gameObject.transform.localPosition = Vector3.zero;
                if (!bs.HasSaveableContent(gameObject))
                {
                    UnityEngine.Object.Destroy(gameObject);
                    gameObject = null;
                }
            }
            savedGameObject = gameObject;
            BuildingSystem.FurnitureInfo info = bs.availableFurnitures.Find((BuildingSystem.FurnitureInfo f) => f.furniture.title == furniture.title);
            TaskItem taskItem = BSAddTaskItem(furniture, info, amount);
            if (furnitureInfo == null || furnitureInfo.furniture == null || gameObject != null)
            {
                bs.availableFurnitures.Add(new BuildingSystem.FurnitureInfo(furniture, taskItem, gameObject, amount, null));
                bs.availableFurnitures.Sort((BuildingSystem.FurnitureInfo slot1, BuildingSystem.FurnitureInfo slot2) => slot1.furniture.name.CompareTo(slot2.furniture.name));
                return true;
            }
            furnitureInfo.amount += amount;
            return true;
        }

        public static TaskItem BSAddTaskItem(Furniture furniture, BuildingSystem.FurnitureInfo info, int amount)
        {
            TaskItem taskItem;
            if (info == null || info.furniture == null)
            {
                taskItem = (TaskItem)ScriptableObject.CreateInstance(typeof(TaskItem));
                taskItem.itemName = furniture.title;
                taskItem.itemDetails = furniture.details;
                taskItem.image = furniture.image;
                taskItem.itemType = TaskItem.Type.Furnitures;
                TaskItemsManager.instance.AddTaskItem(taskItem, amount, false, null, false);
            }
            else
            {
                taskItem = info.taskItem;
                TaskItemsManager.instance.AddTaskItem(info.taskItem, amount, false, null, false);
            }
            return taskItem;
        }

        #endregion
    }
}

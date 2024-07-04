using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.IO;
using Newtonsoft.Json;

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

                            if(PublicVars.furnitureHandlers.ContainsKey(f.title))
                            {
                                f = PublicVars.furnitureHandlers[f.title].Invoke(f);
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
            
        }

        #endregion
    }
}

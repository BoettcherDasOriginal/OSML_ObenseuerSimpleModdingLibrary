using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using OSMLUnity;

namespace OSML.Detour
{
    public static class SavableScriptableObjectDetour
    {
        static ScriptableObject ReplacementLoadFromPath(this SavableScriptableObject obj)
        {
            if (!string.IsNullOrEmpty(obj.addressableAssetPath))
            {
                if(obj.addressableAssetPath.StartsWith("OSML_Furniture"))
                {
                    //string.Spli() doesn't work here -> obj.addressableAssetPath.Split("<#>"); == error
                    string[] data = new string[3];
                    string sep = "<#>";

                    int first = obj.addressableAssetPath.IndexOf(sep);
                    int last = obj.addressableAssetPath.LastIndexOf(sep);

                    data[0] = obj.addressableAssetPath.Substring(0, first);
                    data[1] = obj.addressableAssetPath.Substring(first + 3, last - first - 3);
                    data[2] = obj.addressableAssetPath.Substring(last + 3);

                    Debug.Log(data[0]);
                    Debug.Log(data[1]);
                    Debug.Log(data[2]);
                    try
                    {
                        string name = data[1];
                        string path = data[2];

                        var assetBundle = AssetBundle.LoadFromFile(path);
                        GameObject prefab = assetBundle.LoadAsset<GameObject>(name);

                        OSMLFurniture osmlFurniture = prefab.GetComponent<OSMLFurniture>();
                        return FurnitureCreator.OSMLFurnitureToOS(osmlFurniture);
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
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
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace OSML.Detour
{
    public static class SavableScriptableObjectDetour
    {
        static ScriptableObject ReplacementLoadFromPath(this SavableScriptableObject obj)
        {
            if (!string.IsNullOrEmpty(obj.addressableAssetPath))
            {
                if(obj.addressableAssetPath == "OSML")
                {
                    Debug.Log("[OSML] IT JUST WORKS!!!!");
                    return null;
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

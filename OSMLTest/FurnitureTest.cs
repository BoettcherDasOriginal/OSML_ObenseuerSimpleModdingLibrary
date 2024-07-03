using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using OSML;
using UnityEngine;

namespace OSMLTest
{
    public class FurnitureTest : MonoBehaviour
    {
        private bool _trigger = true;

        private void Update()
        {
            if(PublicVars.instance.firstUpdateFinished && _trigger)
            {
                _trigger = false;

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

                            f.GiveItem();
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }
        }
    }
}

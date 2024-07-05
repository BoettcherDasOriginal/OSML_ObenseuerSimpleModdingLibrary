using System;
using Newtonsoft.Json;
using System.Reflection;
using OSLoader;
using OSML;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace OSMLTest
{
    public class OSMLTest : Mod
    {
        public override void OnModLoaded()
        {
            base.OnModLoaded();

            logger.Log("Hi!");

            SceneManager.sceneLoaded += OnSceneLoaded;

            PublicVars.AddFurnitureHandlers(typeof(Handler));
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (!PublicVars.instance.isInitialized) return;

            if (scene.buildIndex != 0)
            {
                GameObject ft = new GameObject("FurnitureTest", typeof(FurnitureTest));
            }

            /*
            if(scene.buildIndex == 11)
            {
                var shop = GameObject.FindObjectOfType<FurnitureShop>();
                if (shop != null)
                {
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

                                shop.AddFurniture(f, 3);
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e);
                        }
                    }
                }
            }*/
        }
    }
}

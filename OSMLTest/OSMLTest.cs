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
            PublicVars.AddFurnitureShopRestockHandlers(typeof(Handler));
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (!PublicVars.instance.isInitialized) return;

            if (scene.buildIndex != 0)
            {
                GameObject ft = new GameObject("FurnitureTest", typeof(FurnitureTest));
            }
        }
    }
}

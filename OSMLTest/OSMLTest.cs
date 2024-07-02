using System;
using OSLoader;
using OSML;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OSMLTest
{
    public class OSMLTest : Mod
    {
        public override void OnModLoaded()
        {
            base.OnModLoaded();

            logger.Log("Hi!");

            SceneManager.sceneLoaded += OnSceneLoaded;
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

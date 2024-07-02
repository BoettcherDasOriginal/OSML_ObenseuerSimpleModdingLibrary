using System;
using System.Collections.Generic;
using System.Text;
using BehaviorDesigner.Runtime.Tasks;
using OSLoader;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OSML
{
    public class ObenseuerSimpleModdingLibrary : Mod
    {
        public override void OnModLoaded()
        {
            base.OnModLoaded();

            new PublicVars();
            logger.Log("Initializing OSML...");

            PublicVars.instance.version = config.version;

            SceneManager.sceneLoaded += OnSceneLoaded;

            logger.Log($"OSML version {config.version} Initialized!");
            PublicVars.instance.isInitialized = true;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if(!PublicVars.instance.isInitialized) return;

            logger.Log($"Scene: {scene.buildIndex}, {scene.name} loaded!");

            PublicVars.instance.lastLoadedScene = scene.buildIndex;
            PublicVars.instance.firstUpdateFinished = false;

            if(scene.buildIndex != 0)
            {
                GameObject sro = new GameObject("SceneRuntimeObject", typeof(SceneRuntimeObject));
            }
        }
    }

    public class PublicVars
    {
        public static PublicVars instance;

        public string version;
        public bool isInitialized;

        public int lastLoadedScene = 0;

        public bool firstUpdateFinished;

        public PublicVars()
        {
            if( instance == null ) instance = this;
            else return;
        }
    }
}

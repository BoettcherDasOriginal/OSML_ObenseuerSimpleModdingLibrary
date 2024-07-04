using System;
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

            Detour.SavableScriptableObjectDetour.PatchSavableScriptableObject();

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

    /// <summary>
    /// Provides Public Vars, so you dont need to get them everytime!
    /// </summary>
    public class PublicVars
    {
        public static PublicVars instance;

        /// <summary>
        /// The current OSML version
        /// </summary>
        public string version;

        public bool isInitialized;

        /// <summary>
        /// The build index of the last scene during the "SceneManager.sceneLoaded" callback
        /// </summary>
        public int lastLoadedScene = 0;

        /// <summary>
        /// You want to execute your mod logic after this (firstUpdateFinished = true) to make sure that all game logic is already initialized!
        /// </summary>
        public bool firstUpdateFinished;

        public PublicVars()
        {
            if( instance == null ) instance = this;
            else return;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            PublicVars.furnitureHandlers = new Dictionary<string, PublicVars.FurnitureHandler>();

            logger.Log("Initializing OSML...");

            PublicVars.instance.version = config.version;

            SceneManager.sceneLoaded += OnSceneLoaded;

            Detour.FurnitureDetour.PatchSavableScriptableObject();
            Detour.FurnitureDetour.PatchBuildingSystem();

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

        public delegate Furniture FurnitureHandler(Furniture furniture);

        public static Dictionary<string, FurnitureHandler> furnitureHandlers;

        public PublicVars()
        {
            if( instance == null ) instance = this;
            else return;
        }

        public static bool AddFurnitureHandlers(Type type)
        {
            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).Where(m => m.GetCustomAttributes(typeof(FurnitureHandlerAttribute), false).Length > 0).ToArray();

            foreach(MethodInfo method in methods)
            {
                FurnitureHandlerAttribute attribute = method.GetCustomAttribute<FurnitureHandlerAttribute>();

                if(!method.IsStatic)
                {
                    Debug.LogError($"[OSML] '{method.DeclaringType.Name}.{method.Name}' is an instance method, but furniture handler methods must be static");
                    return false;
                }

                Delegate furnitureHandler = Delegate.CreateDelegate(typeof(FurnitureHandler), method, false);
                if(furnitureHandler != null)
                {
                    if(furnitureHandlers.ContainsKey(attribute.FurnitureTitle))
                    {
                        Debug.LogError($"[OSML] DuplicateHandlerException: '{method.DeclaringType}.{method.Name}' Only one handler method is allowed per furniture!");
                        return false;
                    }
                    else
                    {
                        furnitureHandlers.Add(attribute.FurnitureTitle, (FurnitureHandler)furnitureHandler);
                    }
                }
                else
                {
                    Debug.LogError($"[OSML] InvalidHandlerSignatureException: '{method.DeclaringType}.{method.Name}' doesn't match any acceptable furniture handler method signatures! Furniture handler methods should have a 'Furniture' parameter and should return 'Furniture'.");
                    return false;
                }
            }

            return true;
        }
    }
}

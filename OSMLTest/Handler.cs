using System;
using System.Collections.Generic;
using System.Text;
using OSML;
using UnityEngine;

namespace OSMLTest
{
    public class Handler
    {
        [FurnitureHandlerAttribute("OSML Box")]
        public static Furniture osmlBoxHandler(Furniture furniture)
        {
            Debug.Log($"[{furniture.title}] Hello!");

            return furniture;
        }
    }
}

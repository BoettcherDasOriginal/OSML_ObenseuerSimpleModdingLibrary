﻿using System;
using System.Collections.Generic;
using System.Text;
using OSLoader;

namespace OSML_ObenseuerSimpleModdingLibrary
{
    public class ObenseuerSimpleModdingLibrary : Mod
    {
        public override void OnModLoaded()
        {
            base.OnModLoaded();
            logger.Log($"Library version {config.version} loaded!");
        }
    }
}
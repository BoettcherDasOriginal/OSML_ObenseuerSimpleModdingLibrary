using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OSML
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class FurnitureHandlerAttribute : System.Attribute
    {
        public readonly string FurnitureTitle;

        public FurnitureHandlerAttribute(string furnitureTitle)
        {
            FurnitureTitle = furnitureTitle;
        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class FurnitureShopRestockHandlerAttribute : System.Attribute
    {
        public readonly string HandlerUID;

        public FurnitureShopRestockHandlerAttribute(string handlerUID)
        {
            HandlerUID = handlerUID;
        }
    }

    public enum FurnitureShopName
    {
        None,
        [Description("One Stop Shop")]
        OneStopShop,
        [Description("Möbelmann Furnitures")]
        MoebelmannFurnitures,
        [Description("Jonasson's Shop")]
        SamuelJonasson
    }
}

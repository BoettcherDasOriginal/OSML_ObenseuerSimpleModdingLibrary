using System;
using System.Collections.Generic;
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
}

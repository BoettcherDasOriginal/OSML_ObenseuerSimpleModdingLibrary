using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OSML
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class ItemDatabaseHandlerAttribute : System.Attribute
    {
        public readonly string HandlerUID;

        public ItemDatabaseHandlerAttribute(string handlerUID)
        {
            HandlerUID = handlerUID;
        }
    }
}

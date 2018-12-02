using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.Proxying.Emitting.Utility
{
    public static class AttibutesUtil
    {
        public static readonly MethodAttributes MethodDefaultAttributes =
               MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.Public;

        public static readonly CallingConventions InstanceCallingConvention = CallingConventions.HasThis;

        public static readonly CallingConventions StaticCallingConvention = CallingConventions.Standard;
    }
}

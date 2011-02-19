using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NGM.OpenAuthentication.Core {
    internal static class OrchardShapeChecker {
        internal static bool HasRegisterAsShape() {
            var version = new System.Reflection.AssemblyName(typeof (Orchard.ContentManagement.ContentItem).Assembly.FullName).Version.ToString();
            var versionInt = int.Parse(version.Replace(".", string.Empty));
            return versionInt > 10200;
        }
    }
}
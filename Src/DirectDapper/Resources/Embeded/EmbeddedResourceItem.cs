using System;
using System.IO;
using System.Reflection;

namespace DirectDapper.Resources.Embedded
{
    /// <summary>
    /// Stores needed information of an embedded resource.
    /// </summary>
    public class EmbeddedResourceItem : ResourceItem
    {
        /// <summary>
        /// The assembly that contains the resource.
        /// </summary>
        public Assembly Assembly { get; set; }

        internal EmbeddedResourceItem(string fileName, byte[] content, Assembly assembly):base(fileName,content)
        {

            Assembly = assembly;
            LastModifiedUtc = Assembly.Location != null
                ? new FileInfo(Assembly.Location).LastWriteTimeUtc
                : DateTime.UtcNow;
        }
    }
}
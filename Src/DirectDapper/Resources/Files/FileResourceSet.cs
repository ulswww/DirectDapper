using System.Collections.Generic;
using System.IO;
using DirectDapper.Extensions;

namespace DirectDapper.Resources.Files
{
    public class FileResourceSet : ResourceSet
    {
        private readonly string _physicalPath;
        public FileResourceSet(string rootPath,string physicalPath) : base(rootPath)
        {
            this._physicalPath = physicalPath.EnsureEndsWith(Path.DirectorySeparatorChar);

             RootPath = rootPath.Trim('/').Trim('\\').EnsureEndsWith(Path.DirectorySeparatorChar);
        }

        internal override void AddResources(Dictionary<string, ResourceItem> resources)
        {

            var files = Directory.GetFiles(_physicalPath,"*.*",SearchOption.AllDirectories);

            foreach (var filename in files)
            {
                 var relativePath =  RootPath + GetRelativePath(_physicalPath,filename);

                  var filePath =  ResourcePathHelper.NormalizePath(relativePath.Replace(Path.DirectorySeparatorChar,'/'));

                    resources[filePath] = new FileResourceItem(
                        Id,
                        filename,
                        filePath,
                        File.ReadAllBytes(filename)
                    );

            }
        }        
        
        private string GetRelativePath(string baseDirectory, string filename)
        {
            return filename.Substring(baseDirectory.Length).TrimStart(Path.DirectorySeparatorChar);
        }
    }
}
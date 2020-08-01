using System.Collections.Generic;
using System.IO;
using DirectDapper.Extensions;

namespace DirectDapper.Resources.Files
{
    public class FileResourceSet : ResourceSet
    {
        private readonly string _parentPath;
        public FileResourceSet(string parentPath, string rootPath) : base(rootPath)
        {
            this._parentPath = parentPath.EnsureEndsWith(Path.DirectorySeparatorChar);

             RootPath = rootPath.Trim('/').Trim('\\').EnsureEndsWith(Path.DirectorySeparatorChar);
        }

        internal override void AddResources(Dictionary<string, ResourceItem> resources)
        {
            var path = Path.Combine(_parentPath,RootPath);

            var files = Directory.GetFiles(path,"*.*",SearchOption.AllDirectories);

            foreach (var filename in files)
            {
                 var relativePath = GetRelativePath(_parentPath,filename);

                  var filePath =  ResourcePathHelper.NormalizePath(relativePath.Replace(Path.DirectorySeparatorChar,'/'));

                    resources[filePath] = new FileResourceItem(
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
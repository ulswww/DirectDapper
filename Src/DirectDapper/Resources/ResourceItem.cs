using System;

namespace DirectDapper.Resources
{
    public abstract  class ResourceItem
    {
        /// <summary>
        /// File name including extension.
        /// </summary>
        public string FileName { get; }

        public string FileExtension { get; }

        /// <summary>
        /// Content of the resource file.
        /// </summary>
        public byte[] Content { get; set; }

        public DateTime LastModifiedUtc { get; protected set;}

        internal ResourceItem(string fileName, byte[] content)
        {
            FileName = fileName;
            Content = content;
            FileExtension = CalculateFileExtension(FileName);
        }

        private static string CalculateFileExtension(string fileName)
        {
            if (!fileName.Contains("."))
            {
                return null;
            }

            return fileName.Substring(fileName.LastIndexOf(".", StringComparison.Ordinal) + 1);
        }
    }
}
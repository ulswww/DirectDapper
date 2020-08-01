namespace DirectDapper.Resources.Files
{
    public class FileResourceItem : ResourceItem
    {
        private readonly string _fullFilename;

        public FileResourceItem(string fullFilename,string fileName, byte[] content) : base(fileName, content)
        {
            this._fullFilename = fullFilename;
        }
    }
}
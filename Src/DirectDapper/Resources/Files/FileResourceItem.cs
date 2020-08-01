namespace DirectDapper.Resources.Files
{
    public class FileResourceItem : ResourceItem
    {
        private readonly string _fullFilename;

        public FileResourceItem(string setId,string fullFilename,string fileName, byte[] content) : base(setId, fileName, content)
        {
            this._fullFilename = fullFilename;
        }
    }
}
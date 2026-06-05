namespace DirectoryWatcher.Models
{
    public class FileEntry
    {
        public string RelativePath { get; set; } = "";
        public int Version { get; set; } = 1;
        public DateTime LastModified { get; set; }
    }
}

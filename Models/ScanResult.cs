namespace DirectoryWatcher.Models
{
    public class ScanResult
    {
        public string WatchedPath { get; set; } = "";
        public List<FileEntry> Added { get; set; } = new();
        public List<FileEntry> Modified { get; set; } = new();
        public List<FileEntry> Removed { get; set; } = new();
        public List<FileEntry> Unchanged { get; set; } = new();
        public List<string> AddedDir { get; set; } = new();
        public List<string> RemovedDir { get; set; } = new();
    }
}

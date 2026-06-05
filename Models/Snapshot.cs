namespace DirectoryWatcher.Models
{
    public class Snapshot
    {
        public string WatchedPath { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public Dictionary<string, FileEntry> Entries { get; set; } = new();
        public List<string> SubDirs { get; set; } = new();
    }
}

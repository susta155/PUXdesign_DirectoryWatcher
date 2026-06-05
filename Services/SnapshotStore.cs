using DirectoryWatcher.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace DirectoryWatcher.Services
{
    public interface ISnapshotStore
    {
        Snapshot? Load(string watchedPath);
        void Save(Snapshot snapshot);
        void Delete(string watchedPath);
    }
    public class SnapshotStore : ISnapshotStore
    {
        private readonly string _storageDir;

        private static readonly SemaphoreSlim _lock = new(1, 1);
        public SnapshotStore()
        {
            _storageDir = Path.Combine(AppContext.BaseDirectory, "snapshots"); // Probably not optimal — should use some kind of env variable, but for this exercise probably sufficient.
            Directory.CreateDirectory(_storageDir);
        }

        private string SnapshotPath(string watchedPath)
        {
            var safeName = watchedPath
                            .ToLowerInvariant()
                            .Replace(":", "")
                            .Replace("\\", "_")
                            .Replace("/", "_");
            return Path.Combine(_storageDir, $"snapshot_{safeName}.json");
        }

        public void Save(Snapshot snapshot)
        {
            _lock.Wait();
            try
            {
                var path = SnapshotPath(snapshot.WatchedPath);
                var json = JsonSerializer.Serialize(snapshot, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                File.WriteAllText(path, json);
            }
            finally { _lock.Release(); }
        }
        public Snapshot? Load(string watchedPath)
        {
            var path = SnapshotPath(watchedPath);
            if (!File.Exists(path)) return null;
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Snapshot>(json);
        }

        public void Delete(string watchedPath) // should probably track somehow if deleton happend but should be used only for debugging so probably ok
        {
            var path = SnapshotPath(watchedPath);
            if (File.Exists(path)) File.Delete(path);
        }
    }
}

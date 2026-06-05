using DirectoryWatcher.Models;
using System.Security.Cryptography;

namespace DirectoryWatcher.Services
{
    public interface IDirectoryScanner
    {
        ScanResult Scan(string watchedPath);
    }

    public class DirectoryScanner : IDirectoryScanner
    {
        ISnapshotStore _snapshotStore;
        

        public DirectoryScanner(ISnapshotStore snapshotStore)
        {
            _snapshotStore = snapshotStore;
        }

        public ScanResult Scan(string watchedPath)
        {
            var result = new ScanResult
            {
                WatchedPath = watchedPath
            };            

            var current = BuildCurrentSnapshot(watchedPath);
            var previous = _snapshotStore.Load(watchedPath);

            var newSnapshot = current;

            if (previous is null)
            {                
                result.AddedDir = current.SubDirs;
                result.Added = current.Entries.Values.ToList();                
            }
            else
            {
                var remainingPrevious = new Dictionary<string, FileEntry>(previous.Entries, StringComparer.OrdinalIgnoreCase);

                foreach (var (key,fileEntry) in current.Entries)
                {
                    
                    if (!previous.Entries.TryGetValue(key, out var oldEntry))
                    {
                        result.Added.Add(fileEntry);
                    }
                    else
                    {
                        remainingPrevious.Remove(key);
                        if (fileEntry.LastModified != oldEntry.LastModified)
                        {
                            newSnapshot.Entries[key].Version = fileEntry.Version + 1;
                            result.Modified.Add(fileEntry);
                        }
                        else
                        {
                            fileEntry.Version = oldEntry.Version;
                            result.Unchanged.Add(fileEntry);
                        }
                    }
                }

                foreach (var (_,fileEntry) in remainingPrevious)
                {
                    result.Removed.Add(fileEntry);
                }

                var previousDirs = previous.SubDirs;
                var currentDirs = current.SubDirs;

                result.RemovedDir = previousDirs.Except(currentDirs).ToList();
                result.AddedDir = currentDirs.Except(previousDirs).ToList();
                
            }

            newSnapshot = current;

            _snapshotStore.Save(newSnapshot);
            return result;
        }

            
        
        private Snapshot BuildCurrentSnapshot(string root)
        {
            Snapshot snapshot = new Snapshot
            {
                WatchedPath = root,
                CreatedAt = DateTime.UtcNow
            };

            foreach (var dir in Directory.EnumerateDirectories(root, "*", SearchOption.AllDirectories))
            {
                var rel = Path.GetRelativePath(root, dir);
                snapshot.SubDirs.Add(rel);
            }

            foreach (var file in Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories))
            {
                var rel = Path.GetRelativePath(root, file);
                snapshot.Entries[rel] = new FileEntry
                {
                    RelativePath = rel,
                    LastModified = File.GetLastWriteTimeUtc(file)
                };
            }

            return snapshot;
        }

    }
}

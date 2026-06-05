using Microsoft.AspNetCore.Mvc;
using DirectoryWatcher.Services;

namespace DirectoryWatcher.Controllers;


[ApiController]
[Route("api/[controller]")]
public class WatcherController : ControllerBase
{
    private readonly IDirectoryScanner _scanner;
    private readonly ISnapshotStore _store;
    public WatcherController(IDirectoryScanner scanner, ISnapshotStore store)
    {
        _scanner = scanner;
        _store = store;
    }

    [HttpPost("scan")]
    public IActionResult Scan([FromQuery] string path)
    {
        try
        {
            var result = _scanner.Scan(path);
            return Ok(result);
        }
        catch(Exception ex)
        {
            return StatusCode(403, new { error = ex });
        }
    }    

    [HttpDelete("deletepathsnapshot")]
    public IActionResult DeleteSnapshot([FromQuery] string path)
    {
        _store.Delete(path);
        return Ok("OK");
    }
}
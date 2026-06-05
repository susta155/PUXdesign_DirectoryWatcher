using DirectoryWatcher.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<ISnapshotStore, SnapshotStore>();
builder.Services.AddScoped<IDirectoryScanner, DirectoryScanner>();
var app = builder.Build();

app.MapControllers();
app.Run();
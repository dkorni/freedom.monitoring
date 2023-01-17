using Freedom.ServerMonitor.DbRepository.IoC;
using Freedom.ServerMonitor.Logic.IoC;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run();
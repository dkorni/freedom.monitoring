using System.Text;
using Freedom.ServerMonitor.Domain.Interfaces;
using Freedom.ServerMonitor.Extensions;
using Freedom.ServerMonitor.IoC;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.AddHostDependencies();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Application started");
    using (var scope = app.Services.CreateScope())
    {
        scope.ServiceProvider.GetRequiredService<IServerWatcher>().Run();
      
    }
    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Application failed to start");
}
finally
{
    Log.CloseAndFlush();
}

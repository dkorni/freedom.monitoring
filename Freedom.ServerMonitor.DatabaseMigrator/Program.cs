// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Freedom.ServerMonitor.Logic.IoC;


var builder = new ServiceCollection();
builder.AddKeyVault()
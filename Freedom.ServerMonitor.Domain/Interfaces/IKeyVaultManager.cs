namespace Freedom.ServerMonitor.Domain.Interfaces;

public interface IKeyVaultManager
{
    string GetSecret(string key);
}
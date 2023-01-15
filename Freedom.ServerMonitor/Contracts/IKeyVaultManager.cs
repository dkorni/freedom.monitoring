namespace Freedom.ServerMonitor.Contracts;

public interface IKeyVaultManager
{
    string GetSecret(string key);
}
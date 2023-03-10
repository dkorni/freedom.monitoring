using Azure.Security.KeyVault.Secrets;
using Freedom.ServerMonitor.Domain.Interfaces;

namespace Freedom.ServerMonitor.Logic;

public class KeyVaultManager : IKeyVaultManager
{
    private readonly SecretClient _secretClient;

    public KeyVaultManager(SecretClient secretClient)
    {
        _secretClient = secretClient;
    }

    public string GetSecret(string key)
    {
        try
        {
            var response = _secretClient.GetSecret(key);
            return response.Value.Value;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
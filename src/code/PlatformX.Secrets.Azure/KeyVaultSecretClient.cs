using Azure.Security.KeyVault.Secrets;
using PlatformX.Secrets.Shared.Behaviours;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace PlatformX.Secrets.Azure
{
    [ExcludeFromCodeCoverage]
    public class KeyVaultSecretClient : ISecretClient
    {
        private readonly SecretClient _secretClient;
        public KeyVaultSecretClient(SecretClient secretClient)
        {
            _secretClient = secretClient;
        }

        public string GetSecret(string keyName)
        {
            return _secretClient.GetSecret(keyName).Value.Value;
        }

        public Task<string> GetSecretAsync(string keyName)
        {
            throw new System.NotImplementedException();
        }

        public void PurgeDeletedSecret(string keyName)
        {
            _secretClient.PurgeDeletedSecret(keyName);
        }

        public void SetSecret(string keyName, string value)
        {
            _secretClient.SetSecret(keyName, value);
        }

        public Task<bool> SetSecretAsync(string keyName, string value)
        {
            throw new System.NotImplementedException();
        }

        public void StartDeleteSecret(string keyName)
        {
            _secretClient.StartDeleteSecret(keyName);
        }

        bool ISecretClient.SetSecret(string keyName, string value)
        {
            throw new System.NotImplementedException();
        }
    }
}

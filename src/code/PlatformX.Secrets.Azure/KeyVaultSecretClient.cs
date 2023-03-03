using Azure.Security.KeyVault.Secrets;
using PlatformX.Secrets.Shared.Behaviours;
using System.Diagnostics.CodeAnalysis;

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

        public void PurgeDeletedSecret(string keyName)
        {
            _secretClient.PurgeDeletedSecret(keyName);
        }

        public void SetSecret(string keyName, string value)
        {
            _secretClient.SetSecret(keyName, value);
        }

        public void StartDeleteSecret(string keyName)
        {
            _secretClient.StartDeleteSecret(keyName);
        }

        //private SecretClient GetSecretClient(string vaultUrl, string tenantId)
        //{
        //    if (string.IsNullOrEmpty(vaultUrl))
        //    {
        //        throw new ArgumentNullException(nameof(vaultUrl), $"{nameof(vaultUrl)} is null in GetSecretClient");
        //    }

        //    if (string.IsNullOrEmpty(tenantId))
        //    {
        //        throw new ArgumentNullException(nameof(tenantId), $"{nameof(tenantId)} is null in GetSecretClient");
        //    }

        //    if (_secretLoaderConfiguration.Environment == "local")
        //    {
        //        return new SecretClient(new Uri(vaultUrl), new VisualStudioCredential(new VisualStudioCredentialOptions { TenantId = tenantId }));
        //    }
        //    else
        //    {
        //        return new SecretClient(new Uri(vaultUrl), new DefaultAzureCredential(new DefaultAzureCredentialOptions { SharedTokenCacheTenantId = tenantId }));
        //    }
        //}
    }
}

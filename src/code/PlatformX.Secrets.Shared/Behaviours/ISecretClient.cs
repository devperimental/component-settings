using System;
namespace PlatformX.Secrets.Shared.Behaviours
{
    public interface ISecretClient
    {
        string GetSecret(string keyName);
        void PurgeDeletedSecret(string keyName);
        void SetSecret(string keyName, string value);
        void StartDeleteSecret(string keyName);
    }
}

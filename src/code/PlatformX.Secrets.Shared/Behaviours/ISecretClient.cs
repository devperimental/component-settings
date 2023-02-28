using System;
using System.Collections.Generic;
using System.Text;

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

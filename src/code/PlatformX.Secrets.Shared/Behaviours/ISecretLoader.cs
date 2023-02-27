using System;
using System.Collections.Generic;

namespace PlatformX.Secrets.Shared.Behaviours
{
    public interface ISecretLoader
    {
        Dictionary<string, string> LoadSecrets(List<string> keyList);
        string LoadSecret(string secretName);
        string LoadClientSecret(string secretName, string regionKey, string locationKey);
        void SaveSecret(string secretName, string value);
        void SaveClientSecret(string secretName, string value, string regionKey, string locationKey);
        void DeleteSecret(string secretName);
        void DeleteClientSecret(string secretName, string regionKey, string locationKey);
        void PopulateSecrets(Dictionary<string, string> secrets);
    }
}
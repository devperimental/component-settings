﻿using System;
using System.Threading.Tasks;

namespace PlatformX.Secrets.Shared.Behaviours
{
    public interface ISecretClient
    {
        string GetSecret(string keyName);
        Task<string> GetSecretAsync(string keyName);
        void PurgeDeletedSecret(string keyName);
        bool SetSecret(string keyName, string value);
        Task<bool> SetSecretAsync(string keyName, string value);
        void StartDeleteSecret(string keyName);
    }
}

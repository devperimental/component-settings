using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformX.Settings.Shared.Behaviours
{
    public interface IEndpointHelper
    {
        string GetStorageAccount();
        string GetServiceBusNamespace();
        string GetKeyVaultUrl();
        string GetClientStorageAccount(string regionKey, string locationKey);
        string GetClientServiceBusNamespace(string regionKey, string locationKey);
        string GetClientKeyVaultUrl(string regionKey, string locationKey);
        string GetManagementStorageAccount(string regionKey, string locationKey);
        string GetManagementServiceBusNamespace(string regionKey, string locationKey);
        string GetManagementKeyVaultUrl(string regionKey, string locationKey);
    }
}

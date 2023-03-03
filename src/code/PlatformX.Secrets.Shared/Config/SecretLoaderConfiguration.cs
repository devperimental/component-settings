using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformX.Secrets.Shared.Config
{
    public class SecretLoaderConfiguration
    {
        public string Environment { get; set; }
        public string TenantId { get; set; }
        public object Prefix { get; set; }
    }
}

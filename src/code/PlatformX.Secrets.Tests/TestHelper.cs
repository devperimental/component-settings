﻿using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace PlatformX.Secrets.NTesting
{
    [ExcludeFromCodeCoverage]
    public class TestHelper
    {
        public static IConfigurationRoot GetIConfigurationRoot(string outputPath)
        {
            return new ConfigurationBuilder()
                .SetBasePath(outputPath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public static T GetConfiguration<T>(string outputPath, string sectionName) where T : new()
        {
            var configuration = new T();

            var configRoot = GetIConfigurationRoot(outputPath);

            configRoot
                .GetSection(sectionName)
                .Bind(configuration);

            return configuration;
        }

        
    }
}

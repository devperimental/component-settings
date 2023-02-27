﻿using Microsoft.Extensions.Configuration;

namespace PlatformX.Settings.NTesting
{
    public class TestHelper
    {
        public static IConfigurationRoot GetIConfigurationRoot(string outputPath)
        {
            return new ConfigurationBuilder()
                .SetBasePath(outputPath)
                .AddJsonFile("appsettings.json", optional: true)
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

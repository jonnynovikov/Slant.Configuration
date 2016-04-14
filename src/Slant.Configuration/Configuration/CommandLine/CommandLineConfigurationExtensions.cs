// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Slant.Extensions.Configuration.CommandLine;

namespace Slant.Extensions.Configuration
{
    /// <summary>
    /// Configuration extension for adding cmd args as a ConfigurationSource
    /// </summary>
    public static class CommandLineConfigurationExtensions
    {
        /// <summary>
        /// Shortcode for add cmdargs to ConfigurationBuilder
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddCommandLine(this IConfigurationBuilder configurationBuilder, string[] args)
        {
            return configurationBuilder.AddCommandLine(args, switchMappings: null);
        }

        /// <summary>
        /// Shortcode for add cmdargs to ConfigurationBuilder with SwitchMappings
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <param name="args"></param>
        /// <param name="switchMappings"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddCommandLine(
            this IConfigurationBuilder configurationBuilder,
            string[] args,
            IDictionary<string, string> switchMappings)
        {
            configurationBuilder.Add(new CommandLineConfigurationSource { Args = args, SwitchMappings = switchMappings });
            return configurationBuilder;
        }
    }
}

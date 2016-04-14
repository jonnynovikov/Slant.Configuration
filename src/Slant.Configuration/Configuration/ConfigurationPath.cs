// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Slant.Extensions.Configuration
{
    /// <summary>
    /// Utility methods and constants for manipulating Configuration paths
    /// </summary>
    public static class ConfigurationPath
    {
        /// <summary>
        /// Default segments delimiter
        /// </summary>
        public static readonly string KeyDelimiter = ":";

        /// <summary>
        /// Combine configuration path segments
        /// </summary>
        /// <param name="pathSegements"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string Combine(params string[] pathSegements)
        {
            if (pathSegements == null)
            {
                throw new ArgumentNullException(nameof(pathSegements));
            }
            return string.Join(KeyDelimiter, pathSegements);
        }

        /// <summary>
        /// Combine configuration path segments
        /// </summary>
        /// <param name="pathSegements"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string Combine(IEnumerable<string> pathSegements)
        {
            if (pathSegements == null)
            {
                throw new ArgumentNullException(nameof(pathSegements));
            }
            return string.Join(KeyDelimiter, pathSegements);
        }

        public static string GetSectionKey(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }

            var lastDelimiterIndex = path.LastIndexOf(KeyDelimiter, StringComparison.OrdinalIgnoreCase);
            return lastDelimiterIndex == -1 ? path : path.Substring(lastDelimiterIndex + 1);
        }

        public static string GetParentPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            var lastDelimiterIndex = path.LastIndexOf(KeyDelimiter, StringComparison.OrdinalIgnoreCase);
            return lastDelimiterIndex == -1 ? null : path.Substring(0, lastDelimiterIndex);
        }
    }
}

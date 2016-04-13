// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;

namespace Microsoft.Extensions.Configuration.Json
{
    /// <summary>
    /// A JSON file based
    /// </summary>
    public class JsonConfigurationSource : IConfigurationSource
    {
        private string _path;

        public string Path
        {
            get { return _path; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException(Resources.Error_InvalidFilePath, nameof(value));
                }
                _path = value;
            }
        }

        public Func<Stream> CreateStream { get; set; }

        public JsonConfigurationSource()
        {
            CreateStream = () =>
            {
                if (string.IsNullOrEmpty(Path) || !File.Exists(Path))
                {
                    if (!Optional)
                    {
                        throw new FileNotFoundException(Resources.FormatError_FileNotFound(Path), Path);
                    }
                    return null;
                }
                return new FileStream(Path, FileMode.Open, FileAccess.Read);
            };
        }

        /// <summary>
        /// Gets a value that determines if this instance of <see cref="JsonConfigurationProvider"/> is optional.
        /// </summary>
        public bool Optional { get; set; }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new JsonConfigurationProvider(this);
        }
    }
}
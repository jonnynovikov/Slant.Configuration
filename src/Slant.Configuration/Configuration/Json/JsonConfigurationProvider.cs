// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Slant.Extensions.Configuration.Json
{
    /// <summary>
    /// A JSON file based <see cref="ConfigurationProvider"/>.
    /// </summary>
    public class JsonConfigurationProvider : ConfigurationProvider
    {
        /// <summary>
        /// Initializes a new instance of <see cref="JsonConfigurationProvider"/>.
        /// </summary>
        /// <param name="source"></param>
        public JsonConfigurationProvider(JsonConfigurationSource source)
        {
            Source = source;
        }

        public JsonConfigurationSource Source { get; }

        /// <summary>
        /// Loads the contents
        /// </summary>
        public override void Load()
        {
            var stream = Source.CreateStream();
            if (stream == null)
            {
                Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                return;
            }
            using (stream)
            {
                Load(stream);
            }
        }

        public void Load(Stream stream)
        {
            JsonConfigurationFileParser parser = new JsonConfigurationFileParser();
            try
            {
                Data = parser.Parse(stream);
            }
            catch (JsonReaderException e)
            {
                string errorLine = string.Empty;
                if (stream.CanSeek)
                {
                    stream.Seek(0, SeekOrigin.Begin);

                    using (var streamReader = new StreamReader(stream))
                    {
                        var fileContent = ReadLines(streamReader);
                        errorLine = RetrieveErrorContext(e, fileContent);
                    }
                }

                throw new FormatException(Resources.FormatError_JSONParseError(e.LineNumber, errorLine), e);
            }
        }

        private static string RetrieveErrorContext(JsonReaderException e, IEnumerable<string> fileContent)
        {
            string errorLine;
            if (e.LineNumber >= 2)
            {
                var errorContext = fileContent.Skip(e.LineNumber - 2).Take(2).ToList();
                errorLine = errorContext[0].Trim() + Environment.NewLine + errorContext[1].Trim();
            }
            else
            {
                var possibleLineContent = fileContent.Skip(e.LineNumber - 1).FirstOrDefault();
                errorLine = possibleLineContent ?? string.Empty;
            }

            return errorLine;
        }

        private static IEnumerable<string> ReadLines(StreamReader streamReader)
        {
            string line;
            do
            {
                line = streamReader.ReadLine();
                yield return line;
            } while (line != null);
        }
    }
}

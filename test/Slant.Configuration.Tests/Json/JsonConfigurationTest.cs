// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using NUnit.Framework;
using Slant.Extensions.Configuration.Json;
using Slant.Extensions.Configuration.Tests;

namespace Slant.Extensions.Configuration
{
    [TestFixture]
    public class JsonConfigurationTest
    {
        private JsonConfigurationProvider LoadProvider(string json)
        {
            var p = new JsonConfigurationProvider(new JsonConfigurationSource { Optional = true });
            p.Load(TestStreamHelpers.StringToStream(json));
            return p;
        }

        [Test]
        public void LoadKeyValuePairsFromValidJson()
        {
            var json = @"
{
    'firstname': 'test',
    'test.last.name': 'last.name',
        'residential.address': {
            'street.name': 'Something street',
            'zipcode': '12345'
        }
}";
            var jsonConfigSrc = LoadProvider(json);

            Assert.AreEqual("test", jsonConfigSrc.Get("firstname"));
            Assert.AreEqual("last.name", jsonConfigSrc.Get("test.last.name"));
            Assert.AreEqual("Something street", jsonConfigSrc.Get("residential.address:STREET.name"));
            Assert.AreEqual("12345", jsonConfigSrc.Get("residential.address:zipcode"));
        }

        [Test]
        public void LoadMethodCanHandleEmptyValue()
        {
            var json = @"
{
    'name': ''
}";
            var jsonConfigSrc = LoadProvider(json);
            Assert.AreEqual(string.Empty, jsonConfigSrc.Get("name"));
        }

        [Test]
        public void NonObjectRootIsInvalid()
        {
            var json = @"'test'";

            var exception = Assert.Throws<FormatException>(
                () => LoadProvider(json));

            Assert.NotNull(exception.Message);
        }

        [Test]
        public void SupportAndIgnoreComments()
        {
            var json = @"/* Comments */
                {/* Comments */
                ""name"": /* Comments */ ""test"",
                ""address"": {
                    ""street"": ""Something street"", /* Comments */
                    ""zipcode"": ""12345""
                }
            }";
            var jsonConfigSrc = LoadProvider(json);
            Assert.AreEqual("test", jsonConfigSrc.Get("name"));
            Assert.AreEqual("Something street", jsonConfigSrc.Get("address:street"));
            Assert.AreEqual("12345", jsonConfigSrc.Get("address:zipcode"));
        }

        [Test]
        public void ThrowExceptionWhenUnexpectedEndFoundBeforeFinishParsing()
        {
            var json = @"{
                'name': 'test',
                'address': {
                    'street': 'Something street',
                    'zipcode': '12345'
                }
            /* Missing a right brace here*/";
            var exception = Assert.Throws<FormatException>(() => LoadProvider(json));
            Assert.NotNull(exception.Message);
        }

        [Test]
        public void ThrowExceptionWhenPassingNullAsFilePath()
        {
            var expectedMsg = new ArgumentException(Resources.Error_InvalidFilePath, "path").Message;

            var exception = Assert.Throws<ArgumentException>(() => new ConfigurationBuilder().AddJsonFile(path: null));

            Assert.AreEqual(expectedMsg, exception.Message);
        }

        [Test]
        public void ThrowExceptionWhenPassingEmptyStringAsFilePath()
        {
            var expectedMsg = new ArgumentException(Resources.Error_InvalidFilePath, "path").Message;

            var exception = Assert.Throws<ArgumentException>(() => new ConfigurationBuilder().AddJsonFile(string.Empty));

            Assert.AreEqual(expectedMsg, exception.Message);
        }

        [Test]
        public void JsonConfiguration_Throws_On_Missing_Configuration_File()
        {
            var config = new ConfigurationBuilder().AddJsonFile("NotExistingConfig.json", optional: false);
            var exception = Assert.Throws<FileNotFoundException>(() => config.Build());

            // Assert
            Assert.AreEqual(Resources.FormatError_FileNotFound("NotExistingConfig.json"), exception.Message);
        }

        [Test]
        public void JsonConfiguration_Does_Not_Throw_On_Optional_Configuration()
        {
            var config = new ConfigurationBuilder().AddJsonFile("NotExistingConfig.json", optional: true).Build();
        }

        [Test]
        public void ThrowFormatExceptionWhenFileIsEmpty()
        {
            var exception = Assert.Throws<FormatException>(() => LoadProvider(@""));
        }
    }
}
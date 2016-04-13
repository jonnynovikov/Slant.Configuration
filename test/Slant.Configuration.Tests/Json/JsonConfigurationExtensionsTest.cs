// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;

namespace Microsoft.Extensions.Configuration.Json
{
    public class JsonConfigurationExtensionsTest
    {
        [Theory]
        [TestCase(null)]
        [TestCase("")]
        public void AddJsonFile_ThrowsIfFilePathIsNullOrEmpty(string path)
        {
            // Arrange
            var configurationBuilder = new ConfigurationBuilder();

            // Act and Assert
            var ex = Assert.Throws<ArgumentException>(() => configurationBuilder.AddJsonFile(path));
            Assert.AreEqual("path", ex.ParamName);
            ex.Message.Should().StartWith("File path must be a non-empty string.");
        }

        [Test]
        public void AddJsonFile_ThrowsIfFileDoesNotExistAtPath()
        {
            // Arrange
            var path = "file-does-not-exist.json";

            // Act and Assert
            var ex = Assert.Throws<FileNotFoundException>(() => new ConfigurationBuilder().AddJsonFile(path).Build());
            Assert.AreEqual($"The configuration file '{path}' was not found and is not optional.", ex.Message);
        }
    }
}

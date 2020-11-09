// The MIT License (MIT)

// Copyright (c) 2014 Ben Abelshausen

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.IO;
using GTFS.Exceptions;
using GTFS.IO;
using NUnit.Framework;

namespace GTFS.Test
{
    [TestFixture]
    internal class FileNotDisposedTest
    {
        private static DirectoryInfo _assemblyLocation;

        private static DirectoryInfo AssemblyLocation
        {
            get
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                return _assemblyLocation ?? (_assemblyLocation = new DirectoryInfo(Path.GetDirectoryName(typeof(FileNotDisposedTest).Assembly.Location)));
            }
        }

        [Test]
        public void NotDisposingDirectorySourceKeepsSourceFilesOpen()
        {
            var directoryInfo = new DirectoryInfo(Path.Combine(AssemblyLocation.FullName, "folder-feed"));

            try
            {
                var gtfsDirectorySource = new GTFSDirectorySource(directoryInfo.FullName);
                var reader = new GTFSReader<GTFSFeed>();
                reader.Read(gtfsDirectorySource);
            }
            catch (GTFSExceptionBase)
            {
                // ignore our exceptions
            }

            var agencyFile = Path.Combine(directoryInfo.FullName, "agency.txt");

            Assert.Throws<IOException>(
                () =>
                {
                    using (File.OpenWrite(agencyFile))
                    {
                        // do nothing
                    }
                },
                "The process cannot access the file '{0}' because it is being used by another process.",
                agencyFile);
        }

        [Test]
        public void DisposingDirectorySourceClosesSourceFiles()
        {
            var directoryInfo = new DirectoryInfo(Path.Combine(AssemblyLocation.FullName, "folder-feed"));

            try
            {
                using (var gtfsDirectorySource = new GTFSDirectorySource(directoryInfo.FullName))
                {
                    var reader = new GTFSReader<GTFSFeed>();
                    reader.Read(gtfsDirectorySource);
                }
            }
            catch (GTFSExceptionBase)
            {
                // ignore our exceptions
            }

            var agencyFile = Path.Combine(directoryInfo.FullName, "agency.txt");

            using (File.OpenWrite(agencyFile))
            {
                // do nothing
            }
        }
    }
}
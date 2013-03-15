using System;
using NUnit.Framework;
using System.Management;

namespace TestHost
{
    [TestFixture]
    public class PathTests
    {
        
        [Test]
        [TestCase("","", "Should do nothing with an empty string")]
        [TestCase("\\","/", "Should change the single root path slash")]
        [TestCase("/","/", "should not change the correct root path slash")]
        [TestCase("\\foo/bar\\baz","/foo/bar/baz", "Should change all incorrect slashes")]
        public void NormalizePathForUnix(string input, string expected, string failureMessage)
        {
            var inputpath = SetUnixPaths(input);
            var expectedPath = SetUnixPaths(expected);
            
            inputpath.NormalizeSlashes()
                .ShouldEqual(expectedPath, failureMessage);
        }

        [Test]
        [TestCase("","", "Should do nothing with an empty string")]
        [TestCase("/","\\", "Should change the single root path slash")]
        [TestCase("\\","\\", "should not change the correct root path slash")]
        [TestCase("/foo\\bar/baz","\\foo\\bar\\baz", "Should change all incorrect slashes")]
        public void NormalizePathForWindows(string input, string expected, string failureMessage)
        {
            var inputPath = SetWindowsPaths(input);
            var expectedPath = SetWindowsPaths(expected);

            inputPath.NormalizeSlashes()
                .ShouldEqual(expectedPath, failureMessage);
        }
        
        
        [Test]
        [TestCase("", "", "", "")]
        [TestCase("/", "/", "", "if path and root are the get parent result should be an empty string")]
        [TestCase("\\", "\\", "", "if path and root are the get parent result should be an empty string")]
        [TestCase("/", "\\", "", "if path and root are the get parent result should be an empty string")]
        [TestCase("/", "/foo", "", "if path and root are the get parent result should be an empty string")] // TODO: this doesn't make much sense (yet)
        [TestCase("/", "/foo/bar", "/foo", null)]
        [TestCase("/", "\\foo/bar", "/foo", null)]
        public void GetParentPathForUnix(string root, string input, string expected, string failureMessage)
        {
            var rootPath = SetUnixPaths(root);
            var inputPath = SetUnixPaths(input);
            var expectedPath = SetUnixPaths(expected);
            
            inputPath.GetParentPath(rootPath)
                .ShouldEqual(expectedPath, failureMessage);
        }

        [Test]
        [TestCase("", "", "", "")]
        [TestCase("\\", "\\", "", "if path and root are the get parent result should be an empty string")]
        [TestCase("/", "\\", "", "if path and root are the get parent result should be an empty string")]
        [TestCase("/", "/", "", "if path and root are the get parent result should be an empty string")]
        [TestCase("/", "\\foo", "", "if path and root are the get parent result should be an empty string")] // TODO: this doesn't make much sense (yet)
        [TestCase("/", "\\foo\\bar", "\\foo", null)]
        [TestCase("/", "/foo\\bar", "\\foo", null)]
        public void GetParentPathForWindows(string root, string input, string expected, string failureMessage)
        {
            var rootPath = SetWindowsPaths(root);
            var inputPath = SetWindowsPaths(input);
            var expectedPath = SetWindowsPaths(expected);
            
            inputPath.GetParentPath(rootPath)
                .ShouldEqual(expectedPath, failureMessage);
        }


        private Path SetUnixPaths(Path path)
        {
            path.CorrectSlash = "/";
            path.WrongSlash = "\\";
            return path;
        }
        private Path SetWindowsPaths(Path path)
        {
            path.CorrectSlash = "\\";
            path.WrongSlash = "/";
            return path;
        }

    }
}

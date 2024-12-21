﻿using NGeoNames;
using System.Text;

namespace NGeoNames.Tests;

[TestClass]
public class GeoFileReaderTests
{
    [TestMethod]
    public void GeoFileReader_ParsesFileCorrectly1()
    {
        var gf = new GeoFileReader();
        var target = gf.ReadRecords(@"testdata\test_geofilereadercustom1.txt", new CustomParser(9, 1, [','], Encoding.UTF8, true)).ToArray();
        Assert.AreEqual(2, target.Length);
    }

    [TestMethod]
    public void GeoFileReader_ParsesFileCorrectly2()
    {
        var gf = new GeoFileReader();
        var target = gf.ReadRecords(@"testdata\test_geofilereadercustom2.txt", new CustomParser(4, 0, ['!'], Encoding.UTF8, false)).ToArray();
        Assert.AreEqual(3, target.Length);
    }

    [TestMethod]
    [ExpectedException(typeof(NotSupportedException))]
    public void GeoFileReader_ThrowsOnFailureWhenAutodetectingFileType()
    {
        // When filetype == autodetect and an unknown extension is used an exception should be thrown
        var gf = new GeoFileReader();
        var target = gf.ReadRecords(@"testdata\invalid.ext", new CustomParser(5, 0, ['\t'], Encoding.UTF8, false)).ToArray();
    }

    [TestMethod]
    public void GeoFileReader_DoesNotThrowOnInvalidExtensionButSpecifiedFileType()
    {
        // When filetype is specified and an unknown extension is used it should be read fine
        var gf = new GeoFileReader();
        var target = gf.ReadRecords(@"testdata\invalid.ext", FileType.Plain, new CustomParser(5, 0, ['\t'], Encoding.UTF8, false)).ToArray();
    }

    [TestMethod]
    [ExpectedException(typeof(NotSupportedException))]
    public void GeoFileReader_ThrowsOnUnknownSpecifiedFileType()
    {
        // When and unknown filetype is specified an exception should be thrown
        var gf = new GeoFileReader();
        var target = gf.ReadRecords(@"testdata\invalid.ext", (FileType)999, new CustomParser(5, 0, ['\t'], Encoding.UTF8, false)).ToArray();
    }
}

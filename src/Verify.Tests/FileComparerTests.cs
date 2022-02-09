﻿[Serial]
[UsesVerify]
public class FileComparerTests
{
    [Fact]
    public async Task BinaryEquals()
    {
        File.Copy("sample.bmp", "sample.tmp", true);
        try
        {
            var result = await FileComparer.DefaultCompare(new(), new("txt", "BinaryEquals"));
            Assert.True(result.IsEqual);
        }
        finally
        {
            File.Delete("sample.tmp");
        }
    }

    [Fact]
    public async Task VerifyFilePath()
    {
        var path = "lock.txt";
        try
        {
            File.WriteAllText(path, "content");
            await VerifyFile(path);
            Assert.False(FileEx.IsFileLocked(path));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public async Task BinaryNotEqualsSameLength()
    {
        File.Copy("sample.bmp", "sample.tmp", true);
        using (var stream = File.Open("sample.tmp", FileMode.Open))
        {
            stream.Position = 100;
            stream.WriteByte(8);
        }

        try
        {
            var result = await FileComparer.DefaultCompare(new(), new("txt", "BinaryNotEqualsSameLength"));
            Assert.False(result.IsEqual);
        }
        finally
        {
            File.Delete("sample.tmp");
        }
    }

    [Fact]
    public async Task BinaryNotEquals()
    {
        var result = await FileComparer.DefaultCompare(new(), new("txt", "BinaryNotEquals"));
        Assert.False(result.IsEqual);
    }

    [Fact]
    public async Task ShouldNotLock()
    {
        File.Copy("sample.bmp", "sample.tmp", true);
        try
        {
            using (
                new FileStream(
                    "sample.bmp",
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read))
            {
                var result = await FileComparer.DefaultCompare(new(), new("txt", "BinaryEquals"));
                Assert.True(result.IsEqual);
            }
        }
        finally
        {
            File.Delete("sample.tmp");
        }
    }
}
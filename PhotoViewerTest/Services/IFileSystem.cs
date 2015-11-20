using System;
using System.IO;

namespace PhotoViewerTest
{
    public interface IFileSystem
    {
        bool FileExists(string fileName);
        bool SaveBinaryFile(string fileName, Stream data);
        byte[] LoadBinary(string fileName);
        string GetFilePath(string fileName);
    }
}
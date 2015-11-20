using System;
using PhotoViewerTest.Droid;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(FileSystem))]
namespace PhotoViewerTest.Droid
{
    public class FileSystem : IFileSystem
    {
        public bool FileExists(string fileName)
        {
            var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), fileName);

            return System.IO.File.Exists(path);
        }

        public bool SaveBinaryFile(string fileName, Stream data)
        {
            var context = Android.App.Application.Context;
            Stream stream = context.OpenFileOutput(fileName, Android.Content.FileCreationMode.Private);

            return SaveBinaryResource(stream, data);
        }

        public byte[] LoadBinary(string fileName)
        {
            try {
                return System.IO.File.ReadAllBytes(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), fileName));
            } catch {
                return null;
            }
        }

        public string GetFilePath(string fileName)
        {
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), fileName);
        }

        private bool SaveBinaryResource(Stream stream, Stream data)
        {
            data.CopyTo(stream);

            stream.Close();

            return true;
        }
    }
}
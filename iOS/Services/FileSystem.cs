using System;
using PhotoViewerTest.iOS;
using Foundation;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(FileSystem))]
namespace PhotoViewerTest.iOS
{
    public class FileSystem : IFileSystem
    {
        public bool FileExists(string fileName)
        {
            var docUrl = applicationDocumentsDirectory().Append(fileName, false);
            var filePath = docUrl.AbsoluteString.Replace("file://", "");

            return File.Exists(filePath);
        }

        public bool SaveBinaryFile(string fileName, Stream data)
        {
            var docUrl = applicationDocumentsDirectory().Append(fileName, false);
            return SaveBinaryResource(docUrl.AbsoluteString.Replace("file://", ""), data);
        }

        public byte[] LoadBinary(string fileName)
        {
            var docUrl = applicationDocumentsDirectory().Append(fileName, false);
            var filePath = docUrl.AbsoluteString.Replace("file://", "");
            return LoadBinaryResource(filePath);
        }

        public string GetFilePath(string fileName)
        {
            var docUrl = applicationDocumentsDirectory().Append(fileName, false);

            return docUrl.AbsoluteString;
        }

        NSUrl applicationDocumentsDirectory() {
            return NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User)[0];
        }

        private bool SaveBinaryResource(string filePath, Stream data)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                data.CopyTo(fileStream);
            }

            return true;
        }

        private byte[] LoadBinaryResource(string filePath)
        {
            if (!File.Exists(filePath)) {
                return null;
            }

            return File.ReadAllBytes(filePath);
        }
    }
}
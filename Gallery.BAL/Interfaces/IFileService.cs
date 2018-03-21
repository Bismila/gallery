using System;
using System.IO;

namespace Gallery.BAL.Interfaces
{
    public interface IFileService
    {
        string UploadFile(Stream stream, string fileName, long userId);

        void DeleteFile(string filePath, long userId);

        void DeleteAllFile(long userId);
    }
}
